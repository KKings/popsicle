﻿namespace KKings.Foundation.Popsicle.xDb.Ingest.DataReader.OAuth
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;

    public class OAuthGenerator : IOAuthGenerator
    {
        /// <summary>
        /// Provides an internal structure to sort the query parameter
        /// </summary>
        protected class QueryParameter
        {

            /// <summary>
            /// Gets the Query Parameter Name
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// Gets the Query Parameter Value
            /// </summary>
            public string Value { get; }

            public QueryParameter(string name, string value)
            {
                this.Name = name;
                this.Value = value;
            }
        }

        /// <summary>
        /// Comparer class used to perform the sorting of the query parameters
        /// </summary>
        protected class QueryParameterComparer : IComparer<QueryParameter>
        {
            public int Compare(QueryParameter x, QueryParameter y)
            {
                return x.Name == y.Name ? String.CompareOrdinal(x.Value, y.Value) : String.CompareOrdinal(x.Name, y.Name);
            }
        }

        protected const string OAuthVersion = "1.0";
        protected const string OAuthParameterPrefix = "oauth_"; 
        protected const string OAuthConsumerKeyKey = "oauth_consumer_key";
        protected const string OAuthCallbackKey = "oauth_callback";
        protected const string OAuthVersionKey = "oauth_version";
        protected const string OAuthSignatureMethodKey = "oauth_signature_method";
        protected const string OAuthSignatureKey = "oauth_signature";
        protected const string OAuthTimestampKey = "oauth_timestamp";
        protected const string OAuthNonceKey = "oauth_nonce";
        protected const string OAuthTokenKey = "oauth_token";
        protected const string OAuthTokenSecretKey = "oauth_token_secret";

        protected const string HMACSHA1SignatureType = "HMAC-SHA1";
        protected const string PlainTextSignatureType = "PLAINTEXT";
        protected const string RSASHA1SignatureType = "RSA-SHA1";

        protected Random Random = new Random();

        protected string UnreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

        /// <summary>
        /// Helper function to compute a hash value
        /// </summary>
        /// <param name="hashAlgorithm">The hashing algoirhtm used. If that algorithm needs some initialization, like HMAC and its derivatives, they should be initialized prior to passing it to this function</param>
        /// <param name="data">The data to hash</param>
        /// <returns>a Base64 string of the hash value</returns>
        private string ComputeHash(HashAlgorithm hashAlgorithm, string data)
        {
            if (hashAlgorithm == null)
            {
                throw new ArgumentNullException(nameof(hashAlgorithm));
            }

            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException(nameof(data));
            }

            var dataBuffer = Encoding.ASCII.GetBytes(data);
            var hashBytes = hashAlgorithm.ComputeHash(dataBuffer);

            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Internal function to cut out all non oauth query string parameters (all parameters not begining with "oauth_")
        /// </summary>
        /// <param name="parameters">The query string part of the Url</param>
        /// <returns>A list of QueryParameter each containing the parameter name and value</returns>
        private List<QueryParameter> GetQueryParameters(string parameters)
        {
            if (parameters.StartsWith("?"))
            {
                parameters = parameters.Remove(0, 1);
            }

            var result = new List<QueryParameter>();

            if (!string.IsNullOrEmpty(parameters))
            {
                var p = parameters.Split('&');
                foreach (var s in p)
                {
                    if (!string.IsNullOrEmpty(s) && !s.StartsWith(OAuthGenerator.OAuthParameterPrefix))
                    {
                        if (s.IndexOf('=') > -1)
                        {
                            var temp = s.Split('=');
                            result.Add(new QueryParameter(temp[0], temp[1]));
                        }
                        else
                        {
                            result.Add(new QueryParameter(s, string.Empty));
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// This is a different Url Encode implementation since the default .NET one outputs the percent encoding in lower case.
        /// While this is not a problem with the percent encoding spec, it is used in upper case throughout OAuth
        /// </summary>
        /// <param name="value">The value to Url encode</param>
        /// <returns>Returns a Url encoded string</returns>
        protected string UrlEncode(string value)
        {
            var result = new StringBuilder();

            foreach (var symbol in value)
            {
                if (this.UnreservedChars.IndexOf(symbol) != -1)
                {
                    result.Append(symbol);
                }
                else
                {
                    result.Append('%' + $"{(int)symbol:X2}");
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Normalizes the request parameters according to the spec
        /// </summary>
        /// <param name="parameters">The list of parameters already sorted</param>
        /// <returns>a string representing the normalized parameters</returns>
        protected string NormalizeRequestParameters(IList<QueryParameter> parameters)
        {
            var sb = new StringBuilder();
            QueryParameter p = null;
            for (var i = 0; i < parameters.Count; i++)
            {
                p = parameters[i];
                sb.Append($"{p.Name}={p.Value}");

                if (i < parameters.Count - 1)
                {
                    sb.Append("&");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Generate the signature base that is used to produce the signature
        /// </summary>
        /// <param name="url">The full url that needs to be signed including its non OAuth url parameters</param>
        /// <param name="consumerKey">The consumer key</param>        
        /// <param name="token">The token, if available. If not available pass null or an empty string</param>
        /// <param name="tokenSecret">The token secret, if available. If not available pass null or an empty string</param>
        /// <param name="httpMethod">The http method used. Must be a valid HTTP method verb (POST,GET,PUT, etc)</param>
        /// <param name="nonce"></param>
        /// <param name="signatureType">The signature type. To use the default values use <see cref="OAuthBase.SignatureTypes">OAuthBase.SignatureTypes</see>.</param>
        /// <param name="timeStamp"></param>
        /// <param name="normalizedUrl"></param>
        /// <param name="normalizedRequestParameters"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>The signature base</returns>
        public string GenerateSignatureBase(Uri url, 
            string consumerKey, 
            string token, 
            string tokenSecret, 
            string httpMethod, 
            string timeStamp, string nonce, 
            string signatureType, 
            out string normalizedUrl, 
            out string normalizedRequestParameters)
        {
            if (token == null)
            {
                token = String.Empty;
            }

            if (tokenSecret == null)
            {
                tokenSecret = String.Empty;
            }

            if (string.IsNullOrEmpty(consumerKey))
            {
                throw new ArgumentNullException(nameof(consumerKey));
            }

            if (string.IsNullOrEmpty(httpMethod))
            {
                throw new ArgumentNullException(nameof(httpMethod));
            }

            if (string.IsNullOrEmpty(signatureType))
            {
                throw new ArgumentNullException(nameof(signatureType));
            }

            normalizedUrl = null;
            normalizedRequestParameters = null;

            var parameters = this.GetQueryParameters(url.Query);
            parameters.Add(new QueryParameter(OAuthGenerator.OAuthVersionKey, OAuthGenerator.OAuthVersion));
            parameters.Add(new QueryParameter(OAuthGenerator.OAuthNonceKey, nonce));
            parameters.Add(new QueryParameter(OAuthGenerator.OAuthTimestampKey, timeStamp));
            parameters.Add(new QueryParameter(OAuthGenerator.OAuthSignatureMethodKey, signatureType));
            parameters.Add(new QueryParameter(OAuthGenerator.OAuthConsumerKeyKey, consumerKey));

            if (!string.IsNullOrEmpty(token))
            {
                parameters.Add(new QueryParameter(OAuthGenerator.OAuthTokenKey, token));
            }

            parameters.Sort(new QueryParameterComparer());

            normalizedUrl = $"{url.Scheme}://{url.Host}";
            if (!((url.Scheme == "http" && url.Port == 80) || (url.Scheme == "https" && url.Port == 443)))
            {
                normalizedUrl += ":" + url.Port;
            }
            normalizedUrl += url.AbsolutePath;
            normalizedRequestParameters = this.NormalizeRequestParameters(parameters);

            var signatureBase = new StringBuilder();
            signatureBase.Append($"{httpMethod.ToUpper()}&");
            signatureBase.Append($"{this.UrlEncode(normalizedUrl)}&");
            signatureBase.Append($"{this.UrlEncode(normalizedRequestParameters)}");

            return signatureBase.ToString();
        }

        /// <summary>
        /// Generate the signature value based on the given signature base and hash algorithm
        /// </summary>
        /// <param name="signatureBase">The signature based as produced by the GenerateSignatureBase method or by any other means</param>
        /// <param name="hash">The hash algorithm used to perform the hashing. If the hashing algorithm requires initialization or a key it should be set prior to calling this method</param>
        /// <returns>A base64 string of the hash value</returns>
        public string GenerateSignatureUsingHash(string signatureBase, HashAlgorithm hash)
        {
            return this.ComputeHash(hash, signatureBase);
        }

        /// <summary>
        /// Generates a signature using the HMAC-SHA1 algorithm
        /// </summary>		
        /// <param name="url">The full url that needs to be signed including its non OAuth url parameters</param>
        /// <param name="consumerKey">The consumer key</param>
        /// <param name="consumerSecret">The consumer seceret</param>
        /// <param name="token">The token, if available. If not available pass null or an empty string</param>
        /// <param name="tokenSecret">The token secret, if available. If not available pass null or an empty string</param>
        /// <param name="httpMethod">The http method used. Must be a valid HTTP method verb (POST,GET,PUT, etc)</param>
        /// <param name="timeStamp"></param>
        /// <param name="nonce"></param>
        /// <param name="normalizedUrl"></param>
        /// <param name="normalizedRequestParameters"></param>
        /// <returns>A base64 string of the hash value</returns>
        public string GenerateSignature(Uri url, 
            string consumerKey, 
            string consumerSecret, 
            string token, 
            string tokenSecret, 
            string httpMethod, 
            string timeStamp, 
            string nonce, 
            out string normalizedUrl,
            out string normalizedRequestParameters)
        {
            return this.GenerateSignature(url, consumerKey, consumerSecret, token, tokenSecret, httpMethod, timeStamp, nonce, SignatureTypes.HMACSHA1, out normalizedUrl, out normalizedRequestParameters);
        }

        /// <summary>
        /// Generates a signature using the specified signatureType 
        /// </summary>		
        /// <param name="url">The full url that needs to be signed including its non OAuth url parameters</param>
        /// <param name="consumerKey">The consumer key</param>
        /// <param name="consumerSecret">The consumer seceret</param>
        /// <param name="token">The token, if available. If not available pass null or an empty string</param>
        /// <param name="tokenSecret">The token secret, if available. If not available pass null or an empty string</param>
        /// <param name="httpMethod">The http method used. Must be a valid HTTP method verb (POST,GET,PUT, etc)</param>
        /// <param name="nonce"></param>
        /// <param name="signatureType">The type of signature to use</param>
        /// <param name="timeStamp"></param>
        /// <param name="normalizedUrl"></param>
        /// <param name="normalizedRequestParameters"></param>
        /// <exception cref="NotImplementedException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>A base64 string of the hash value</returns>
        public string GenerateSignature(Uri url, 
            string consumerKey, 
            string consumerSecret, 
            string token, 
            string tokenSecret, 
            string httpMethod, 
            string timeStamp,
            string nonce, 
            SignatureTypes signatureType, 
            out string normalizedUrl, 
            out string normalizedRequestParameters)
        {
            normalizedUrl = null;
            normalizedRequestParameters = null;

            switch (signatureType)
            {
                case SignatureTypes.PLAINTEXT:
                {
                    return HttpUtility.UrlEncode($"{consumerSecret}&{tokenSecret}");
                }
                case SignatureTypes.HMACSHA1:
                {
                    var signatureBase = this.GenerateSignatureBase(url, consumerKey, token, tokenSecret, httpMethod, timeStamp,
                        nonce, OAuthGenerator.HMACSHA1SignatureType, out normalizedUrl, out normalizedRequestParameters);

                    var hmacsha1 = new HMACSHA1
                    {
                        Key = Encoding.ASCII.GetBytes(
                            $"{this.UrlEncode(consumerSecret)}&{(string.IsNullOrEmpty(tokenSecret) ? "" : this.UrlEncode(tokenSecret))}")
                    };

                    return this.GenerateSignatureUsingHash(signatureBase, hmacsha1);
                }
                case SignatureTypes.RSASHA1:
                {
                    throw new NotImplementedException();
                }
                default:
                {
                    throw new ArgumentException("Unknown signature type", nameof(signatureType));
                }
            }
        }

        /// <summary>
        /// Generate the timestamp for the signature        
        /// </summary>
        /// <returns></returns>
        public virtual string GenerateTimeStamp()
        {
            var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var timeStamp = ts.TotalSeconds.ToString(CultureInfo.InvariantCulture);

            return timeStamp.Substring(0, timeStamp.IndexOf(".", StringComparison.Ordinal));
        }

        /// <summary>
        /// Generate a nonce
        /// </summary>
        /// <returns></returns>
        public virtual string GenerateNonce()
        {
            return this.Random.Next(123400, 9999999).ToString();
        }
    }
}