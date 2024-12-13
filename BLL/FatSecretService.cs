using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace BLL
{
    public class FatSecretService
    {
        private readonly string _consumerKey = "923be06874064d459082ec63229ec3b5";
        private readonly string _consumerSecret = "fe6e0c228a2c4d62ba9c728e881a9358";
        private readonly string _apiUrl = "https://platform.fatsecret.com/rest/server.api";

        public FatSecretService()
        {
        }

        private string GenerateNonce()
        {
            var nonce = new StringBuilder();
            var random = new Random();
            for (int i = 0; i < 8; i++)
            {
                nonce.Append((char)random.Next('a', 'z' + 1));
            }
            return nonce.ToString();
        }

        private string GenerateTimeStamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        }

        private string GenerateSignature(string url, string nonce, string timestamp, SortedDictionary<string, string> parameters)
        {
            var signatureBase = new StringBuilder();
            signatureBase.Append("POST&");
            signatureBase.Append(Uri.EscapeDataString(url));
            signatureBase.Append("&");

            var encodedParams = new List<string>();
            foreach (var parameter in parameters)
            {
                encodedParams.Add($"{Uri.EscapeDataString(parameter.Key)}={Uri.EscapeDataString(parameter.Value)}");
            }
            signatureBase.Append(Uri.EscapeDataString(string.Join("&", encodedParams)));

            var signatureKey = $"{Uri.EscapeDataString(_consumerSecret)}&";
            using (var hasher = new HMACSHA1(Encoding.ASCII.GetBytes(signatureKey)))
            {
                var hash = hasher.ComputeHash(Encoding.ASCII.GetBytes(signatureBase.ToString()));
                return Convert.ToBase64String(hash);
            }
        }

        public async Task<JObject> GetFoodCategoriesAsync()
        {
            var nonce = GenerateNonce();
            var timestamp = GenerateTimeStamp();

            var parameters = new SortedDictionary<string, string>
            {
                { "method", "food_categories.get" }, // Убедитесь, что это правильный метод
                { "oauth_consumer_key", _consumerKey },
                { "oauth_nonce", nonce },
                { "oauth_signature_method", "HMAC-SHA1" },
                { "oauth_timestamp", timestamp },
                { "oauth_version", "1.0" },
                { "format", "json" }
            };

            var signature = GenerateSignature(_apiUrl, nonce, timestamp, parameters);
            parameters.Add("oauth_signature", signature);

            var content = new FormUrlEncodedContent(parameters);

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(_apiUrl, content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Error retrieving food categories: " + responseString);
                }

                return JObject.Parse(responseString);
            }
        }
    }
}
