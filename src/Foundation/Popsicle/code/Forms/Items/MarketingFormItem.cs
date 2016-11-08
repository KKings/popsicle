namespace KKings.Foundation.Popsicle.Forms.Items
{
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Data.Managers;

    public class MarketingFormItem : CustomItem
    {
        public static class Constants
        {
            public static readonly ID MarketingFormTemplateId = ID.Parse("{97498E65-1427-4613-80C4-1AB77E549C3E}");

            public static readonly ID FormIdFieldId = ID.Parse("{4BD70981-DF66-433B-B7DB-7F9478480E78}");
        }

        public MarketingFormItem(Item innerItem) : base(innerItem) { }

        /// <summary>
        /// Convert an <see cref="Item"/> to a <see cref="MarketingFormItem"/>
        /// </summary>
        /// <param name="item">The Item</param>
        public static implicit operator MarketingFormItem(Item item)
        {
            if (item == null)
            {
                return null;
            }

            var template = TemplateManager.GetTemplate(item.TemplateID, item.Database);

            if (!template.InheritsFrom(Constants.MarketingFormTemplateId))
            {
                return null;
            }

            return new MarketingFormItem(item);
        }

        /// <summary>
        /// Form Id
        /// </summary>
        public string FormId
        {
            get { return this.InnerItem[Constants.FormIdFieldId]; }
        }
    }
}