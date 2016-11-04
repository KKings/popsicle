namespace KKings.Foundation.Popsicle.Pipelines.MAFlushTrackingEvents
{
    using System.Linq;

    public class EventsJobRunner
    {
        public void Process(FlushTrackingEventArgs args)
        {
            if (!args.TrackingEvents.Any())
            {
                return;
            }

            // Todo: JobRunner
        }
    }
}