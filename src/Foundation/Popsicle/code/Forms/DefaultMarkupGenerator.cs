namespace KKings.Foundation.Popsicle.Forms
{
    using System.Web;
    using System.Web.Mvc;
    using Items;

    public class DefaultMarkupGenerator : IMarkupGenerator
    {
        public IHtmlString Generate(MarketingFormItem item)
        {
            var span = new TagBuilder("span")
            {
                InnerHtml = $"Markup automatically generated {item.FormId}. Please create a class and implement {typeof(IMarkupGenerator)} to generate specific markup."
            };

            return new HtmlString(span.ToString(TagRenderMode.Normal));
        }
    }
}