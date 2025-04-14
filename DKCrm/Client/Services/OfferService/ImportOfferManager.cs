using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models;
using System.Net.Http.Json;
using DKCrm.Shared.Models.OfferModels;
using DKCrm.Client.Constants;

namespace DKCrm.Client.Services.OfferService
{
    public class ImportOfferManager : IImportOfferManager
    {
        private readonly HttpClient _httpClient;

        public ImportOfferManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ImportOffer> GetDetailsAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<ImportOffer>($"api/ImportOffer/Get/{id}") ?? throw new InvalidOperationException();
        }
        public async Task<SortPagedResponse<ImportOffer>> GetBySortFilterPaginationAsync(SortPagedRequest<FilterOfferTuple> request, CancellationToken token)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/ImportOffer/GetBySortPagedSearchChapter", request, cancellationToken: token) ?? throw new InvalidOperationException();
            return await response.Content.ReadFromJsonAsync<SortPagedResponse<ImportOffer>>(cancellationToken: token) ?? throw new InvalidOperationException();
        }
        public async Task<bool> AddAsync(ImportOffer item)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/ImportOffer/Post", item, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }
        public async Task<bool> UpdateAsync(ImportOffer item)
        {
            var result = await _httpClient.PutAsJsonAsync($"api/ImportOffer/Update", item, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }
        public async Task<bool> UpdatePriceAsync(PriceForImportOffer item)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/ImportOffer/UpdatePrice", item, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }
        public async Task<bool> AddOfferToExportOrderAsync(ExportProductPriceImportOffer link)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/ImportOffer/AddOfferToExportOrder", link, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }
    }
}
