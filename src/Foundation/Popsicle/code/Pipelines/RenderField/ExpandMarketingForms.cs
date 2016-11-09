namespace KKings.Foundation.Popsicle.Pipelines.RenderField
{
    using System.Linq;
    using ExpandShortCodes;
    using Sitecore.Diagnostics;
    using Sitecore.Pipelines;
    using Sitecore.Pipelines.RenderField;

    public class ExpandMarketingForms
    {
        /// <summary>
        /// Fields that potentially contain ShortCodes that need to be expanded
        /// </summary>
        private static readonly string[] FIELDS = { "rich text", "multi-line text" };

        /// <summary>
        /// Main method called within RenderField pipeline
        /// </summary>
        /// <param name="args"></param>
        public void Process(RenderFieldArgs args)
        {
            if (!ExpandMarketingForms.CanFieldBeProcessed(args))
            {
                return;
            }

            args.Result.FirstPart = ExpandMarketingForms.GetExpandedShortCodes(args.Result.FirstPart);
        }

        /// <summary>
        /// Verifies that the field being rendered should be processed
        /// </summary>
        /// <param name="args">Instance of RenderFieldArgs</param>
        /// <returns>True if can be processed</returns>
        private static bool CanFieldBeProcessed(RenderFieldArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Assert.ArgumentNotNull(args.FieldTypeKey, "args.FieldTypeKey");

            var fieldTypeKey = args.FieldTypeKey.ToLower();

            return ExpandMarketingForms.FIELDS.Any(f => f.Equals(fieldTypeKey));
        }

        /// <summary>
        /// Runs the expandShortCodes Pipeline
        /// </summary>
        /// <param name="content">Content with Possible ShortCodes</param>
        /// <returns>Expanded ShortCodes within Content</returns>
        private static string GetExpandedShortCodes(string content)
        {
            Assert.ArgumentNotNull(content, "content");

            var args = new ExpandShortCodeArgs
            {
                Content = content
            };

            CorePipeline.Run("expandShortCodes", args);

            return args.Content;
        }
    }
}