using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using KKings.Foundation.Popsicle.Pipelines.ExpandShortCode;
using Sitecore;

namespace KKings.Foundation.Popsicle.Pipelines.ExpandShortCode
{
    public class MarketingFormExpander : ExpandShortCodeProcessor
    {
        /// <summary>
        /// ID Not Found Format to display to Content Author
        /// </summary>
        private const string FormNotFoundFormat = "<i>Id, '{0}', cannot be found or invalid format.</i>";

        /// <summary>
        /// 
        /// </summary>
        private readonly string _token;

        /// <summary>
        /// 
        /// </summary>
        private const string RegexFormat = @"\[{0} id=""(?<id>.*)""\]";

        /// <summary>
        /// Creates a new instance with the specified Token
        /// </summary>
        public MarketingFormExpander(string token)
        {
            if (String.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            this._token = token;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected override IList<ShortCode> GetShortCodes(string content)
        {
            if (String.IsNullOrWhiteSpace(content))
            {
                return new List<ShortCode>();
            }

            // Find all matches within the content based on the MarketoRegex
            var matches = Regex.Matches(content, String.Format(MarketingFormExpander.RegexFormat, this._token),
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

            return matches.Cast<Match>()
                          .Select(this.GetShortCode)
                          .ToList();
        }

        /// <summary>
        /// Creates a ShortCode from a RegEx match.
        /// </summary>
        /// <param name="match">The match result.</param>
        /// <returns>A new ShortCode object.</returns>
        private ShortCode GetShortCode(Match match)
        {
            var shortCode = new ShortCode
            {
                Unexpanded = match.Value
            };

            var formId = match.Groups["id"].Value;

            if (!String.IsNullOrEmpty(formId))
            {
                /**
             * Since the content editor can freely type, sometimes the RTE
             * will add html into the id attribute. This does an additional
             * check to pull out the first matching guid. */
                var matches = Regex.Matches(formId,
                    @"(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\})");

                if (matches.Count > 0)
                {
                    shortCode.Expanded = "";
                    return shortCode;
                }
            }

            if (Context.PageMode.IsPreview)
            {
                shortCode.Expanded = $"<!-- {formId} was not found. -->";
                return shortCode;
            }

            return shortCode;
        }
    }
}