namespace KKings.Foundation.Popsicle.Outcome
{
    using Analytics;
    using Microsoft.Extensions.DependencyInjection;
    using Pipelines.MABuildTrackingOutcome;
    using Pipelines.MAFilterTrackingOutcomes;
    using Sitecore.Analytics.Data.DataAccess.MongoDb;
    using Sitecore.Analytics.Outcome.Data;
    using Sitecore.Analytics.Outcome.Model;
    using Sitecore.DependencyInjection;
    using Sitecore.Pipelines;

    /// <summary>
    /// Overriding the Outcome Repository to intercept the save events 
    /// of an Outcome before being flushed to MongoDb
    /// </summary>
    public class OutcomeRepository : MongoDbOutcomeRepository
    {
        public OutcomeRepository(MongoDbCollection mongoCollection) : base(mongoCollection) { }

        public OutcomeRepository(string connectionStringName) : base(connectionStringName) { }

        public OutcomeRepository(string connectionStringName, string collectionName) : base(connectionStringName, collectionName) { }
        
        /// <summary>
        /// Saves the Outcome to the Database
        /// <para>Runs two pipelines to filter and create a Tracking Event</para>
        /// </summary>
        /// <param name="outcome">The outcome</param>
        public override void Save(IOutcome outcome)
        {
            var eventTracker = ServiceLocator.ServiceProvider.GetService<IEventTracker>();

            if (!eventTracker.IsActive)
            {
                base.Save(outcome);
                return;
            }

            // Runs pipeline to filter out Outcomes that should not be sent
            var filterArgs = new FilterTrackingOutcomesArgs(outcome);
            CorePipeline.Run("ma.filterPageOutcomes", filterArgs, false);

            if (filterArgs.IsFiltered)
            {
                base.Save(outcome);
                return;
            }
            
            var buildArgs = new BuildTrackingOutcomeArgs(outcome);
            CorePipeline.Run("ma.buildTrackingOutcome", buildArgs, false);

            eventTracker.Track(buildArgs.TrackingEvent);

            base.Save(outcome);
        }
    }
}