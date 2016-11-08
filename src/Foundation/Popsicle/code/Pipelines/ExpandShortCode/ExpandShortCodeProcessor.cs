namespace KKings.Foundation.Popsicle.Pipelines.ExpandShortCode
{
    using System;
    using System.Collections.Generic;
    using Sitecore.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Processor to expand all ShortCodes
    /// <para>Each Processor must fill in how to retrieve each ShortCode</para>
    /// </summary>
    public abstract class ExpandShortCodeProcessor
    {
        /// <summary>
        /// Iterates over all ShortCode instances and replaces a ShortCode with the expanded content
        /// </summary>
        /// <param name="args"></param>
        public virtual void Process(ExpandShortCodeArgs args)
        {
            if (String.IsNullOrWhiteSpace(args.Content))
            {
                return;
            }

            var shortCodes = this.GetShortCodes(args.Content);

            if (!shortCodes.Any())
            {
                return;
            }

            args.Content = this.ExpandShortCodes(shortCodes, args.Content);
        }

        /// <summary>
        /// Expands all ShortCodes
        /// </summary>
        /// <param name="shortCodes">List of ShortCodes</param>
        /// <param name="content">Original Content</param>
        /// <returns>Content with ShortCodes expanded</returns>
        protected virtual string ExpandShortCodes(IList<ShortCode> shortCodes, string content)
        {
            Assert.ArgumentNotNull(shortCodes, "shortcodes");
            Assert.ArgumentNotNull(content, "content");

            return shortCodes.Aggregate(content,
                (current, shortcode) => current.Replace(shortcode.Unexpanded, shortcode.Expanded));
        }

        /// <summary>
        /// Search for all ShortCodes within a string of content
        /// </summary>
        /// <param name="content">Content with potential ShortCodes</param>
        /// <returns>An array of ShortCodes</returns>
        protected abstract IList<ShortCode> GetShortCodes(string content);
    }