namespace KKings.Foundation.Popsicle.Pipelines.RegisterPageEvent
{
    using Analytics;
    using BuildTrackingEvent;
    using FilterPageEvents;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.Abstractions;
    using Sitecore.Analytics;
    using Sitecore.Analytics.Pipelines.RegisterPageEvent;
    using Sitecore.DependencyInjection;
    using Sitecore.Pipelines;

    public class RecordEvent : RegisterPageEventProcessor
    {
        /// <summary>
        /// BaseLog Implementation for Logging
        /// </summary>
        public virtual BaseLog Logger { get; } = ServiceLocator.ServiceProvider.GetService<BaseLog>();

        public override void Process(RegisterPageEventArgs args)
        {
            var eventTracker = ServiceLocator.ServiceProvider.GetService<IEventTracker>();

            if (!eventTracker.IsActive)
            {
                return;
            }

            // Runs pipeline to filter out PageEvents that should not be sent
            var filterArgs = new FilterPageEventsArgs(args.PageEvent);
            CorePipeline.Run("ma.filterPageEvents", filterArgs);

            if (filterArgs.IsFiltered)
            {
                this.Logger.Debug($"{args.PageEvent?.Name} is being filtered from the tracker.", this);
                return;
            }

            var pageEvent = args.PageEvent;

            var buildArgs = new BuildTrackingEventArgs(pageEvent, Tracker.Current.Contact);
            CorePipeline.Run("ma.buildTrackingEvent", buildArgs);

            eventTracker.Track(buildArgs.TrackingEvent);
        }
    }
}