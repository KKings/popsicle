namespace KKings.Foundation.Popsicle.Pipelines.MABuildTrackingOutcome
{
    using System;
    using Events;
    using Sitecore.Analytics.Outcome.Model;
    using Sitecore.Pipelines;

    public class BuildTrackingOutcomeArgs : PipelineArgs
    {
        /// <summary>
        /// Gets the Outcome to Build
        /// </summary>
        public IOutcome Outcome { get; private set; }

        /// <summary>
        /// Gets or Sets the Tracking Event
        /// </summary>
        public ITrackingEvent TrackingEvent { get; set; }

        public BuildTrackingOutcomeArgs(IOutcome outcome)
        {
            if (outcome == null)
            {
                throw new ArgumentNullException($"{nameof(outcome)}");
            }

            this.Outcome = outcome;
        }
    }
}