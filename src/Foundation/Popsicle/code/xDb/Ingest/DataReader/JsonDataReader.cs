namespace KKings.Foundation.Popsicle.xDb.Ingest.DataReader
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using Sitecore.Abstractions;
    using Sitecore.Collections;
    using Sitecore.Diagnostics;
    using Sitecore.Extensions.StringExtensions;

    public class JsonDataReader : IDataReader
    {
        /// <summary>
        /// Logging Implementation
        /// </summary>
        private readonly BaseLog logger;

        /// <summary>
        /// Gets the Endpoint
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// Gets the Query String Parameter
        /// </summary>
        public IDictionary<string, string> Parameters { get; }

        public JsonDataReader(BaseLog logger)
        {
            this.logger = logger;
            this.Parameters = new HashDictionary<string, string>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual Stream GetDataStream()
        {
            var uri = this.GetUri(this.Endpoint, this.Parameters);

            try
            {
                var request = WebRequest.Create(uri);
                var response = request.GetResponse();
                var stream = response.GetResponseStream();
                return stream;
            }
            catch (WebException ex)
            {
                Log.Warn($"This exception may not indicate an error. Check the documentation for the endpoint {this.Endpoint}", ex, this);
                return null;
            }
        }

        /// <summary>
        /// Converts a string dictionary into a query string.
        /// </summary>
        /// <param name="parameters">The keys and values that are converted into a format compatible with query strings</param>
        /// <param name="doNotEncode">If false the keys and values will be URL encoded, if true the keys and values will not be URL encoded</param>
        /// <returns>A string that can be used as the query string on a uri</returns>
        protected virtual string GetQueryString(IDictionary<string, string> parameters, bool doNotEncode = false)
        {
            if (parameters == null)
            {
                return null;
            }

            return doNotEncode
                ? parameters.Select(p => $"{p.Key}={p.Value}").Aggregate((a, b) => a + "&" + b)
                : parameters.Select(p => $"{HttpUtility.UrlEncode(p.Key)}={HttpUtility.UrlEncode(p.Value)}").Aggregate((a, b) => a + "&" + b);
        }

        /// <summary>
        /// Gets a Uri with the specified parameters appended to the query string
        /// </summary>
        /// <param name="url">The URL, such as http://localhost/path</param>
        /// <param name="parameters">The keys and values that are appended to the query string</param>
        /// <param name="doNotEncode">If false the keys and values will be URL encoded, if true the keys and values will not be URL encoded</param>
        /// <returns>A Uri with the specified parameters appended to the query string</returns>
        protected virtual Uri GetUri(string url, IDictionary<string, string> parameters, bool doNotEncode = false)
        {
            Assert.ArgumentNotNullOrEmpty(url, "url");
            Assert.ArgumentNotNull(parameters, "parameters");

            var queryString = this.GetQueryString(parameters, doNotEncode);
            var builder = new UriBuilder(url);

            if (!String.IsNullOrEmpty(builder.Query))
            {
                queryString = builder.Query.Right(builder.Query.Length - 1) + "&" + queryString;
            }

            builder.Query = queryString;

            return builder.Uri;
        }
    }
}