namespace KKings.Foundation.Popsicle.Pipelines.ExpandShortCodes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Forms;
    using Forms.Items;
    using Sitecore;
    using Sitecore.Abstractions;
    using Sitecore.Data;

    public class MarketingFormExpander : ExpandShortCodeProcessor
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual string Token { get; } = "form";

        /// <summary>
        /// 
        /// </summary>
        public const string RegexFormat = @"<span\sstyle\s*=\s*(?:""|')(?:.*)>\s?(\[{0}\sid=""(?<id>.*)""\])\s?<\/span>";

        /// <summary>
        /// 
        /// </summary>
        public const string GuidRegexFormat = @"(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\})";

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsPreviewMode {  get { return Context.PageMode.IsPreview;  } }

        /// <summary>
        /// 
        /// </summary>
        public virtual Database ContextDatabase {  get { return Context.Database;  } }

        /// <summary>
        /// 
        /// </summary>
        public virtual IMarkupGenerator MarkupGenerator { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="factory"></param>
        public MarketingFormExpander(BaseFactory factory)
        {
            this.MarkupGenerator = factory.CreateObject("forms/generator", true) as IMarkupGenerator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public override IList<ShortCode> GetShortCodes(string content)
        {
            if (String.IsNullOrWhiteSpace(content))
            {
                return new List<ShortCode>();
            }

            var regex = String.Format(MarketingFormExpander.RegexFormat, this.Token);

            // Find all matches within the content based on the MarketoRegex
            var matches = Regex.Matches(content, regex, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            return matches.Cast<Match>()
                          .Select(this.ConvertMatch)
                          .Select(this.ExpandShortCode)
                          .ToList();
        }

        /// <summary>
        /// Creates a ShortCode from a RegEx match.
        /// </summary>
        /// <param name="match">The match result.</param>
        /// <returns>A new ShortCode object.</returns>
        public virtual ShortCode ExpandShortCode(RegexMatch match)
        {
            var shortCode = new ShortCode
            {
                Unexpanded = match.Value
            };

            ID formId;
            
            if (!String.IsNullOrEmpty(match.Id) && ID.TryParse(match.Id, out formId))
            {
                var item = this.ContextDatabase.GetItem(formId);

                if (item != null)
                {
                    shortCode.Expanded = this.MarkupGenerator.Generate(new MarketingFormItem(item));
                    return shortCode;
                }
            }

            if (this.IsPreviewMode)
            {
                shortCode.Expanded = $"<!-- {match.Id} was not found. -->";
                return shortCode;
            }

            return shortCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public virtual RegexMatch ConvertMatch(Match match)
        {
            var value = match.Value;
            var id = String.Empty;
            var tempId = match.Groups["id"].Value;

            if (!String.IsNullOrEmpty(tempId))
            {
                /**
                 * Since the content editor can freely type, sometimes the RTE
                 * will add html into the id attribute. This does an additional
                 * check to pull out the first matching guid. 
                 */
                var matches = Regex.Matches(tempId, MarketingFormExpander.GuidRegexFormat);

                if (matches.Count > 0)
                {
                    id = matches[0].Value;
                }
            }

            return new RegexMatch(value, id);
        }
    }
}