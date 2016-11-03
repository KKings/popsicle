namespace KKings.Foundation.Popsicle.Pipelines.MAFilterPageEvents
{
    public class FilterNonGoalEvents
    {
        public void Process(FilterPageEventsArgs args)
        {
            if (!args.PageEvent?.IsGoal ?? true)
            {
                args.IsFiltered = true;
            }
        }
    }
}