using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DAL.AbsModels;
using DAL.Repositories;

namespace BLL.Services.Implementations
{
    public class AbcSupplyService : IAbcSupplyService
    {
        private readonly HttpClient _httpClient;

        public AbcSupplyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            // Убедитесь, что базовый адрес настроен либо здесь, либо при конфигурации HttpClient
            //_httpClient.BaseAddress = new Uri("https://partners.abcsupply.com/");
        }

        public async Task<OrderResponse> PlaceOrderAsync(OrderRequest orderRequest)
        {
            HttpResponseMessage response = null;
            try
            {
                response = await _httpClient.PostAsJsonAsync("/api/order/v2/orders", orderRequest);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<OrderResponse>();
            }
            catch (HttpRequestException e) when (response != null && !response.IsSuccessStatusCode)
            {

                throw new ApplicationException($"Error calling API: {e.Message}", e);
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(string confirmationNumber)
        {
            HttpResponseMessage response = null;
            try
            {
                response = await _httpClient.GetAsync($"/api/order/v2/orders?confirmationNumber={confirmationNumber}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<IEnumerable<Order>>();
            }
            catch (HttpRequestException e) when (response != null && !response.IsSuccessStatusCode)
            {

                throw new ApplicationException($"Error calling API: {e.Message}", e);
            }
        }

    }
}
