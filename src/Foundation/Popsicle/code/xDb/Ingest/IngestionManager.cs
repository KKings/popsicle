namespace KKings.Foundation.Popsicle.xDb.Ingest
{
    using DataReader;
    using Resolvers;
    using Sitecore.Analytics.Tracking;

    public class IngestionManager : IIngestionManager
    {
        private readonly IDataReader dataReader;
        private readonly IHydratorResolver resolver;

        public IngestionManager(IDataReader reader, IHydratorResolver resolver)
        {
            this.dataReader = reader;
            this.resolver = resolver;
        }

        public void Ingest(Contact contact)
        {
            var hydrator = this.resolver.Resolve(contact);

            hydrator?.Hydrate(this.dataReader.GetDataStream());
        }
    }
}