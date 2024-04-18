using DKCrm.Client.Constants;
using DKCrm.Shared.Models.CompanyModels;
using System.Net.Http.Json;

namespace DKCrm.Client.Services.CompanyServices
{
    public class CompanyTagsManager : ICompanyTagsManager
    {
        private readonly HttpClient _httpClient;

        public CompanyTagsManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<TagsCompany>> GetAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<TagsCompany>>("api/companyTags") ?? throw new InvalidOperationException();
        }

        public async Task<TagsCompany> GetDetailsAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<TagsCompany>($"api/companyTags/{id}") ?? throw new InvalidOperationException();
        }

        public async Task<bool> UpdateAsync(TagsCompany tagsCompany)
        {
            var result = await _httpClient.PutAsJsonAsync("api/companyTags", tagsCompany, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> AddAsync(TagsCompany tagsCompany)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/companyTags", tagsCompany, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveRangeAsync(IEnumerable<TagsCompany> tagsCompanies)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/companyTags/removerange", tagsCompanies, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }
        public async Task<bool> RemoveAsync(Guid id)
        {
            var result = await _httpClient.DeleteAsync($"api/companyTags/{id}");
            return result.IsSuccessStatusCode;
        }
    }
}
