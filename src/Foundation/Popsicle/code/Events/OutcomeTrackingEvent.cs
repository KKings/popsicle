namespace KKings.Foundation.Popsicle.Events
{
    using System;

    public class OutcomeTrackingEvent : ITrackingEvent
    {
        /// <summary>
        /// Gets or sets the DateTime of when the Outcome was triggered
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Gets or sets the DefinitionId of the Outcome
        /// </summary>
        public Guid DefinitionId { get; set; }

        /// <summary>
        /// Gets or sets the Data associated with the Outcome
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Gets or sets the DataKey associated with the Outcome
        /// </summary>
        public string DataKey { get; set; }

        /// <summary>
        /// Gets or sets the Text associated with the Outcome
        /// </summary>
        public string Text { get; set; }
    }
}