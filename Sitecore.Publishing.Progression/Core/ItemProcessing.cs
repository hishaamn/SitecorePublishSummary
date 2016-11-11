// -------------------------------------------------------------------------------------
//  <author>Hishaam Namooya</author>
// -------------------------------------------------------------------------------------

namespace Sitecore.Publishing.Progression.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Sitecore.Configuration;
    using Sitecore.Data;
    using Sitecore.Publishing;
    using Sitecore.Publishing.Pipelines.PublishItem;
    using Sitecore.Publishing.Progression.Common;
    using Sitecore.Publishing.Progression.Entities;

    public class ItemProcessing
    {
        private int ItemDisplayValue { get; set; }

        public void OnItemProcessed(object sender, EventArgs args)
        {
            var isProgressActive = Factory.GetDatabase("master").Items.GetItem(Constants.PublishSettings).Fields[Constants.IsActive].Value;

            if (!isProgressActive.Equals("1"))
            {
                return;
            }

            if (Utilities.ItemDisplayCount.ContainsKey("ItemCountSetting"))
            {
                this.ItemDisplayValue = Int32.Parse(Utilities.ItemDisplayCount["ItemCountSetting"]);
            }
            else
            {
                var publishingSetting = Factory.GetDatabase("master").Items.GetItem(Constants.PublishSettings).Fields[Constants.ItemCountDisplay].Value;

                Utilities.ItemDisplayCount.Add("ItemCountSetting", publishingSetting);

                this.ItemDisplayValue = Int32.Parse(publishingSetting);
            }

            var itemArgs = args as ItemProcessedEventArgs;
            
            if (itemArgs != null)
            {
                var publishContext = itemArgs.Context.PublishContext;

                if (itemArgs.Context.PublishOptions.Mode == PublishMode.Incremental)
                {
                    var processedItems = publishContext.ProcessedItems;

                    var itemsToProcess = publishContext.Queue[2].Count();

                    var totalItems = Math.Pow(itemArgs.Context.PublishContext.Languages.Count(), 2) +
                                     (itemArgs.Context.PublishContext.Languages.Count() * itemsToProcess);

                    PublishInfoBridgeEntity.PublishQueueBridge = totalItems;
                    PublishInfoBridgeEntity.PublishedCountBridge = processedItems.Count;
                    PublishInfoBridgeEntity.OwnerBridge = itemArgs.Context.User.LocalName;
                    PublishInfoBridgeEntity.PublishModeBridge = itemArgs.Context.PublishOptions.Mode.ToString();
                    PublishInfoBridgeEntity.ItemCreatedBridge = publishContext.Statistics.Created;
                    PublishInfoBridgeEntity.ItemUpdatedBridge = publishContext.Result.Statistics.Updated;
                    PublishInfoBridgeEntity.ItemSkippedBridge = publishContext.Statistics.Skipped;
                    PublishInfoBridgeEntity.ItemDeletedBridge = publishContext.Statistics.Deleted;

                    PublishInfoBridgeEntity.PublishedItems = new List<PublishedItemEntity>();

                    var itemToDisplayList = processedItems.Skip(processedItems.Count - this.ItemDisplayValue);

                    foreach (var processedItem in itemToDisplayList)
                    {
                        var item = Factory.GetDatabase("master").Items.GetItem(new ID(processedItem));

                        if (item != null)
                        {
                            var publishedItems = new PublishedItemEntity
                            {
                                ItemName = item.DisplayName,
                                ItemPath = item.Paths.ContentPath
                            };

                            PublishInfoBridgeEntity.PublishedItems.Add(publishedItems);
                        }
                    }
                }
            }
        }
    }
}