namespace KKings.Foundation.Popsicle.Pipelines.Ingest
{
    using System.Web;
    using Sitecore.Analytics.Tracking;
    using Sitecore.Pipelines;

    public class IngestionPipelineArgs : PipelineArgs
    {
        /// <summary>
        /// Gets the Contact
        /// </summary>
        public Contact Contact { get; private set; }

        /// <summary>
        /// Gets the HttpContext 
        /// </summary>
        public HttpContextBase HttpContextBase { get; private set; }

        public IngestionPipelineArgs(Contact contact, HttpContextBase httpContextBase)
        {
            this.Contact = contact;
            this.HttpContextBase = httpContextBase;
        }
    }
}