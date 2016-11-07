namespace KKings.Foundation.Popsicle.xDb.Ingest
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using DataReader;
    using Microsoft.Extensions.DependencyInjection;
    using Resolvers;
    using Sitecore.Abstractions;
    using Sitecore.Analytics.Tracking;
    using Sitecore.DependencyInjection;
    using Sitecore.IO;

    public class IngestionManager : IIngestionManager
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IDataReader dataReader;

        /// <summary>
        /// 
        /// </summary>
        public virtual List<IHydratorResolver> Resolvers { get; } = new List<IHydratorResolver>();

        public IngestionManager(IDataReader reader)
        {
            this.dataReader = reader;
        }

        public void Ingest(Contact contact)
        {
            if (!this.Resolvers.Any())
            {
                var baseLog = ServiceLocator.ServiceProvider.GetService<BaseLog>();

                baseLog?.Warn("No resolvers have been configured. Please configure resolvers for the IngestionManager.", this);
                return;
            }

            using (var stream = this.dataReader.GetDataStream())
            {
                foreach (var hydrator in this.Resolvers.Select(resolver => resolver.Resolve(contact)))
                {
                    hydrator.Hydrate(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.Position = 0;
                }
            }
        }
    }
}