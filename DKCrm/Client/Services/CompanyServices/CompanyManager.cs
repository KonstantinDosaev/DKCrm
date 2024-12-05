using System.Net.Http.Json;
using DKCrm.Shared.Models.CompanyModels;
using DKCrm.Client.Constants;

namespace DKCrm.Client.Services.CompanyServices
{
    public class CompanyManager: ICompanyManager
    {
        private readonly HttpClient _httpClient;

        public CompanyManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<Company>> GetAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Company>>("api/company") ?? throw new InvalidOperationException();
        }
        public async Task<List<Company>> GetCompaniesByTypeAsync(string type)
        {
            return await _httpClient.GetFromJsonAsync<List<Company>>($"api/Company/byType/{type}") ?? throw new InvalidOperationException();
        }
        public async Task<Company> GetDetailsAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<Company>($"api/Company/{id}") ?? throw new InvalidOperationException();
        }

        public async Task<bool> UpdateAsync(Company company)
        {
            var result = await _httpClient.PutAsJsonAsync($"api/Company/company/{company}", company, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> AddAsync(Company company)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/Company/{company}", company, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveRangeAsync(IEnumerable<Company> companies)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/company/removerange", companies);
            return result.IsSuccessStatusCode;
        }
        public async Task<bool> RemoveAsync(Guid id)
        {
            var result = await _httpClient.DeleteAsync($"api/company/{id}");
            return result.IsSuccessStatusCode;
        }       
        public async Task<bool> RemoveBankDetailsAsync(Guid id)
        {
            var result = await _httpClient.DeleteAsync($"api/company/bank-details/{id}");
            return result.IsSuccessStatusCode;
        }
        public async Task<bool> UpdateTagsAsync(TagsRequest tagRequest)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/Company/updateTags/{tagRequest}", tagRequest, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }
        public async Task<bool> AddBankAsync(BankDetails bankDetails)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/Company/addbank", bankDetails, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }

    }
}
