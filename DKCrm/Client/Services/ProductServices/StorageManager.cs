using DKCrm.Shared.Models.CompanyModels;
using DKCrm.Shared.Models.Products;
using System.Net.Http.Json;
using DKCrm.Shared.Models.OrderModels;
using System.Text.Json;
using DKCrm.Client.Constants;

namespace DKCrm.Client.Services.ProductServices
{
    public class StorageManager: IStorageManager
    {
        private readonly HttpClient _httpClient;

        public StorageManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<Storage>> GetAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Storage>>("api/Storage") ?? throw new InvalidOperationException();
        }

        public async Task<Storage> GetDetailsAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<Storage>($"api/Storage/{id}") ?? throw new InvalidOperationException();
        }

        public async Task<List<ProductsInStorage>> GetProductInStorageListAsync(Guid productId)
        {
            return await _httpClient.GetFromJsonAsync<List<ProductsInStorage>>($"api/Storage/productInStorageList/{productId}") ?? throw new InvalidOperationException();
        }

        public async Task<bool> UpdateAsync(Storage storage)
        {
            var result = await _httpClient.PutAsJsonAsync("api/Storage", storage, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> AddAsync(Storage storage)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/Storage", storage, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var result = await _httpClient.DeleteAsync($"api/Storage/{id}");
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> ReserveAProductAsync(SoldFromStorage soldFromStorage)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/Storage/ReserveAProduct", soldFromStorage, new JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
                PropertyNamingPolicy = null
            });
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> CancelReserveAProductAsync(SoldFromStorage soldFromStorage)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/Storage/CancelReserveAProduct", soldFromStorage, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }
    }
}
