namespace KKings.Foundation.Popsicle.Pipelines.ExpandShortCodes
{
    using System;

    public class RegexMatch
    {
        /// <summary>
        /// Gets the Value
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Gets the Id
        /// </summary>
        public string Id { get; private set; }

        public RegexMatch(String value, String id)
        {
            this.Value = value;
            this.Id = id;
        }
    }
}