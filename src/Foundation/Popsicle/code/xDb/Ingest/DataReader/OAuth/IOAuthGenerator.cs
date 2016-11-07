
namespace KKings.Foundation.Popsicle.xDb.Ingest.DataReader.OAuth
{
    using System;
    using System.Security.Cryptography;

    public interface IOAuthGenerator
    {
        string GenerateNonce();
        string GenerateSignature(Uri url, string consumerKey, string consumerSecret, string token, string tokenSecret, string httpMethod, string timeStamp, string nonce, out string normalizedUrl, out string normalizedRequestParameters);
        string GenerateSignature(Uri url, string consumerKey, string consumerSecret, string token, string tokenSecret, string httpMethod, string timeStamp, string nonce, SignatureTypes signatureType, out string normalizedUrl, out string normalizedRequestParameters);
        string GenerateSignatureBase(Uri url, string consumerKey, string token, string tokenSecret, string httpMethod, string timeStamp, string nonce, string signatureType, out string normalizedUrl, out string normalizedRequestParameters);
        string GenerateSignatureUsingHash(string signatureBase, HashAlgorithm hash);
        string GenerateTimeStamp();
    }
}