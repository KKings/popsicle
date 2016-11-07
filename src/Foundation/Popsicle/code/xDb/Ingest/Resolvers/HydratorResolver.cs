namespace KKings.Foundation.Popsicle.xDb.Ingest.Resolvers
{
    using System;
    using Hydrators;
    using Sitecore.Analytics.Tracking;
    using Sitecore.Diagnostics;

    public class HydratorResolver : IHydratorResolver
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
        /// 
        /// </summary>
        public virtual Type HydratorType => String.IsNullOrEmpty(this.HydratorTypeName) ? null : Type.GetType(this.HydratorTypeName);

        /// <summary>
        /// 
        /// </summary>
        public virtual Type FacetType  => String.IsNullOrEmpty(this.FacetTypeName) ? null : Type.GetType(this.FacetTypeName);

        public virtual IHydrator Resolve(Contact contact)
        {
            var type = this.HydratorType;

            if (type == null)
            {
                Log.Error("Could not determine hydrator type.", this);
                return null;
            }
            
            if (contact == null)
            {
                Log.Error("No contact is available for this session so nothing can be hydrated", this);
                return null;
            }

            var getFacetMethod = contact.GetType().GetMethod("GetFacet");
            var genericGetFacetMethod = getFacetMethod.MakeGenericMethod(this.FacetType);
            var contactFacet = genericGetFacetMethod.Invoke(contact, new object[] { this.FacetName });

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