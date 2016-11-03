namespace KKings.Foundation.Popsicle.Pipelines.MABuildTrackingEvent
{
    using System;
    using Events;
    using Sitecore.Analytics.Model;
    using Sitecore.Pipelines;

    public class BuildTrackingEventArgs : PipelineArgs
    {
        /// <summary>
        /// Gets the Page Event to Build
        /// </summary>
        public PageEventData PageEvent { get; private set; }

        /// <summary>
        /// Gets or Sets the Tracking Event
        /// </summary>
        public ITrackingEvent TrackingEvent { get; set; }

        public BuildTrackingEventArgs(PageEventData pageEvent)
        {
            if (pageEvent == null)
            {
                throw new ArgumentNullException($"{nameof(pageEvent)}");
            }

            this.PageEvent = pageEvent;
        }
    }
}