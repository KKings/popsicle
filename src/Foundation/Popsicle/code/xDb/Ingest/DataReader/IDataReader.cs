namespace KKings.Foundation.Popsicle.xDb.Ingest.DataReader
{
    using System.IO;

    /// <summary>
    /// Reads data from an external system
    /// </summary>
    public interface IDataReader
    {
        Stream GetDataStream();
    }
}