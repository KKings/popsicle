namespace KKings.Foundation.Popsicle.xDb.Ingest.Hydrators
{
    using System.IO;

    public interface IHydrator
    {
        object Target { get; }
        void Hydrate(Stream source);
    }
}