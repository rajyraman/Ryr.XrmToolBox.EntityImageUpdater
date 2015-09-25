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

namespace Ryr.XrmToolBox.EntityImageUpdater
{
    public partial class EntityImageUpdater : PluginControlBase
    {
        private List<EntityMetadata> entitiesCache;
        private ListViewItem[] listViewItemsCache;
        private List<Tuple<Guid, string, byte[]>> imageCache;

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

            WorkAsync("Loading entities...",
                e =>
                {
                    e.Result = MetadataHelper.RetrieveEntities(Service);
                },
                e =>
                {
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
            if (lvEntities.SelectedItems.Count > 0)
            {
                string entityLogicalName = lvEntities.SelectedItems[0].Tag.ToString();

                // Reinit other controls
                lvAttributes.Items.Clear();
                lvResults.Items.Clear();

                Cursor = Cursors.WaitCursor;

                WorkAsync("Loading text attributes...",
                    dwea =>
                    {
                        dwea.Result = MetadataHelper.RetrieveEntity(entityLogicalName, Service);
                        var entityMetadata = MetadataHelper.RetrieveEntity(entityLogicalName, Service);
                        dwea.Result = entityMetadata.Attributes.Where(x => x.AttributeType.Value == AttributeTypeCode.String 
                        && ((StringAttributeMetadata)x).Format.Value == Microsoft.Xrm.Sdk.Metadata.StringFormat.Url);
                    },
                    dwea =>
                    {
                        if (dwea.Error != null)
                        {
                            string errorMessage = CrmExceptionHelper.GetErrorMessage(dwea.Error, true);
                            CommonDelegates.DisplayMessageBox(ParentForm, errorMessage, "Error", MessageBoxButtons.OK,
                                                              MessageBoxIcon.Error);
                        }
                        else
                        {
                            Cursor = Cursors.Default;
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
        }

        private void lvAttributes_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            lvAttributes.Sorting = lvAttributes.Sorting == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
            lvAttributes.ListViewItemSorter = new ListViewItemComparer(e.Column, lvAttributes.Sorting);
        }

        private void tsbUpdateImages_Click(object sender, EventArgs e)
        {
            var selectedAttributes = ListViewDelegates.GetSelectedItems(lvAttributes);
            if (selectedAttributes.Any())
            {
                var attributes = selectedAttributes.Select(x => x.Tag.ToString()).ToArray();
                var entityName = selectedAttributes[0].SubItems[3].Text;
                Cursor = Cursors.WaitCursor;

                WorkAsync("Processing entity data...",
                dwea =>
                    {
                        var entityRecords = RetrieveAllPages(Service, entityName, attributes);
                        dwea.Result = UpdateImages(entityRecords, attributes);
                    },
                    dwea =>
                    {
                        if (dwea.Error != null)
                        {
                            string errorMessage = CrmExceptionHelper.GetErrorMessage(dwea.Error, true);
                            CommonDelegates.DisplayMessageBox(ParentForm, errorMessage, "Error", MessageBoxButtons.OK,
                                                              MessageBoxIcon.Error);
                        }
                        else
                        {
                            Cursor = Cursors.Default;
                            tsbUpdateImages.Enabled = false;
                        }
                    });
            }
        }

        private List<Tuple<Guid, string, byte[]>> UpdateImages(List<Entity> entityRecords, string[] attributes)
        {
            imageCache.Clear();
            byte[] image = null;

            var imageList = new ImageList();
            imageList.ImageSize = new Size(144, 144);
            lvResults.LargeImageList = imageList;

            var imageIndex = 0;
            var requests = 0;

            ExecuteMultipleRequest executeMultipleRequests = new ExecuteMultipleRequest();
            executeMultipleRequests.Settings = new ExecuteMultipleSettings { ContinueOnError = true, ReturnResponses = true };
            executeMultipleRequests.Requests = new OrganizationRequestCollection();

            foreach (var entity in entityRecords)
            {
                foreach (var attribute in attributes)
                {
                    var url = entity.GetAttributeValue<string>(attribute);
                    if (!string.IsNullOrEmpty(url) && url.IndexOf("http")>-1)
                    {
                        //Check if we already have retrived the logo for this domain
                        if(!imageCache.Any(x=>x.Item2 == url))
                        {
                            var imageTask = GetLogoForDomain(new Uri(url).Host);
                            imageTask.Wait();
                            image = imageTask.Result;
                            if(image != null) imageCache.Add(new Tuple<Guid, string, byte[]>(entity.Id, url, image));
                        }
                        image = imageCache.FirstOrDefault(x => x.Item2 == url)?.Item3;
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

        private async Task<byte[]> GetLogoForDomain(string domain)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://logo.clearbit.com/");
                client.DefaultRequestHeaders.Accept.Clear();

                HttpResponseMessage response = await client.GetAsync(domain + "?size=144");
                if (response.IsSuccessStatusCode)
                {
                    var r = await response.Content.ReadAsByteArrayAsync();
                    return r;
                }
                return null;
            }
        }
        private List<Entity> RetrieveAllPages(IOrganizationService service, string entityName, string[] attributes)
        {
            EntityCollection results = null;
            List<Entity> allPagesAllRows = new List<Entity>();
            var query = new QueryByAttribute(entityName);
            query.ColumnSet = new ColumnSet(attributes);
            query.AddAttributeValue("statecode", 0);
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

        private void lvAttributes_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedAttributes = ListViewDelegates.GetSelectedItems(lvAttributes);
            if (selectedAttributes.Any()) tsbUpdateImages.Enabled = true;
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
    }
}
