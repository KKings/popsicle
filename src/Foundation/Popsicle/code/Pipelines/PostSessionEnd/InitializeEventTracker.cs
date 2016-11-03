namespace KKings.Foundation.Popsicle.Pipelines.PostSessionEnd
{
    using Analytics;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.DependencyInjection;
    using Sitecore.Pipelines.EndSession;
    using Sitecore.Pipelines.SessionEnd;
    using Sitecore.Xdb.Configuration;

    public class InitializeEventTracker
    {
        public void Process(PostSessionEndArgs args)
        {
            if (XdbSettings.Tracking.Enabled)
            {
                var eventTracker = ServiceLocator.ServiceProvider.GetService<IEventTracker>();
                eventTracker.StartTracking();
            }
        }
    }
}