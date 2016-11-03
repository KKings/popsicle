namespace KKings.Foundation.Popsicle.Caching
{
    using System.Collections.Generic;
    using Events;

    public interface IEventCache
    {
        /// <summary>
        /// Get All Cache Entries
        /// </summary>
        /// <returns>List of Entries</returns>
        IEnumerable<ITrackingEvent> AllEntries();

        /// <summary>
        /// Adds a cache entry to the cache
        /// </summary>
        /// <param name="value">cache entry</param>
        void Add(ITrackingEvent value);

        /// <summary>
        /// Clear all entries in cache
        /// </summary>
        void Clear();
    }
}