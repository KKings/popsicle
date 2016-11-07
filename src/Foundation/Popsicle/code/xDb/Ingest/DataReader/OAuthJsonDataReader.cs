namespace KKings.Foundation.Popsicle.xDb.Ingest.DataReader
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using OAuth;
    using Sitecore.Abstractions;

    public class OAuthJsonDataReader : JsonDataReader
    {
        /// <summary>
        /// Base Implementation for oAuth
        /// </summary>
        private readonly IOAuthGenerator oAuthGenerator;

        /// <summary>
        /// Gets or sets if the Reader should use OAuth for authentication
        /// </summary>
        public bool UseOAuth { get; set; }

        /// <summary>
        /// Gets or sets the OAuth Api Key
        /// </summary>
        public string OAuthApiKey { get; set; }

        /// <summary>
        /// Gets or sets the OAuth Secret
        /// </summary>
        public string OAuthSecret { get; set; }

        public OAuthJsonDataReader(BaseLog logger, IOAuthGenerator oAuthGenerator) : base(logger)
        {
            this.oAuthGenerator = oAuthGenerator;
        }

        /// <summary>
        /// Gets a signed Uri for the specified endpoint with the specified parameters appended to the query string.
        /// This Uri can be used with REST APIs that use OAuth.
        /// </summary>
        /// <param name="url">The url that will have the parameters appended to its query string</param>
        /// <param name="parameters">The keys and values that are appended to the query string</param>
        /// <param name="doNotEncode"></param>
        /// <returns>A signed Uri for the endpoint with the parameters appended to the query string</returns>
        protected override Uri GetUri(string url, IDictionary<string, string> parameters, bool doNotEncode = false)
        {
            // ReSharper disable once BaseMethodCallWithDefaultParameter
            var uri = base.GetUri(url, parameters);
            string nurl, nreq;

            var nounce = this.oAuthGenerator.GenerateNonce();
            var timestamp = this.oAuthGenerator.GenerateTimeStamp();

            var signatureUrl = this.oAuthGenerator.GenerateSignature(
                url: uri,
                consumerKey: this.OAuthApiKey,
                consumerSecret: this.OAuthSecret,
                token: String.Empty,
                tokenSecret: String.Empty,
                httpMethod: "GET",
                timeStamp: timestamp,
                nonce: nounce,
                signatureType: SignatureTypes.HMACSHA1,
                normalizedUrl: out nurl,
                normalizedRequestParameters: out nreq);

            signatureUrl = HttpUtility.UrlEncode(signatureUrl);

            var parameters2 = new Dictionary<string, string>
            {
                { "oauth_consumer_key", this.OAuthApiKey },
                { "oauth_nonce", nounce },
                { "oauth_timestamp", timestamp },
                { "oauth_signature_method", "HMAC-SHA1" },
                { "oauth_version", "1.0" },
                { "oauth_signature", signatureUrl }
            };

            return base.GetUri(uri.ToString(), parameters2, true);
        }
    }
}