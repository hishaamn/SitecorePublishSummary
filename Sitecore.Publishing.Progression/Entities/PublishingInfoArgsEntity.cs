// -------------------------------------------------------------------------------------
//  <author>Hishaam Namooya</author>
// -------------------------------------------------------------------------------------

namespace Sitecore.Publishing.Progression.Entities
{
    using System;
    using System.Collections.Generic;

    public class PublishingInfoArgsEntity : EventArgs
    {
        public double PublishedCount { get; set; }

        public double PublishQueue { get; set; }

        public string Owner { get; set; }

        public string PublishMode { get; set; }

        public int ItemCreated { get; set; }

        public int ItemUpdated { get; set; }

        public int ItemSkipped { get; set; }

        public int ItemDeleted { get; set; }

        public List<PublishedItemEntity> PublishedItems { get; set; }
    }
}
