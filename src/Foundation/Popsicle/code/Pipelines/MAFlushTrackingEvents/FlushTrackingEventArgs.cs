namespace KKings.Foundation.Popsicle.Pipelines.MAFlushTrackingEvents
{
    using System;
    using System.Collections.Generic;
    using Events;
    using Sitecore.Pipelines;

    public class FlushTrackingEventArgs : PipelineArgs
    {
        /// <summary>
        /// Gets the Tracking Events
        /// </summary>
        public IEnumerable<ITrackingEvent> TrackingEvents { get; private set; }

        public FlushTrackingEventArgs(IEnumerable<ITrackingEvent> trackingEvents)
        {
            if (trackingEvents == null)
            {
                throw new ArgumentNullException(nameof(trackingEvents));
            }

            this.TrackingEvents = trackingEvents;
        }

    }
}