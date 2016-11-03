namespace KKings.Foundation.Popsicle.Pipelines.SessionEnd
{
    using Analytics;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.DependencyInjection;
    using Sitecore.Pipelines.SessionEnd;
    using Sitecore.Xdb.Configuration;

    public class InitializeEventTracker
    {
        public void Process(SessionEndArgs args)
        {
            if (XdbSettings.Tracking.Enabled)
            {
                var eventTracker = ServiceLocator.ServiceProvider.GetService<IEventTracker>();
                eventTracker.StartTracking();
            }
        }
    }
}