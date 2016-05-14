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
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Ryr.XrmToolBox.EntityImageUpdater
{
    public partial class EntityImageUpdater : PluginControlBase, IMessageBusHost, IGitHubPlugin, IHelpPlugin
    {
        private List<EntityMetadata> entitiesCache;
        private ListViewItem[] listViewItemsCache;
        private ConcurrentStack<Tuple<Guid, string, byte[]>> imageCache;
        private string selectedFolder;
        readonly List<string> imageTypes = new List<string> { "png", "jpg", "jpeg" };

        public string RepositoryName => "Ryr.XrmToolBox.EntityImageUpdater";

        public string UserName => "rajyraman";

        public string HelpUrl => "https://dreamingincrm.com/2015/09/27/xrmtoolbox-tool-entity-image-updater/";

        public event EventHandler<MessageBusEventArgs> OnOutgoingMessage;

        public EntityImageUpdater()
        {
            imageCache = new ConcurrentStack<Tuple<Guid, string, byte[]>>();
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
            WorkAsync(new WorkAsyncInfo("Loading entities...", w =>
            {
                w.Result = MetadataHelper.RetrieveEntities(Service);
            })
            { PostWorkCallBack = c =>
            {
                Cursor = Cursors.Default;
                if (c.Error != null)
                {
                    string errorMessage = CrmExceptionHelper.GetErrorMessage(c.Error, true);
                    CommonDelegates.DisplayMessageBox(ParentForm, errorMessage, "Error", MessageBoxButtons.OK,
                                                      MessageBoxIcon.Error);
                }
                else
                {
                    entitiesCache = (List<EntityMetadata>)c.Result;
                    lvEntities.Items.Clear();
                    var list = new List<ListViewItem>();
                    foreach (EntityMetadata emd in (List<EntityMetadata>)c.Result)
                    {
                        var item = new ListViewItem { Text = emd.DisplayName.UserLocalizedLabel.Label, Tag = emd.LogicalName };
                        item.SubItems.Add(emd.LogicalName);
                        item.SubItems.Add(emd.PrimaryNameAttribute);
                        list.Add(item);
                    }

                    this.listViewItemsCache = list.ToArray();
                    lvEntities.Items.AddRange(listViewItemsCache);

                    gbEntities.Enabled = true;
                }
            }});
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

        private string RetrievePrimaryAttributeForEntity(string entityLogicalName)
        {
            var entityMetadata = MetadataHelper.RetrieveEntity(entityLogicalName, Service);
            return entityMetadata.PrimaryNameAttribute;
        }

        private void RetrieveTextAttributeByType(string entityLogicalName, SourceDataType sourceDataType)
        {
            Cursor = Cursors.WaitCursor;
            WorkAsync(new WorkAsyncInfo($"Loading attributes valid for {sourceDataType}...", dwea =>
            {
                var entityMetadata = MetadataHelper.RetrieveEntity(entityLogicalName, Service);

                var stringAttributes = entityMetadata.Attributes
                .Where(x => x.AttributeType.Value == AttributeTypeCode.String);

                switch (sourceDataType)
                {
                    case SourceDataType.Url:
                        stringAttributes = stringAttributes.Where(x => ((StringAttributeMetadata)x).Format.Value == Microsoft.Xrm.Sdk.Metadata.StringFormat.Url);
                        break;
                    case SourceDataType.Email:
                        stringAttributes = stringAttributes.Where(x => ((StringAttributeMetadata)x).Format.Value == Microsoft.Xrm.Sdk.Metadata.StringFormat.Email);
                        break;
                }
                dwea.Result = stringAttributes;
            })
            { PostWorkCallBack = c =>
            {
                Cursor = Cursors.Default;
                if (c.Error != null)
                {
                    string errorMessage = CrmExceptionHelper.GetErrorMessage(c.Error, true);
                    CommonDelegates.DisplayMessageBox(ParentForm, errorMessage, "Error", MessageBoxButtons.OK,
                                                      MessageBoxIcon.Error);
                }
                else
                {
                    var attributeMetadata = (IEnumerable<AttributeMetadata>)c.Result;
                    foreach (var attribute in attributeMetadata)
                    {
                        if (attribute.IsLogical.GetValueOrDefault()) continue;
                        var item = new ListViewItem { Text = attribute.DisplayName.UserLocalizedLabel?.Label, Tag = attribute.LogicalName };
                        item.SubItems.Add(attribute.SchemaName);
                        item.SubItems.Add(attribute.Description?.UserLocalizedLabel?.Label);
                        item.SubItems.Add(entityLogicalName);
                        ListViewDelegates.AddItem(lvAttributes, item);
                    }
                }
            }});
        }

        private void lvAttributes_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            lvAttributes.Sorting = lvAttributes.Sorting == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
            lvAttributes.ListViewItemSorter = new ListViewItemComparer(e.Column, lvAttributes.Sorting);
        }

        private void tsbUpdateImages_Click(object sender, EventArgs e)
        {
            lvResults.Items.Clear();
            UpdateImages();
        }

        private void UpdateImages(QueryExpression query = null)
        {
            var selectedAttribute = ListViewDelegates.GetSelectedItems(lvAttributes)[0];
            var attributeSchemaName = selectedAttribute.Tag.ToString();
            var entityName = selectedAttribute.SubItems[3].Text;
            Cursor = Cursors.WaitCursor;
            WorkAsync(new WorkAsyncInfo(string.Format("Retrieving records and {0} uploading logo...", query != null ? "selectively" : ""), 
            (bw, e) =>
            {
                var entityRecords = new List<Entity>();
                if (query != null)
                {
                    entityRecords = query.PageInfo == null ? RetrieveAllPages(bw, query, attributeSchemaName) : ExecuteQuery(bw, query);
                }
                else
                {
                    entityRecords = RetrieveAllPages(bw, entityName, attributeSchemaName);
                }
                e.Result = UpdateImages(bw, entityRecords, attributeSchemaName, entityName);
            })
            {
                PostWorkCallBack = c =>
                    {
                        Cursor = Cursors.Default;
                        tsbUpdateImages.Enabled = false;
                        tsbFxbEdit.Enabled = false;
                        if (c.Error != null)
                        {
                            string errorMessage = CrmExceptionHelper.GetErrorMessage(c.Error, true);
                            CommonDelegates.DisplayMessageBox(ParentForm, errorMessage, "Error", MessageBoxButtons.OK,
                                                                MessageBoxIcon.Error);
                        }
                    },
                ProgressChanged = c => SetWorkingMessage(c.UserState.ToString())
            });
        }

        private List<Tuple<Guid, string, byte[]>> UpdateImages(BackgroundWorker bw, List<Entity> entityRecords, string attribute, string entityName)
        {
            var timer = Stopwatch.StartNew();

            imageCache.Clear();
            int rowNumber = 1, totalRecords = entityRecords.Count;

            var imageList = new ImageList();
            imageList.ColorDepth = ColorDepth.Depth32Bit;
            imageList.ImageSize = new Size(40, 40);
            lvResults.SmallImageList = imageList;

            var imageIndex = 0;
            var requests = 0;

            ExecuteMultipleRequest executeMultipleRequests = new ExecuteMultipleRequest();
            executeMultipleRequests.Settings = new ExecuteMultipleSettings { ContinueOnError = true, ReturnResponses = true };
            executeMultipleRequests.Requests = new OrganizationRequestCollection();

            //https://msdn.microsoft.com/en-us/library/hh195051%28v=vs.110%29.aspx
            var imageTaskList = new List<KeyValuePair<Tuple<Entity, string>, Task<byte[]>>>();
            foreach (var entity in entityRecords)
            {
                var attributeValue = entity.GetAttributeValue<string>(attribute);
                if (string.IsNullOrEmpty(attributeValue) || 
                    imageTaskList.Any(x => x.Key.Item2 == attributeValue)) continue;

                var logoTask = Task.Run(() =>
                {
                    bw.ReportProgress(0, $"Record {rowNumber++} of {totalRecords} : Retrieving image for {attributeValue}");
                    return GetLogo(entity, attributeValue);
                });
                imageTaskList.Add(new KeyValuePair<Tuple<Entity, string>, Task<byte[]>>(
                    new Tuple<Entity, string>(entity, attributeValue), logoTask));
            }
            var imageTasks = imageTaskList.Select(x => x.Value).ToList();
            Task.WaitAll(imageTasks.ToArray());

            var updatedImages = imageCache.ToList();
            foreach (var entity in entityRecords)
            {
                var updateEntity = new Entity { Id = entity.Id, LogicalName = entity.LogicalName };
                var attributeValue = entity.GetAttributeValue<string>(attribute);
                var entityImage = updatedImages.SingleOrDefault(x=>x.Item2 == attributeValue)?.Item3;
                if (entityImage == null) continue;

                updateEntity["entityimage"] = entityImage;
                executeMultipleRequests.Requests.Add(new UpdateRequest { Target = updateEntity });
                bw.ReportProgress(0, $"Preparing update request for {attribute}");
                requests++;

                if (requests == 100 || entityRecords.Count == requests)
                {
                    bw.ReportProgress(0, $"Executing update requests for {attribute}");
                    ExecuteMultipleUpdates(updatedImages, executeMultipleRequests);
                    executeMultipleRequests.Requests.Clear();
                    requests = 0;
                }
            }

            //Process any leftover requests
            if (executeMultipleRequests.Requests.Any())
            {
                bw.ReportProgress(0, "Pushing through final set of update requests");
                ExecuteMultipleUpdates(updatedImages, executeMultipleRequests);
            }

            var imageConverter = new ImageConverter();
            var displayColumn = ListViewDelegates.GetSelectedItems(lvEntities)[0].SubItems[2].Text;
            foreach (var entity in entityRecords)
            {
                var attributeValue = entity.GetAttributeValue<string>(attribute);
                var primaryDisplayName = entity.GetAttributeValue<string>(displayColumn);
                var entityImage = updatedImages.SingleOrDefault(x => x.Item2 == attributeValue)?.Item3;
                if (entityImage == null) continue;

                var imageListImage = imageConverter.ConvertFrom(entityImage) as Bitmap;
                imageList.Images.Add(imageListImage);
                var listViewItem = new ListViewItem($"{primaryDisplayName} ({attributeValue})");
                listViewItem.Tag = $"{ConnectionDetail.WebApplicationUrl}main.aspx?etn={entityName}&pagetype=entityrecord&id={entity.Id}";
                listViewItem.ImageIndex = imageIndex++;
                lvResults.Items.Add(listViewItem);
            }
            timer.Stop();
            MessageBox.Show($"{lvResults.Items.Count} images updated in {timer.ElapsedMilliseconds / 1000} second(s)", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return updatedImages;
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

        private async Task<byte[]> GetLogo(Entity entity, string attributeValue)
        {
            var selectedLogoSource = gbLogoSource.Controls.OfType<RadioButton>().FirstOrDefault(x => x.Checked)?.Tag.ToString();
            var logoSource = (LogoSource)Enum.Parse(typeof(LogoSource), selectedLogoSource);
            if (imageCache.Any(x => x.Item2 == attributeValue))
            {
                return imageCache.FirstOrDefault(x => x.Item2 == attributeValue)?.Item3;
            }

            var image = await RetrieveLogoFromSelectedApi(logoSource, attributeValue);
            if (image != null)
            {
                imageCache.Push(new Tuple<Guid, string, byte[]>(entity.Id, attributeValue, image));
            }
            return image;
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
                        relativeAddress = $"{new Uri(attributeValue).Host}?size=144";
                    break;
                case LogoSource.Gravatar:
                    baseAddress = "http://www.gravatar.com/";
                    relativeAddress = $"/avatar/{CalculateMD5Hash(attributeValue)}?s=144&&d=404";
                    break;
                case LogoSource.Twitter:
                    baseAddress = "https://twitter.com/";
                    relativeAddress = $"/{attributeValue}/profile_image?size=normal";
                    break;
            }
            if ((string.IsNullOrEmpty(baseAddress) || string.IsNullOrEmpty(relativeAddress)) 
                && logoSource == LogoSource.Folder && !string.IsNullOrEmpty(selectedFolder))
            {
                foreach (var imageType in imageTypes)
                {
                    var imageFullPath = $"{Path.Combine(selectedFolder, attributeValue)}.{imageType}";
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

        private List<Entity> RetrieveAllPages(BackgroundWorker bw, QueryExpression query)
        {
            EntityCollection results = null;
            List<Entity> allPagesAllRows = new List<Entity>();
            var displayColumn = ListViewDelegates.GetSelectedItems(lvEntities)[0].SubItems[2].Text;
            if(!query.ColumnSet.Columns.Contains(displayColumn)) query.ColumnSet.AddColumn(displayColumn);
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
                bw.ReportProgress(0, $"{query.EntityName}: Retrieving Page {query.PageInfo.PageNumber}..");
                results = Service.RetrieveMultiple(query);
                allPagesAllRows.AddRange(results.Entities.ToList());
                query.PageInfo.PageNumber++;
                query.PageInfo.PagingCookie = results.PagingCookie;
            } while (results.MoreRecords);

            return allPagesAllRows;
        }

        private List<Entity> RetrieveAllPages(BackgroundWorker bw, QueryExpression query, string attribute)
        {
            query.ColumnSet = new ColumnSet(attribute);
            return RetrieveAllPages(bw, query);
        }

        private List<Entity> RetrieveAllPages(BackgroundWorker bw, string entityName, string attribute)
        {
            var query = new QueryExpression(entityName);
            query.ColumnSet = new ColumnSet(attribute);
            //TODO: Don't assume statecode always exists. Probably use metadata to check if statecode exists
            query.Criteria.AddCondition("statecode", ConditionOperator.Equal, 0);
            return RetrieveAllPages(bw, query);
        }
        private List<Entity> ExecuteQuery(BackgroundWorker bw, QueryExpression query)
        {
            bw.ReportProgress(0, $"{query.EntityName}: Retrieving Page {query.PageInfo.PageNumber}, {query.PageInfo.Count} records..");
            return Service.RetrieveMultiple(query).Entities.ToList();
        }
        private void lvAttributes_SelectedIndexChanged(object sender, EventArgs e)
        {
            imageCache.Clear();
            lvResults.Items.Clear();
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
                var query = queryExpressionResponse.Query;
                var displayColumn = ListViewDelegates.GetSelectedItems(lvEntities)[0].SubItems[2].Text;
                if (!query.ColumnSet.Columns.Contains(displayColumn)) query.ColumnSet.AddColumn(displayColumn);
                UpdateImages(query);
            }
        }

        private void lvResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            var imageListView = ((ListView)sender).SelectedItems;
            if (imageListView.Count == 0) return;

            var clickedImage = imageListView[0];
            if (clickedImage.Tag != null)
            {
                Process.Start(clickedImage.Tag.ToString());
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
