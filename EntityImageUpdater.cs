using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XrmToolBox.Extensibility;
using XrmToolBox.Helpers;
using Microsoft.Xrm.Sdk.Metadata;
using XrmToolBox.Common;
using XrmToolBox.Extensibility.Interfaces;
using System.ComponentModel.Composition;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System.Net.Http;
using System.Security.Cryptography;
using Cinteros.Xrm.FetchXmlBuilder;
using System.IO;
using Microsoft.Crm.Sdk.Messages;

namespace Ryr.XrmToolBox.EntityImageUpdater
{
    public partial class EntityImageUpdater : PluginControlBase, IMessageBusHost
    {
        private List<EntityMetadata> entitiesCache;
        private ListViewItem[] listViewItemsCache;
        private List<Tuple<Guid, string, byte[]>> imageCache;
        private string selectedFolder;
        readonly List<string> imageTypes = new List<string> { "png", "jpg", "jpeg" };

        public event EventHandler<MessageBusEventArgs> OnOutgoingMessage;

        public EntityImageUpdater()
        {
            imageCache = new List<Tuple<Guid, string, byte[]>>();
            InitializeComponent();
        }

        private void tsbLoadEntities_Click(object sender, EventArgs e)
        {
            ExecuteMethod(LoadEntities);
        }

        private void LoadEntities()
        {
            lvEntities.Items.Clear();
            gbEntities.Enabled = false;

            lvAttributes.Items.Clear();
            lvResults.Items.Clear();

            Cursor = Cursors.WaitCursor;
            WorkAsync("Loading entities...",
                e =>
                {
                    e.Result = MetadataHelper.RetrieveEntities(Service);
                },
                e =>
                {
                    Cursor = Cursors.Default;
                    if (e.Error != null)
                    {
                        string errorMessage = CrmExceptionHelper.GetErrorMessage(e.Error, true);
                        CommonDelegates.DisplayMessageBox(ParentForm, errorMessage, "Error", MessageBoxButtons.OK,
                                                          MessageBoxIcon.Error);
                    }
                    else
                    {
                        entitiesCache = (List<EntityMetadata>)e.Result;
                        lvEntities.Items.Clear();
                        var list = new List<ListViewItem>();
                        foreach (EntityMetadata emd in (List<EntityMetadata>)e.Result)
                        {
                            var item = new ListViewItem { Text = emd.DisplayName.UserLocalizedLabel.Label, Tag = emd.LogicalName };
                            item.SubItems.Add(emd.LogicalName);
                            list.Add(item);
                        }

                        this.listViewItemsCache = list.ToArray();
                        lvEntities.Items.AddRange(listViewItemsCache);

                        gbEntities.Enabled = true;
                    }
                });
        }

        private void lvEntities_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            lvEntities.Sorting = lvEntities.Sorting == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
            lvEntities.ListViewItemSorter = new ListViewItemComparer(e.Column, lvEntities.Sorting);

            //HACK: Have to figure out why clicking entity sort column twice in rapid sucession fires lvEntities_SelectedIndexChanged and hangs
            //This is still not perfect, but doesn't hang with Loading text attributes
            lvEntities.SelectedItems.Clear();
            lvAttributes.Items.Clear();
        }

        private void lvEntities_SelectedIndexChanged(object sender, EventArgs e)
        {
            gbLogoSource.Enabled = true;
            rbClearbit.Checked = false;
            rbFileSystem.Checked = false;
            rbGravatar.Checked = false;
            rbTwitter.Checked = false;
            tsbUpdateImages.Enabled = false;
            selectedFolder = string.Empty;
            imageCache.Clear();
            lvAttributes.Items.Clear();
            lvResults.Items.Clear();
            txtLogoFrom.Text = "";
        }

        private void RetrieveTextAttributeByType(string entityLogicalName, SourceDataType sourceDataType)
        {
            Cursor = Cursors.WaitCursor;
            WorkAsync(string.Format("Loading attributes valid for {0}...", sourceDataType),
                dwea =>
                {
                    dwea.Result = MetadataHelper.RetrieveEntity(entityLogicalName, Service);
                    var entityMetadata = MetadataHelper.RetrieveEntity(entityLogicalName, Service);
                    switch (sourceDataType)
                    {
                        case SourceDataType.Url:
                            dwea.Result = entityMetadata.Attributes.Where(x => x.AttributeType.Value == AttributeTypeCode.String
                            && ((StringAttributeMetadata)x).Format.Value == Microsoft.Xrm.Sdk.Metadata.StringFormat.Url);
                            break;
                        case SourceDataType.Email:
                            dwea.Result = entityMetadata.Attributes.Where(x => x.AttributeType.Value == AttributeTypeCode.String
                            && ((StringAttributeMetadata)x).Format.Value == Microsoft.Xrm.Sdk.Metadata.StringFormat.Email);
                            break;
                        case SourceDataType.TwitterHandle:
                        case SourceDataType.Folder:
                            dwea.Result = entityMetadata.Attributes.Where(x => x.AttributeType.Value == AttributeTypeCode.String
                            && ((StringAttributeMetadata)x).Format.Value == Microsoft.Xrm.Sdk.Metadata.StringFormat.Text);
                            break;
                        default:
                            break;
                    }
                },
                dwea =>
                {
                    Cursor = Cursors.Default;
                    if (dwea.Error != null)
                    {
                        string errorMessage = CrmExceptionHelper.GetErrorMessage(dwea.Error, true);
                        CommonDelegates.DisplayMessageBox(ParentForm, errorMessage, "Error", MessageBoxButtons.OK,
                                                          MessageBoxIcon.Error);
                    }
                    else
                    {
                        var attributeMetadata = (IEnumerable<AttributeMetadata>)dwea.Result;
                        foreach (var attribute in attributeMetadata)
                        {
                            if (attribute.DisplayName.LocalizedLabels.Any())
                            {
                                var item = new ListViewItem { Text = attribute.DisplayName.LocalizedLabels[0].Label, Tag = attribute.LogicalName };
                                item.SubItems.Add(attribute.SchemaName);
                                item.SubItems.Add(attribute.Description.LocalizedLabels[0].Label);
                                item.SubItems.Add(entityLogicalName);
                                ListViewDelegates.AddItem(lvAttributes, item);
                            }
                        }
                    }
                });
        }

        private void lvAttributes_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            lvAttributes.Sorting = lvAttributes.Sorting == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
            lvAttributes.ListViewItemSorter = new ListViewItemComparer(e.Column, lvAttributes.Sorting);
        }

        private void tsbUpdateImages_Click(object sender, EventArgs e)
        {
            UpdateImages();
        }

        private void UpdateImages(QueryExpression query = null)
        {
            var selectedAttribute = ListViewDelegates.GetSelectedItems(lvAttributes)[0];
            var attributeSchemaName = selectedAttribute.Tag.ToString();
            var entityName = selectedAttribute.SubItems[3].Text;
            Cursor = Cursors.WaitCursor;

            WorkAsync(string.Format("Retrieving records and {0} uploading logo...", query != null ? "selectively" : ""),
                dwea =>
                {
                    var entityRecords = query != null ? 
                    RetrieveAllPages(query, attributeSchemaName) : RetrieveAllPages(entityName, attributeSchemaName);
                    dwea.Result = UpdateImages(entityRecords, attributeSchemaName);
                },
                dwea =>
                {
                    Cursor = Cursors.Default;
                    tsbUpdateImages.Enabled = false;
                    tsbFxbEdit.Enabled = false;
                    if (dwea.Error != null)
                    {
                        string errorMessage = CrmExceptionHelper.GetErrorMessage(dwea.Error, true);
                        CommonDelegates.DisplayMessageBox(ParentForm, errorMessage, "Error", MessageBoxButtons.OK,
                                                            MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show(string.Format("{0} images updated", imageCache.Count),"Success",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
                    }
                });
        }

        private List<Tuple<Guid, string, byte[]>> UpdateImages(List<Entity> entityRecords, string attribute)
        {
            imageCache.Clear();
            byte[] image = null;

            var imageList = new ImageList();
            imageList.ImageSize = new Size(40, 40);
            lvResults.SmallImageList = imageList;

            var imageIndex = 0;
            var requests = 0;

            ExecuteMultipleRequest executeMultipleRequests = new ExecuteMultipleRequest();
            executeMultipleRequests.Settings = new ExecuteMultipleSettings { ContinueOnError = true, ReturnResponses = true };
            executeMultipleRequests.Requests = new OrganizationRequestCollection();

            foreach (var entity in entityRecords)
            {
                var attributeValue = entity.GetAttributeValue<string>(attribute);
                if (string.IsNullOrEmpty(attributeValue)) continue;
                //Check if we already have retrived the logo for this value
                if (!imageCache.Any(x => x.Item2 == attributeValue))
                {
                    var imageTask = GetLogo(attributeValue);
                    imageTask.Wait();
                    image = imageTask.Result;
                    if (image != null) imageCache.Add(new Tuple<Guid, string, byte[]>(entity.Id, attributeValue, image));
                }
                image = imageCache.FirstOrDefault(x => x.Item2 == attributeValue)?.Item3;
                if (image != null)
                {
                    entity["entityimage"] = image;
                    executeMultipleRequests.Requests.Add(new UpdateRequest { Target = entity });
                    requests++;

                    if (requests == 100 || entityRecords.Count == requests)
                    {
                        ExecuteMultipleUpdates(imageCache, executeMultipleRequests);
                        executeMultipleRequests.Requests.Clear();
                        requests = 0;
                    }
                }
            }

            //Processs any leftover requests
            if (executeMultipleRequests.Requests.Any()) ExecuteMultipleUpdates(imageCache, executeMultipleRequests);

            foreach (var updatedImage in imageCache)
            {
                var imageListImage = (Bitmap)((new ImageConverter()).ConvertFrom(updatedImage.Item3));
                imageList.Images.Add(imageListImage);
                var listViewItem = new ListViewItem(updatedImage.Item2);
                listViewItem.ImageIndex = imageIndex++;
                lvResults.Items.Add(listViewItem);
            }
            return imageCache;
        }

        private void ExecuteMultipleUpdates(List<Tuple<Guid, string, byte[]>> imagesToUpdate, ExecuteMultipleRequest executeMultipleRequests)
        {
            var executeMultipleResponses = (ExecuteMultipleResponse)Service.Execute(executeMultipleRequests);
            foreach (var executeMultipleResponse in executeMultipleResponses.Responses)
            {
                var updateResponse = (UpdateResponse)executeMultipleResponse.Response;
                if (executeMultipleResponse.Fault != null) imagesToUpdate.RemoveAll(x => x.Item1 == ((UpdateRequest)executeMultipleRequests.Requests[executeMultipleResponse.RequestIndex]).Target.Id);
            }
        }

        private async Task<byte[]> GetLogo(string attributeValue)
        {
            var selectedLogoSource = gbLogoSource.Controls.OfType<RadioButton>().FirstOrDefault(x => x.Checked)?.Tag.ToString();
            var logoSource = (LogoSource)Enum.Parse(typeof(LogoSource), selectedLogoSource);
            return await RetrieveLogoFromSelectedApi(logoSource, attributeValue);
        }

        private async Task<byte[]> RetrieveLogoFromSelectedApi(LogoSource logoSource, string attributeValue)
        {
            var baseAddress = string.Empty;
            var relativeAddress = string.Empty;
            switch (logoSource)
            {
                case LogoSource.Clearbit:
                    baseAddress = "https://logo.clearbit.com/";
                    if (attributeValue.IndexOf("http") > -1)
                        relativeAddress = string.Format("{0}?size=144",new Uri(attributeValue).Host);
                    break;
                case LogoSource.Gravatar:
                    baseAddress = "http://www.gravatar.com/";
                    relativeAddress = string.Format("/avatar/{0}?s=144&&d=404", CalculateMD5Hash(attributeValue));
                    break;
                case LogoSource.Twitter:
                    baseAddress = "https://twitter.com/";
                    relativeAddress = string.Format("/{0}/profile_image?size=normal", attributeValue);
                    break;
            }
            if ((string.IsNullOrEmpty(baseAddress) || string.IsNullOrEmpty(relativeAddress)) 
                && logoSource == LogoSource.Folder && !string.IsNullOrEmpty(selectedFolder))
            {
                foreach (var imageType in imageTypes)
                {
                    var imageFullPath = string.Format("{0}.{1}", Path.Combine(selectedFolder, attributeValue),imageType);
                    if (File.Exists(imageFullPath))
                    {
                        return File.ReadAllBytes(imageFullPath);
                    }
                }
                return null;
            }
            else
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseAddress);
                    client.DefaultRequestHeaders.Accept.Clear();

                    HttpResponseMessage response = await client.GetAsync(relativeAddress);
                    if (response.IsSuccessStatusCode)
                    {
                        var r = await response.Content.ReadAsByteArrayAsync();
                        return r;
                    }
                    return null;
                }
            }
        }

        //https://msdn.microsoft.com/en-us/library/s02tk69a%28v=vs.110%29.aspx
        //MD5 hash computation
        private static string CalculateMD5Hash(string email)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                email = email.Trim().ToLower();
                var hashBytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(email));
                StringBuilder hash = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    hash.Append(hashBytes[i].ToString("x2"));
                }
                return hash.ToString();
            }
        }
        private List<Entity> RetrieveAllPages(QueryExpression query)
        {
            EntityCollection results = null;
            List<Entity> allPagesAllRows = new List<Entity>();
            query.PageInfo = new PagingInfo();
            if (query.PageInfo.Count == 0)
            {
                query.PageInfo = new PagingInfo
                {
                    PageNumber = 1,
                    Count = 500,
                    PagingCookie = null
                };
            }
            do
            {
                results = Service.RetrieveMultiple(query);
                allPagesAllRows.AddRange(results.Entities.ToList());
                query.PageInfo.PageNumber++;
                query.PageInfo.PagingCookie = results.PagingCookie;
            } while (results.MoreRecords);

            return allPagesAllRows;
        }

        private List<Entity> RetrieveAllPages(QueryExpression query, string attribute)
        {
            query.ColumnSet = new ColumnSet(attribute);
            return RetrieveAllPages(query);
        }

        private List<Entity> RetrieveAllPages(string entityName, string attribute)
        {
            var query = new QueryExpression(entityName);
            query.ColumnSet = new ColumnSet(attribute);
            //TODO: Don't assume statecode always exists. Probably use metadata to check if statecode exists
            query.Criteria.AddCondition("statecode", ConditionOperator.Equal, 0);
            return RetrieveAllPages(query);
        }

        private void lvAttributes_SelectedIndexChanged(object sender, EventArgs e)
        {
            imageCache.Clear();
            lvResults.Clear();
            var selectedAttributes = ListViewDelegates.GetSelectedItems(lvAttributes);
            if (selectedAttributes.Any())
            {
                tsbUpdateImages.Enabled = true;
                tsbFxbEdit.Enabled = true;
            }
        }

        private void OnSearchKeyUp(object sender, KeyEventArgs e)
        {
            var entityName = txtSearchEntity.Text;
            if (string.IsNullOrWhiteSpace(entityName))
            {
                lvEntities.BeginUpdate();
                lvEntities.Items.Clear();
                lvEntities.Items.AddRange(listViewItemsCache);
                lvEntities.EndUpdate();
            }
            else
            {
                lvEntities.BeginUpdate();
                lvEntities.Items.Clear();
                var filteredItems = listViewItemsCache
                    .Where(item => item.Text.StartsWith(entityName, StringComparison.OrdinalIgnoreCase))
                    .ToArray();
                lvEntities.Items.AddRange(filteredItems);
                lvEntities.EndUpdate();
            }
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }

        private void LogoSource_Selected(object sender, EventArgs e)
        {
            if (!((RadioButton)sender).Checked) return;

            var source = ((RadioButton)sender).Tag.ToString();
            var entityLogicalName = lvEntities.SelectedItems[0].Tag.ToString();
            gbAttributes.Enabled = true;
            lvAttributes.Items.Clear();
            lvResults.Items.Clear();
            switch (source)
            {
                case "Clearbit":
                    txtLogoFrom.Text = "clearbit.com";
                    // Reinit other controls
                    RetrieveTextAttributeByType(entityLogicalName, SourceDataType.Url);
                    break;
                case "Twitter":
                    txtLogoFrom.Text = "twitter.com";
                    RetrieveTextAttributeByType(entityLogicalName, SourceDataType.TwitterHandle);
                    break;
                case "Gravatar":
                    txtLogoFrom.Text = "gravatar.com";
                    RetrieveTextAttributeByType(entityLogicalName, SourceDataType.Email);
                    break;
                case "Folder":
                    RetrieveTextAttributeByType(entityLogicalName, SourceDataType.Folder);
                    using (var folderDialog = new FolderBrowserDialog())
                    {
                        if (folderDialog.ShowDialog() == DialogResult.OK)
                        {
                            txtLogoFrom.Text = folderDialog.SelectedPath;
                            selectedFolder = folderDialog.SelectedPath;
                        }
                    }
                    break;
            }
        }

        private void tsbFxbEdit_Click(object sender, EventArgs e)
        {
            var selectedAttribute = lvAttributes.SelectedItems[0];
            var attributeSchemaName = selectedAttribute.Tag.ToString();
            var entityName = selectedAttribute.SubItems[3].Text;
            var fetchXml = string.Format(@"
                    <fetch count='500' >
                        <entity name='{0}' >
                        <attribute name='{1}' />
                        <filter>
                            <condition attribute='statecode' operator='eq' value='0' />
                        </filter>
                        </entity>
                    </fetch>", entityName, attributeSchemaName);
            var messageBusEventArgs = new MessageBusEventArgs("FetchXML Builder");
            var fXBMessageBusArgument = new FXBMessageBusArgument(FXBMessageBusRequest.FetchXML)
            {
                FetchXML = fetchXml
            };
            messageBusEventArgs.TargetArgument = fXBMessageBusArgument;
            OnOutgoingMessage(this, messageBusEventArgs);
        }

        public void OnIncomingMessage(MessageBusEventArgs message)
        {
            if (message.SourcePlugin == "FetchXML Builder" &&
                        message.TargetArgument is FXBMessageBusArgument)
            {
                var fxbArg = (FXBMessageBusArgument)message.TargetArgument;
                //TODO: Find out why fxbArg.QueryExpression is blank, so have to manually convert to fetch to query
                var queryExpressionResponse = (FetchXmlToQueryExpressionResponse)Service.Execute(new FetchXmlToQueryExpressionRequest { FetchXml = fxbArg.FetchXML });
                UpdateImages(queryExpressionResponse.Query);
            }
        }
    }

    enum SourceDataType
    {
        Url, Email, TwitterHandle, Folder
    }
    enum LogoSource
    {
        Clearbit, Gravatar, Twitter, Folder
    }
}
