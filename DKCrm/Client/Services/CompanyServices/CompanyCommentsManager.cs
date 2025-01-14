using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models;
using System.Net.Http.Json;
using DKCrm.Shared.Models.CompanyModels;
using DKCrm.Shared.Requests;
using DKCrm.Shared.Requests.OrderService;

namespace DKCrm.Client.Services.CompanyServices
{
    public class CompanyCommentsManager : ICompanyCommentsManager
    {

        private readonly HttpClient _httpClient;

        public CompanyCommentsManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GetCommentsForPaginationResponse<CompanyComment>> GetAllForCompanyAsync(GetCommentsForPaginationRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/CompanyComments/GetAllForCompany", request) ?? throw new InvalidOperationException();
            return await response.Content.ReadFromJsonAsync<GetCommentsForPaginationResponse<CompanyComment>>() ?? throw new InvalidOperationException();
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
        public async Task<int> SetLogUsersVisitAsync(LogUsersVisitToCompanyComments log)
        {
            var response = await _httpClient.PostAsJsonAsync("api/CompanyComments/SetLogUsersVisit", log);
            return await response.Content.ReadFromJsonAsync<int>();
        }
        public async Task<LogUsersVisitToCompanyComments?> GetLogUsersVisitAsync(Guid companyId)
        {
            return await _httpClient.GetFromJsonAsync<LogUsersVisitToCompanyComments?>
                ($"api/CompanyComments/GetLogUsersVisit/{companyId}");
        }
        public async Task<GetCommentsForPaginationResponse<CompanyComment>> GetWarningCommentsAsync(GetWarningCommentsToCompanyRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/CompanyComments/GetWarningComments", request) ?? throw new InvalidOperationException();
            return await response.Content.ReadFromJsonAsync<GetCommentsForPaginationResponse<CompanyComment>>() ?? throw new InvalidOperationException();

        }
    }
}
