namespace KKings.Foundation.Popsicle.Pipelines.MABuildTrackingEvent
{
    using System;
    using Events;

    public class BuildTrackingEvent
    {
        public void Process(BuildTrackingEventArgs args)
        {
            var pageEvent = args.PageEvent;

            var trackingEvent = new GoalTrackingEvent
            {
                Data = pageEvent.Data,
                DataKey = pageEvent.DataKey,
                DateTime = pageEvent.DateTime.ToUniversalTime(),
                DefinitionId = pageEvent.PageEventDefinitionId,
                Text = !String.IsNullOrEmpty(pageEvent.Name) ? pageEvent.Name : pageEvent.Text
            };

            args.TrackingEvent = trackingEvent;
        }
    }
}