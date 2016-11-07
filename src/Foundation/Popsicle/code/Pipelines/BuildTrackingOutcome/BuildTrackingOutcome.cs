namespace KKings.Foundation.Popsicle.Pipelines.BuildTrackingOutcome
{
    using System.Globalization;
    using Events;
    using Sitecore.Abstractions;
    using Sitecore.Analytics.Data.Items;
    using Sitecore.Data;

    public class BuildTrackingOutcome
    {
        private readonly BaseLog logger;
        private readonly BaseFactory factory;

        public virtual Database ContextDatabase => Sitecore.Context.Database ?? this.factory.GetDatabase("web");

        public BuildTrackingOutcome(BaseLog logger, BaseFactory factory)
        {
            this.logger = logger;
            this.factory = factory;
        }

        public void Process(BuildTrackingOutcomeArgs args)
        {
            var outcome = args.Outcome;

            var definitionItem = this.ContextDatabase.GetItem(outcome.DefinitionId);

            if (definitionItem == null)
            {
                this.logger.Warn($"Unable to find item with ID, {outcome.DefinitionId}", this);
                return;
            }

            var trackingEvent = new OutcomeTrackingEvent
            {
                DefinitionId = outcome.DefinitionId.Guid,
                DateTime = outcome.DateTime,
                Text = definitionItem[OutcomeDefinitionItem.FieldIDs.NameField],
                Data = outcome.MonetaryValue.ToString(CultureInfo.InvariantCulture)
            };

            args.TrackingEvent = trackingEvent;
        }

    }
}