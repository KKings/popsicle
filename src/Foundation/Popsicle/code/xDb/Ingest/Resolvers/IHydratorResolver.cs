namespace KKings.Foundation.Popsicle.xDb.Ingest.Resolvers
{
    using Hydrators;
    using Sitecore.Analytics.Tracking;

    public interface IHydratorResolver
    {
        IHydrator Resolve(Contact contact);
    }
}