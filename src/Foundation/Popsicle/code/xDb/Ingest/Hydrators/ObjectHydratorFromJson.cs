namespace KKings.Foundation.Popsicle.xDb.Ingest.Hydrators
{
    using System.IO;
    using System.Text;
    using Newtonsoft.Json;
    using Sitecore.Diagnostics;
    using System;

    public class ObjectHydratorFromJson : IHydrator
    {
        /// <summary>
        /// Gets the Target Object
        /// </summary>
        public object Target { get; }

        public ObjectHydratorFromJson(object target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            this.Target = target;
        }

        /// <summary>
        /// Hydrate the source
        /// </summary>
        /// <param name="source"></param>
        public void Hydrate(Stream source)
        {
            var data = new StreamReader(source, Encoding.UTF8).ReadToEnd();
            source.Close();

            if (String.IsNullOrEmpty(data))
            {
                Log.Warn("Data could not be read from the stream. This may not indicate an error.", this);
                return;
            }

            var settings = new JsonSerializerSettings
            {
                ObjectCreationHandling = ObjectCreationHandling.Reuse,
                NullValueHandling = NullValueHandling.Ignore
            };

            JsonConvert.PopulateObject(data, this.Target, settings);
        }
    }
}