﻿using DKCrm.Shared.Models.OrderModels;
using System.Net.Http.Json;

namespace DKCrm.Client.Services.OrderServices
{
    public class ExportedOrderManager : IExportedOrderManager
    {
        private readonly HttpClient _httpClient;

        public ExportedOrderManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<ExportedOrder>> GetAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ExportedOrder>>("api/ExportedOrder/Get") ?? throw new InvalidOperationException();
        }

        public async Task<ExportedOrder> GetDetailsAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<ExportedOrder>($"api/ExportedOrder/Get/{id}") ?? throw new InvalidOperationException();
        }

        public async Task<bool> UpdateAsync(ExportedOrder item)
        {
            var result = await _httpClient.PutAsJsonAsync("api/ExportedOrder/Put", item);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> AddAsync(ExportedOrder item)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/ExportedOrder/Post", item);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveRangeAsync(IEnumerable<ExportedOrder> items)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/ExportedOrder/DeleteRange/removerange", items);
            return result.IsSuccessStatusCode;
        }
        public async Task<bool> RemoveAsync(Guid id)
        {
            var result = await _httpClient.DeleteAsync($"api/ExportedOrder/Delete/{id}");
            return result.IsSuccessStatusCode;
        }
    }
}