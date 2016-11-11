// -------------------------------------------------------------------------------------
//  <author>Hishaam Namooya</author>
// -------------------------------------------------------------------------------------

namespace Sitecore.Publishing.Progression.Common
{
    using System.Collections.Generic;
    using Sitecore.Configuration;
    using Sitecore.Publishing.Progression.Entities;
    using Sitecore.Web.UI.HtmlControls;
    using Sitecore.Web.UI.WebControls;

    public class Utilities
    {
        public static Dictionary<string, string> ItemDisplayCount = new Dictionary<string, string>();

        public static GridPanel SetPublishInfo(GridPanel publishFields, PublishingInfoArgsEntity summary, bool completed)
        {
            var owner = new Literal
            {
                Text = "Owner: " + summary.Owner
            };

            publishFields.Controls.Add(owner);

            var mode = new Literal
            {
                Text = "Publish Mode: " + summary.PublishMode
            };

            publishFields.Controls.Add(mode);

            var processedCount = new Literal
            {
                Text = "Total Item Processed: " + summary.PublishedCount
            };

            publishFields.Controls.Add(processedCount);

            var totalItems = new Literal
            {
                Text = "Total Items to be processed: " + summary.PublishQueue
            };

            publishFields.Controls.Add(totalItems);

            if (completed && summary.PublishedItems != null)
            {
                var itemCreated = new Literal
                {
                    Text = "Item Created: " + summary.ItemCreated
                };

                publishFields.Controls.Add(itemCreated);

                var itemSkipped = new Literal
                {
                    Text = "Item Skipped: " + summary.ItemSkipped
                };

                publishFields.Controls.Add(itemSkipped);

                var itemDeleted = new Literal
                {
                    Text = "Item Deleted: " + summary.ItemDeleted
                };

                publishFields.Controls.Add(itemDeleted);

                var itemUpdated = new Literal
                {
                    Text = "Item Updated: " + (summary.ItemUpdated + 1)
                };

                publishFields.Controls.Add(itemUpdated);
            }

            return publishFields;
        }

        public static GridPanel InitializePublishInfo(GridPanel publishFields)
        {
            publishFields.Controls.Clear();

            var owner = new Literal
            {
                Text = "Owner: "
            };

            publishFields.Controls.Add(owner);

            var mode = new Literal
            {
                Text = "Publish Mode: "
            };

            publishFields.Controls.Add(mode);

            var processedCount = new Literal
            {
                Text = "Total Item Processed: "
            };

            publishFields.Controls.Add(processedCount);

            var totalItems = new Literal
            {
                Text = "Total Items to be published: "
            };

            publishFields.Controls.Add(totalItems);

            return publishFields;
        }

        public static void InitializeItemDisplayCount()
        {
            if (ItemDisplayCount.Count != 0)
            {
                var publishingSetting = Factory.GetDatabase("master").Items.GetItem(Constants.PublishSettings).Fields[Constants.ItemCountDisplay].Value;

                ItemDisplayCount.Add("ItemCountSetting", publishingSetting);
            }
        } 

        public static Listview SetPublishingIListview(Listview publishListview, List<PublishedItemEntity> publishedItemsInfo)
        {
            if (publishedItemsInfo != null)
            {
                foreach (var publishedItem in publishedItemsInfo)
                {
                    var listSubItem = new ListviewItem();

                    publishListview.Controls.Add(listSubItem);

                    listSubItem.ID = Control.GetUniqueID("I");

                    listSubItem.ColumnValues["itemName"] = publishedItem.ItemName;
                    listSubItem.ColumnValues["itemPath"] = publishedItem.ItemPath;
                }
            }

            return publishListview;
        }
    }
}
