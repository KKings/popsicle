namespace KKings.Foundation.Popsicle.Pipelines.ExpandShortCode
{
    using Sitecore.Pipelines;

    /// <summary>
    /// Stores content containing shortcodes
    /// </summary>
    public class ExpandShortCodeArgs : PipelineArgs
    {
        /// <summary>
        /// Gets or sets the content containing shortcodes
        /// </summary>
        public string Content { get; set; }
    }
}