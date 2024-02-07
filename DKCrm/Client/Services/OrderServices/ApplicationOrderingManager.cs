using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace DKCrm.Client.Services.OrderServices
{
    public class ApplicationOrderingManager : IApplicationOrderingManager
    {
        private readonly HttpClient _httpClient;

        public ApplicationOrderingManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<ApplicationOrderingProducts>> GetAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ApplicationOrderingProducts>>("api/ApplicationOrdering/Get") ?? throw new InvalidOperationException();
        }

        public async Task<ApplicationOrderingProducts> GetDetailsAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<ApplicationOrderingProducts>($"api/ApplicationOrdering/Get/{id}") ?? throw new InvalidOperationException();
        }
        public async Task<SortPagedResponse<ApplicationOrderingProducts>> GetBySortFilterPaginationAsync(SortPagedRequest<FilterApplicationOrderTuple> request)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/ApplicationOrdering/GetBySortPagedSearchChapter", request) ?? throw new InvalidOperationException();
            return await response.Content.ReadFromJsonAsync<SortPagedResponse<ApplicationOrderingProducts>>() ?? throw new InvalidOperationException();

        }
        public async Task<bool> UpdateAsync(ApplicationOrderingProducts item)
        {
            var result = await _httpClient.PutAsJsonAsync("api/ApplicationOrdering/Put", item, new JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
                PropertyNamingPolicy = null
            });
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> AddAsync(ApplicationOrderingProducts item)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/ApplicationOrdering/Post", item, new JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
                PropertyNamingPolicy = null
            });
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveRangeAsync(IEnumerable<ApplicationOrderingProducts> items)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/ApplicationOrdering/DeleteRange/removerange", items);
            return result.IsSuccessStatusCode;
        }
        public async Task<bool> RemoveAsync(Guid id)
        {
            var result = await _httpClient.DeleteAsync($"api/ApplicationOrdering/Delete/{id}");
            return result.IsSuccessStatusCode;
        }
    }
}
