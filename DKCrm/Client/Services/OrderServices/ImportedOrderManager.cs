﻿using System.Net.Http.Json;
using DKCrm.Client.Constants;
using DKCrm.Shared.Models;
using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Client.Services.OrderServices
{
    public class ImportedOrderManager : IImportedOrderManager
    {
        private readonly HttpClient _httpClient;

        public ImportedOrderManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<ImportedOrder>> GetAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ImportedOrder>>("api/ImportedOrder/Get") ?? throw new InvalidOperationException();
        }

        public async Task<ImportedOrder> GetDetailsAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<ImportedOrder>($"api/ImportedOrder/Get/{id}") ?? throw new InvalidOperationException();
        }
        public async Task<SortPagedResponse<ImportedOrder>> GetBySortFilterPaginationAsync(SortPagedRequest<FilterOrderTuple> request)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/ImportedOrder/GetBySortPagedSearchChapter", request) ?? throw new InvalidOperationException();
            return await response.Content.ReadFromJsonAsync<SortPagedResponse<ImportedOrder>>() ?? throw new InvalidOperationException();

        }
        public async Task<bool> UpdateAsync(ImportedOrder item)
        {
            var result = await _httpClient.PutAsJsonAsync("api/ImportedOrder/Put", item, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }

        public async Task<Guid> AddAsync(ImportedOrder item)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/ImportedOrder/Post", item, JsonOptions.JsonIgnore);
            return await result.Content.ReadFromJsonAsync<Guid>();
        }

        public async Task<bool> RemoveRangeAsync(IEnumerable<ImportedOrder> items)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/ImportedOrder/DeleteRange/removerange", items);
            return result.IsSuccessStatusCode;
        }
        public async Task<bool> RemoveAsync(Guid id)
        {
            var result = await _httpClient.DeleteAsync($"api/ImportedOrder/Delete/{id}");
            return result.IsSuccessStatusCode;
        }
        public async Task<bool> AddStatusToOrderAsync(ImportedOrderStatusImportedOrder statusOrder)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/ImportedOrder/AddStatusToOrder/add-status", statusOrder, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }
    }
}
