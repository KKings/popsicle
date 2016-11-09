namespace KKings.Foundation.Popsicle.Forms
{
    using System.Web;
    using Items;

    public interface IMarkupGenerator
    {
        string Generate(MarketingFormItem item);
    }
}
