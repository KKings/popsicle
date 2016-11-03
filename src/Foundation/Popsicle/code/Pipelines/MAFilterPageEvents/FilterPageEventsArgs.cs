namespace KKings.Foundation.Popsicle.Pipelines.MAFilterPageEvents
{
    using Sitecore.Analytics.Model;
    using Sitecore.Pipelines;

    public class FilterPageEventsArgs : PipelineArgs
    {
        /// <summary>
        /// Gets or sets if the Page Event should be filtered
        /// </summary>
        public bool IsFiltered { get; set; } = false;

        /// <summary>
        /// Gets the Page Event
        /// </summary>
        public PageEventData PageEvent { get; private set; }

        public FilterPageEventsArgs(PageEventData pageEvent)
        {
            this.PageEvent = pageEvent;
        }
    }
}