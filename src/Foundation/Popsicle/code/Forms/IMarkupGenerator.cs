namespace KKings.Foundation.Popsicle.Forms
{
    using Items;

    public interface IMarkupGenerator
    {
        string Generate(MarketingFormItem item);
    }
}
