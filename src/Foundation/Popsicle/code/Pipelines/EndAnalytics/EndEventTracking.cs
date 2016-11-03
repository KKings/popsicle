namespace KKings.Foundation.Popsicle.Pipelines.EndAnalytics
{
    using Analytics;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.DependencyInjection;
    using Sitecore.Pipelines;

    public class EndEventTracking
    {
        public void Process(PipelineArgs args)
        {
            var eventTracker = ServiceLocator.ServiceProvider.GetService<IEventTracker>();
 
            eventTracker.EndTracking();
        }
    }
}