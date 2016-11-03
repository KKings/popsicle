namespace KKings.Foundation.Popsicle.Pipelines.VisitEnd
{
    using Analytics;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.Abstractions;
    using Sitecore.Analytics.Pipelines.VisitEnd;
    using Sitecore.DependencyInjection;

    public class LogTrackedEvents : VisitEndProcessor
    {
        /// <summary>
        /// Base Implementation for Logging
        /// </summary>
        public virtual BaseLog Logger { get; } = ServiceLocator.ServiceProvider.GetService<BaseLog>();

        public override void Process(VisitEndArgs args)
        {
            var eventTracker = ServiceLocator.ServiceProvider.GetService<IEventTracker>();

            if (eventTracker != null)
            {
                foreach (var @event in eventTracker.AllEvents)
                {
                    this.Logger.Info($"Tracked Event: {@event.DateTime} - {@event.Text} - {@event.DefinitionId}", this);
                }
            }
        }
    }
}