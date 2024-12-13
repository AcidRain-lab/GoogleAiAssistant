using System;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace BLL
{
    public class EdamamService
    {
        private readonly string _appId = "bd70a8d5";
        private readonly string _appKey = "03ce4ba048d8cf221b169c203e23726f";
        private readonly string _apiUrl = "https://api.edamam.com/api/food-database/v2";

        public EdamamService()
        {
        }

        public async Task<JObject> SearchFoodAsync(string query)
        {
            var client = new RestClient($"{_apiUrl}/parser");
            var request = new RestRequest("", Method.Get);
            request.AddParameter("ingr", query);
            request.AddParameter("app_id", _appId);
            request.AddParameter("app_key", _appKey);

            var response = await client.ExecuteAsync(request);
            if (!response.IsSuccessful)
            {
                throw new Exception("Error retrieving food data: " + response.Content);
            }

            return JObject.Parse(response.Content);
        }

        public async Task<JObject> GetFoodDetailsAsync(object foodDetailsRequest)
        {
            var client = new RestClient($"{_apiUrl}/nutrients");
            var request = new RestRequest("", Method.Post);
            request.AddParameter("app_id", _appId);
            request.AddParameter("app_key", _appKey);
            request.AddJsonBody(foodDetailsRequest);

            var response = await client.ExecuteAsync(request);
            if (!response.IsSuccessful)
            {
                throw new Exception("Error retrieving food details: " + response.Content);
            }

            return JObject.Parse(response.Content);
        }


        public async Task<JObject> GetFoodCategoriesAsync()
        {
            var client = new RestClient($"{_apiUrl}/categories");
            var request = new RestRequest("", Method.Get);
            request.AddParameter("app_id", _appId);
            request.AddParameter("app_key", _appKey);

            var response = await client.ExecuteAsync(request);
            if (!response.IsSuccessful)
            {
                throw new Exception("Error retrieving food categories: " + response.Content);
            }

            return JObject.Parse(response.Content);
        }

        public async Task<JObject> GetSubCategoriesAsync(string categoryId)
        {
            var client = new RestClient($"{_apiUrl}/subcategories");
            var request = new RestRequest("", Method.Get);
            request.AddParameter("category_id", categoryId);
            request.AddParameter("app_id", _appId);
            request.AddParameter("app_key", _appKey);

            var response = await client.ExecuteAsync(request);
            if (!response.IsSuccessful)
            {
                throw new Exception("Error retrieving subcategories: " + response.Content);
            }

            return JObject.Parse(response.Content);
        }
    }
}
