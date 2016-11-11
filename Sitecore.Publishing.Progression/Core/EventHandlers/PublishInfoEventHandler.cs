// -------------------------------------------------------------------------------------
//  <author>Hishaam Namooya</author>
// -------------------------------------------------------------------------------------

namespace Sitecore.Publishing.Progression.Core.EventHandlers
{
    using System;
    using Sitecore.Publishing.Progression.Entities;

    public class PublishInfoEventHandler
    {
        public void Handle(object sender, EventArgs args)
        {
            var publishInfoArgs = Events.Event.ExtractParameter(args, 0) as PublishingInfoArgsEntity;

            if (publishInfoArgs != null)
            {
                publishInfoArgs.Owner = PublishInfoBridgeEntity.OwnerBridge;
                publishInfoArgs.PublishMode = PublishInfoBridgeEntity.PublishModeBridge;
                publishInfoArgs.PublishQueue = PublishInfoBridgeEntity.PublishQueueBridge;
                publishInfoArgs.PublishedCount = PublishInfoBridgeEntity.PublishedCountBridge;

                publishInfoArgs.ItemCreated = PublishInfoBridgeEntity.ItemCreatedBridge;
                publishInfoArgs.ItemDeleted = PublishInfoBridgeEntity.ItemDeletedBridge;
                publishInfoArgs.ItemSkipped = PublishInfoBridgeEntity.ItemSkippedBridge;
                publishInfoArgs.ItemUpdated = PublishInfoBridgeEntity.ItemUpdatedBridge;

                publishInfoArgs.PublishedItems = PublishInfoBridgeEntity.PublishedItems;
            }

        }
    }
}
