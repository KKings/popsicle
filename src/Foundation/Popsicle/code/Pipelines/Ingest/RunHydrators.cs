namespace KKings.Foundation.Popsicle.Pipelines.Ingest
{
    using Sitecore.Abstractions;
    using xDb.Ingest;

    public class RunHydrators
    {
        private readonly BaseFactory factory;

        public RunHydrators(BaseFactory factory)
        {
            this.factory = factory;
        }

        public virtual void Process(IngestionPipelineArgs args)
        {
            var ingestionManager = this.factory.CreateObject("ingestion/ingestionManager", true) as IIngestionManager;

            ingestionManager?.Ingest(args.Contact);
        }
    }
}