// -------------------------------------------------------------------------------------
//  <author>Hishaam Namooya</author>
// -------------------------------------------------------------------------------------

namespace Sitecore.Publishing.Progression.Entities
{
    using System.Collections.Generic;

    public class PublishInfoBridgeEntity
    {
        public static double PublishedCountBridge { get; set; }

        public static double PublishQueueBridge { get; set; }

        public static string OwnerBridge { get; set; }

        public static string PublishModeBridge { get; set; }

        public static int ItemCreatedBridge { get; set; }

        public static int ItemUpdatedBridge { get; set; }

        public static int ItemSkippedBridge { get; set; }

        public static int ItemDeletedBridge { get; set; }

        public static List<PublishedItemEntity> PublishedItems { get; set; }

    }
}