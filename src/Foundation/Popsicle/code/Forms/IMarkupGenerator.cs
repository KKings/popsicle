namespace KKings.Foundation.Popsicle.Forms
{
    using System.Web;
    using Items;

    public interface IMarkupGenerator
    {
        IHtmlString Generate(MarketingFormItem item);
    }
}
