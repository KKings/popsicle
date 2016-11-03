namespace KKings.Foundation.Popsicle.Analytics
{
    using System.Collections.Generic;
    using Events;

    public interface IEventTracker
    {
        /// <summary>
        /// All currently tracked events
        /// </summary>
        IEnumerable<ITrackingEvent> AllEvents { get; }

        /// <summary>
        /// Is the Tracker Active
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Adds a tracking event
        /// </summary>
        /// <param name="trackingEvent">The Tracking Event</param>
        void Track(ITrackingEvent trackingEvent);

        /// <summary>
        /// Clear all entries that are being tracked
        /// </summary>
        void Clear();

        /// <summary>
        /// Start the tracking session
        /// </summary>
        void StartTracking();

        /// <summary>
        /// Ends the tracking session
        /// </summary>
        void EndTracking();
    }
}