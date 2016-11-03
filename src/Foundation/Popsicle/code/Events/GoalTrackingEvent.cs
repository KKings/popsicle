
namespace KKings.Foundation.Popsicle.Events
{
    using System;
    using Sitecore.Data;

    public class GoalTrackingEvent : ITrackingEvent
    {
        /// <summary>
        /// Gets or sets the DateTime of when the Goal was triggered
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Gets or sets the DefinitionId of the Goal
        /// </summary>
        public Guid DefinitionId { get; set; }

        /// <summary>
        /// Gets or sets the Data associated with the goal
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Gets or sets the DataKey associated with the goal
        /// </summary>
        public string DataKey { get; set; }

        /// <summary>
        /// Gets or sets the Text associated with the goal
        /// </summary>
        public string Text { get; set; }
    }
}