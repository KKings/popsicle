namespace KKings.Foundation.Popsicle.Pipelines.PostSessionEnd
{
    using System.Linq;
    using Analytics;
    using Microsoft.Extensions.DependencyInjection;
    using Pipelines.FlushTrackingEvents;
    using Sitecore.DependencyInjection;
    using Sitecore.Pipelines;
    using Sitecore.Pipelines.EndSession;

    public class FlushTrackingEvents
    {
        public void Process(PostSessionEndArgs args)
        {
            var eventTracker = ServiceLocator.ServiceProvider.GetService<IEventTracker>();

            if (eventTracker.AllEvents.Any())
            {
                var flushEvents = new FlushTrackingEventArgs(eventTracker.AllEvents);

                CorePipeline.Run("ma.flushTrackingEvents", flushEvents);

                eventTracker.Clear();
                eventTracker.EndTracking();
            }
        }
    }
}