namespace KKings.Foundation.Popsicle.Pipelines.PostSessionEnd
{
    using Analytics;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.Abstractions;
    using Sitecore.DependencyInjection;
    using Sitecore.Pipelines.EndSession;
    using Sitecore.Pipelines.SessionEnd;

    public class LogTrackedEvents 
    {
        /// <summary>
        /// BaseLog Implementation for Logging
        /// </summary>
        public virtual BaseLog Logger { get; } = ServiceLocator.ServiceProvider.GetService<BaseLog>();

        public void Process(PostSessionEndArgs args)
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