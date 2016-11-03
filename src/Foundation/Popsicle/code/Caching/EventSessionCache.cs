namespace KKings.Foundation.Popsicle.Caching
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Events;

    public sealed class EventSessionCache : IEventCache
    {
        /// <summary>
        /// Base Implementation of the HttpContext
        /// </summary>
        private readonly HttpContextBase httpContextBase;

        /// <summary>
        /// Items Key within the HttpContext
        /// </summary>
        public string ItemsKey { get; } = "event_session_cache";

        public EventSessionCache(HttpContextBase httpContextBase)
        {
            if (httpContextBase?.Session == null)
            {
                throw new ArgumentNullException(nameof(httpContextBase));
            }

            /**
             * If a previous request has already created a cache session, do not override it
             */
            if (httpContextBase.Session != null && httpContextBase.Session[this.ItemsKey] == null)
            {
                httpContextBase.Session.Add(this.ItemsKey, Enumerable.Empty<ITrackingEvent>());
            }

            this.httpContextBase = httpContextBase;
        }

        /// <summary>
        /// Returns all Tracked Events for the session
        /// </summary>
        /// <returns>All currently tracked events for the session</returns>
        public IEnumerable<ITrackingEvent> AllEntries()
        {
            return this.httpContextBase?.Session?[this.ItemsKey] as IEnumerable<ITrackingEvent> 
                ?? Enumerable.Empty<ITrackingEvent>();
        }

        /// <summary>
        /// Adds a <see cref="ITrackingEvent"/> to be tracked
        /// <exception cref="Exception">Throws if Session is not able to be accessed</exception>
        /// </summary>
        /// <param name="value">The Tracking Event</param>
        public void Add(ITrackingEvent value)
        {
            var httpSessionStateBase = this.httpContextBase?.Session;

            if (httpSessionStateBase == null)
            {
                throw new Exception("Unable to access session to add new tracked events.");
            }

            var trackedEvents = this.httpContextBase?.Session?[this.ItemsKey] as IEnumerable<ITrackingEvent> ?? Enumerable.Empty<ITrackingEvent>();

            httpSessionStateBase[this.ItemsKey] = trackedEvents.Concat(new[] { value });
        }

        /// <summary>
        /// Clears all Entries from the session
        /// <exception cref="Exception">Throws if Session is not able to be accessed</exception>
        /// </summary>
        public void Clear()
        {
            var httpSessionStateBase = this.httpContextBase?.Session;

            if (httpSessionStateBase == null)
            {
                throw new Exception("Unable to access session to clear all tracked events.");
            }

            httpSessionStateBase[this.ItemsKey] = Enumerable.Empty<ITrackingEvent>();
        }
    }
}