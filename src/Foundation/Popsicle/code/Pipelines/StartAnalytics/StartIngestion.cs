namespace KKings.Foundation.Popsicle.Pipelines.StartAnalytics
{
    using System.Web;
    using Ingest;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.DependencyInjection;
    using Sitecore.Diagnostics;
    using Sitecore.Pipelines;

    public class StartIngestion
    {
        public virtual void Process(PipelineArgs args)
        {
            if (!Sitecore.Analytics.Tracker.Current.Session.Settings.IsFirstRequest)
            {
                return;
            }

            var httpContextBase = ServiceLocator.ServiceProvider.GetService<HttpContextBase>();

            var ingestionArgs = new IngestionPipelineArgs(Sitecore.Analytics.Tracker.Current.Contact, httpContextBase);

            CorePipeline.Run("ma.ingestion", ingestionArgs);
        }
    }
}