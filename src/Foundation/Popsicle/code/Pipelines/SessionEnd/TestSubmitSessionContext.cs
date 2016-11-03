using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KKings.Foundation.Popsicle.Pipelines.SessionEnd
{
    using Sitecore.Analytics;
    using Sitecore.Analytics.Outcome;
    using Sitecore.Analytics.Outcome.Extensions;
    using Sitecore.Analytics.Outcome.Model;
    using Sitecore.Analytics.Pipelines.SubmitSessionContext;
    using Sitecore.Diagnostics;

    public class TestSubmitSessionContext : SubmitSessionContextProcessor
    {
        private OutcomeManager OutcomeManager { get; set; }

        public override void Process(SubmitSessionContextArgs args)
        {
            foreach (var outcome in Tracker.Current.GetContactOutcomes())
            {
                ;
            }
        }
    }
}