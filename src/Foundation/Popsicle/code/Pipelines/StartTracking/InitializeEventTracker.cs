
namespace KKings.Foundation.Popsicle.Pipelines.StartTracking
{
    using Analytics;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.Analytics.Pipelines.StartTracking;
    using Sitecore.DependencyInjection;
    using Sitecore.Xdb.Configuration;

    public class InitializeEventTracker : StartTrackingProcessor
    {
        public override void Process(StartTrackingArgs args)
        {
            if (XdbSettings.Tracking.Enabled)
            {
                var eventTracker = ServiceLocator.ServiceProvider.GetService<IEventTracker>();
                eventTracker.StartTracking();
            }
        }
    }
}