using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models;
using System.Net.Http.Json;
using DKCrm.Shared.Models.CompanyModels;

namespace DKCrm.Client.Services.CompanyServices
{
    public class CompanyCommentsManager : ICompanyCommentsManager
    {

        private readonly HttpClient _httpClient;

        public CompanyCommentsManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CompanyComment>> GetAllForCompanyAsync(Guid companyId)
        {
            return await _httpClient.GetFromJsonAsync<List<CompanyComment>>($"api/CompanyComments/GetAllForCompany/{companyId}") ?? throw new InvalidOperationException();
        }
       // public async Task<SortPagedResponse<CommentOrder>> GetBySortFilterPaginationAsync(SortPagedRequest<FilterOrderCommentTuple> request)
      //  {
            //var response = await _httpClient.PostAsJsonAsync($"api/OrderComment/GetBySortPagedSearchChapter", request) ?? throw new InvalidOperationException();
           // return await response.Content.ReadFromJsonAsync<SortPagedResponse<CompanyComment>>() ?? throw new InvalidOperationException();

       // }
        public async Task RemoveRangeAsync(IEnumerable<Guid> listId)
        {
            await _httpClient.PostAsJsonAsync("api/CompanyComments/RemoveRange", listId);
        }
        public async Task SaveCommentAsync(CompanyComment comment)
        {
            await _httpClient.PostAsJsonAsync("api/CompanyComments/SaveComment", comment);
        }
    }
}
