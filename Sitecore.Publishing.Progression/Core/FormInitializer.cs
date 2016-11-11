// -------------------------------------------------------------------------------------
//  <author>Hishaam Namooya</author>
// -------------------------------------------------------------------------------------

namespace Sitecore.Publishing.Progression.Core
{
    using System;
    using System.Linq;
    using Sitecore;
    using Sitecore.Jobs;
    using Sitecore.Publishing.Progression.Common;
    using Sitecore.Publishing.Progression.Entities;
    using Sitecore.Shell.Framework.Commands;
    using Sitecore.Web.UI.HtmlControls;
    using Sitecore.Web.UI.Sheer;
    using Sitecore.Web.UI.WebControls;
    using Sitecore.Web.UI.WebControls.Ribbons;
    using Constants = Sitecore.Publishing.Progression.Constants;
    using Edit = Sitecore.Web.UI.HtmlControls.Edit;

    public class FormInitializer : BaseForm
    {
        protected Border RibbonPanel;
        protected Edit PublishJobName;
        protected GridPanel GridPublishSummary;
        protected GridPanel GridPublishData;
        protected Literal SummaryItemLiteral;
        protected Listview ListPublishedItem;

        protected override void OnLoad(EventArgs args)
        {
            base.OnLoad(args);

            if (!Context.ClientPage.IsEvent)
            {
                InitializeRibbon();
                InitializePublishSummary(this.GridPublishData);
            }
        }

        protected override void OnPreRender(EventArgs args)
        {
            base.OnPreRender(args);

            if (Context.ClientPage.IsEvent)
            {
                return;
            }

            this.CheckStatus();
        }

        protected virtual void InitializeRibbon()
        {
            var mainRibbon = new Ribbon
            {
                ID = "MainRibbon",
                CommandContext = new CommandContext()
            };

            var item = Context.Database.GetItem(Constants.RibbonPath);

            mainRibbon.CommandContext.RibbonSourceUri = item.Uri;

            this.RibbonPanel.Controls.Add(mainRibbon);
        }

        protected static GridPanel InitializePublishSummary(GridPanel GridPublishData)
        {
            return Utilities.InitializePublishInfo(GridPublishData);
        }

        public void Refresh()
        {
            this.CheckStatus();
        }

        private static PublishingInfoArgsEntity GetPublishSummary()
        {
            var args = new PublishingInfoArgsEntity();

            Events.Event.RaiseEvent("custom:publishInfo", args);

            return args;
        }

        protected void CheckStatus()
        {
            var isJobDone = JobManager.GetJobs().FirstOrDefault(j => j.Category.Equals("publish") && j.Status.State == JobState.Running);

            var summary = GetPublishSummary();

            if (summary != null)
            {
                this.GridPublishData.Controls.Clear();

                this.ListPublishedItem.Controls.Clear();

                if (isJobDone != null && !isJobDone.IsDone)
                {
                    this.GridPublishData = Utilities.SetPublishInfo(this.GridPublishData, summary, false);

                    Context.ClientPage.ClientResponse.SetOuterHtml("GridPublishData", this.GridPublishData);

                    var publishListView = Utilities.SetPublishingIListview(this.ListPublishedItem, summary.PublishedItems);

                    Context.ClientPage.ClientResponse.SetOuterHtml("ListPublishedItem", publishListView);

                    SheerResponse.Timer("CheckStatus", 1000);
                }
                else
                {
                    this.GridPublishData = Utilities.SetPublishInfo(this.GridPublishData, summary, true);

                    Context.ClientPage.ClientResponse.SetOuterHtml("GridPublishData", this.GridPublishData);

                    if (Context.ClientPage.IsEvent)
                    {
                        var publishListView = Utilities.SetPublishingIListview(this.ListPublishedItem, summary.PublishedItems);

                        Context.ClientPage.ClientResponse.SetOuterHtml("ListPublishedItem", publishListView);
                    }

                    Utilities.ItemDisplayCount.Clear();

                    Context.ClientPage.ClientResponse.Alert("Published Completed");
                }
            }
        }
    }
}
