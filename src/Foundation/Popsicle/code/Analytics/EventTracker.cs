namespace KKings.Foundation.Popsicle.Analytics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Caching;
    using Events;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.Abstractions;
    using Sitecore.DependencyInjection;

    public class EventTracker : IEventTracker
    {
        /// <summary>
        /// Base Implementation for Logging
        /// </summary>
        private readonly BaseLog logger;

        /// <summary>
        /// Is the tracker active for the request
        /// <para>If the tracker is not active, the tracker will be unable to track new events</para>
        /// </summary>
        public virtual bool IsActive { get; private set; }

        /// <summary>
        /// Event Cache for storing tracked events between requests
        /// </summary>
        public virtual IEventCache EventCache { get; set; }

        /// <summary>
        /// All currently Tracked Events
        /// </summary>
        public virtual IEnumerable<ITrackingEvent> AllEvents
        {
            get { return this.EventCache?.AllEntries() ?? Enumerable.Empty<ITrackingEvent>(); }
        }

        /// <summary>
        /// HttpContextBase Implementation for the current request/response
        /// </summary>
        public virtual HttpContextBase HttpContextBase
        {
            get { return ServiceLocator.ServiceProvider.GetService<HttpContextBase>(); }
        }

        public EventTracker(BaseLog logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Tracks a new tracking event
        /// <exception cref="Exception">Throws if the tracker is not currently active. </exception>
        /// </summary>
        /// <param name="trackingEvent">The tracking event</param>
        public virtual void Track(ITrackingEvent trackingEvent)
        {
            if (!this.IsActive)
            {
                throw new Exception("EventTracker has not been initialized.");
            }

            if (trackingEvent == null)
            {
                this.logger.Error("Attempted to track a null event.", this);

                return;
            }

            this.EventCache?.Add(trackingEvent);
        }

        /// <summary>
        /// Starts the Tracking Session
        /// </summary>
        public void StartTracking()
        {
            this.IsActive = true;

            this.EventCache = new EventSessionCache(this.HttpContextBase);
        }

        /// <summary>
        /// Ends the tracking session
        /// </summary>
        public virtual void EndTracking()
        {
            this.IsActive = false;
        }

        /// <summary>
        /// Clears all entries in the EventCache
        /// </summary>
        public virtual void Clear()
        {
            this.EventCache.Clear();
        }
    }
}