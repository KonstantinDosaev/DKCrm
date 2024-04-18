using DKCrm.Client.Constants;
using DKCrm.Shared.Models.OrderModels;
using System.Net.Http.Json;

namespace DKCrm.Client.Services.OrderServices
{
    public class ExportedOrderStatusManager : IExportedOrderStatusManager
    {
        private readonly HttpClient _httpClient;

        public ExportedOrderStatusManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<ExportedOrderStatus>> GetAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ExportedOrderStatus>>("api/ExportedOrderStatus/Get") ?? throw new InvalidOperationException();
        }

        public async Task<ExportedOrderStatus> GetDetailsAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<ExportedOrderStatus>($"api/ExportedOrderStatus/Get/{id}") ?? throw new InvalidOperationException();
        }

        public async Task<bool> UpdateAsync(ExportedOrderStatus item)
        {
            var result = await _httpClient.PutAsJsonAsync("api/ExportedOrderStatus/Put", item, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> AddAsync(ExportedOrderStatus item)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/ExportedOrderStatus/Post", item, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveRangeAsync(IEnumerable<ExportedOrderStatus> items)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/ExportedOrderStatus/DeleteRange/removerange", items);
            return result.IsSuccessStatusCode;
        }
        public async Task<bool> RemoveAsync(Guid id)
        {
            var result = await _httpClient.DeleteAsync($"api/ExportedOrderStatus/Delete/{id}");
            return result.IsSuccessStatusCode;
        }
    }
}
