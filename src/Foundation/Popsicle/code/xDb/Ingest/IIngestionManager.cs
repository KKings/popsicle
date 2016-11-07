namespace KKings.Foundation.Popsicle.xDb.Ingest
{
    using Sitecore.Analytics.Tracking;

    public interface IIngestionManager
    {
        void Ingest(Contact contact);
    }
}