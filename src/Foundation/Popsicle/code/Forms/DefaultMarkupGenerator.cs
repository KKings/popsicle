namespace KKings.Foundation.Popsicle.Forms
{
    using System.Web;
    using System.Web.Mvc;
    using Items;

    public class DefaultMarkupGenerator : IMarkupGenerator
    {
        public string Generate(MarketingFormItem item)
        {
            var span = new TagBuilder("span")
            {
                InnerHtml = $"Markup automatically generated {item.FormId}. Please create a class and implement {typeof(IMarkupGenerator)} to generate specific markup."
            };

            return span.ToString(TagRenderMode.Normal);
        }
    }
}