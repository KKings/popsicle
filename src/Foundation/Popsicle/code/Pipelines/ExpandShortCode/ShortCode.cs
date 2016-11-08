namespace KKings.Foundation.Popsicle.Pipelines.ExpandShortCode
{
    using System;

    /// <summary>
    /// Stores shortcode before and after content
    /// </summary>
    public class ShortCode
    {
        /// <summary>
        /// Gets or sets the Unexpanded Shortcode Content
        /// <para>As entered within the Content</para>
        /// </summary>
        public string Unexpanded { get; set; } = String.Empty;

        /// <summary>
        /// Gets or sets the Expanded Shortcode Content
        /// <para>After a Processor has expanded the content</para>
        /// </summary>
        public string Expanded { get; set; } = String.Empty;
    }
}