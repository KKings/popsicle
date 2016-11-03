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

    public class OutcomeRepository : MongoDbOutcomeRepository
    {
        public OutcomeRepository(MongoDbCollection mongoCollection) : base(mongoCollection) { }

        public OutcomeRepository(string connectionStringName) : base(connectionStringName) { }

        public OutcomeRepository(string connectionStringName, string collectionName) : base(connectionStringName, collectionName) { }

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