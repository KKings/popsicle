namespace KKings.Foundation.Popsicle.xDb.Ingest.Resolvers
{
    using System;
    using Hydrators;
    using Sitecore.Analytics.Tracking;
    using Sitecore.Diagnostics;

    public class DefaultHydratorResolver : IHydratorResolver
    {
        /// <summary>
        /// Gets or sets the Facet Name
        /// </summary>
        public string FacetName { get; set; }

        /// <summary>
        /// Gets or sets the Hydrator Type Name
        /// </summary>
        public string HydratorTypeName { get; set; }

        /// <summary>
        /// Gets or sets the Facet Type Name
        /// </summary>
        public string FacetTypeName { get; set; }

        /// <summary>
        /// Type of Hydrator to instantiate
        /// </summary>
        public virtual Type HydratorType => String.IsNullOrEmpty(this.HydratorTypeName) ? null : Type.GetType(this.HydratorTypeName);

        /// <summary>
        /// Facet Type used to get the method from the Contact Record
        /// </summary>
        public virtual Type FacetType  => String.IsNullOrEmpty(this.FacetTypeName) ? null : Type.GetType(this.FacetTypeName);

        /// <summary>
        /// Responsible for instantiating the Configured Facet
        /// </summary>
        /// <param name="contact">Contact from xDb</param>
        /// <returns>Implementation of the IHydrator</returns>
        public virtual IHydrator Resolve(Contact contact)
        {
            if (contact == null)
            {
                Log.Error("No contact is available for this session so nothing can be hydrated", this);
                return null;
            }

            var type = this.HydratorType;

            if (type == null)
            {
                Log.Error("Could not determine hydrator type.", this);
                return null;
            }
            
            var getFacetMethod = contact.GetType().GetMethod("GetFacet");
            var genericGetFacetMethod = getFacetMethod.MakeGenericMethod(this.FacetType);
            var contactFacet = genericGetFacetMethod.Invoke(contact, new object[] { this.FacetName });

            // Create an instance of the IHydrator and pass in the Contact Facet object
            var hydrator = Activator.CreateInstance(type, contactFacet) as IHydrator;

            if (hydrator == null)
            {
                Log.SingleError($"The type {type.FullName} is not a {typeof(IHydrator).FullName}", this);
                return null;
            }

            return hydrator;
        }
    }
}