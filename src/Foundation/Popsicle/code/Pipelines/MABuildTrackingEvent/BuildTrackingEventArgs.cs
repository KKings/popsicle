namespace KKings.Foundation.Popsicle.Pipelines.MABuildTrackingEvent
{
    using System;
    using Events;
    using Sitecore.Analytics.Model;
    using Sitecore.Analytics.Tracking;
    using Sitecore.Pipelines;

    public class BuildTrackingEventArgs : PipelineArgs
    {
        /// <summary>
        /// Gets the Page Event to Build
        /// </summary>
        public PageEventData PageEvent { get; private set; }

        /// <summary>
        /// Gets the Contact Record
        /// </summary>
        public Contact Contact { get; private set; }

        /// <summary>
        /// Gets or Sets the Tracking Event
        /// </summary>
        public ITrackingEvent TrackingEvent { get; set; }

        public BuildTrackingEventArgs(PageEventData pageEvent, Contact contact)
        {
            if (pageEvent == null)
            {
                throw new ArgumentNullException($"{nameof(pageEvent)}");
            }

            if (contact == null)
            {
                throw new ArgumentNullException($"{nameof(contact)}");
            }

            this.PageEvent = pageEvent;
            this.Contact = contact;
        }
    }
}