namespace KKings.Foundation.Popsicle.Pipelines.FilterOutcomes
{
    using Sitecore.Analytics.Outcome.Model;
    using Sitecore.Pipelines;

    public class FilterTrackingOutcomesArgs : PipelineArgs
    {
        /// <summary>
        /// Gets or sets if the Page Event should be filtered
        /// </summary>
        public bool IsFiltered { get; set; } = false;

        /// <summary>
        /// Gets the Page Event
        /// </summary>
        public IOutcome Outcome { get; private set; }

        public FilterTrackingOutcomesArgs(IOutcome outcome)
        {
            this.Outcome = outcome;
        }
    }
}