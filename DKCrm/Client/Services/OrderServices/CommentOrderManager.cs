using System.Net.Http.Json;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models;
using DKCrm.Shared.Models.CompanyModels;
using DKCrm.Shared.Requests;
using System.Security.Claims;

namespace DKCrm.Client.Services.OrderServices
{
    public class CommentOrderManager  : ICommentOrderManager
    {
        private readonly HttpClient _httpClient;

        public CommentOrderManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CommentOrder>> GetAllForOrderAsync(Guid orderId)
        {
            return await _httpClient.GetFromJsonAsync<List<CommentOrder>>($"api/OrderComment/GetAllForOrder/{orderId}") ?? throw new InvalidOperationException();
        }
        public async Task<SortPagedResponse<CommentOrder>> GetBySortFilterPaginationAsync(SortPagedRequest<FilterOrderCommentTuple> request)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/OrderComment/GetBySortPagedSearchChapter", request) ?? throw new InvalidOperationException();
            return await response.Content.ReadFromJsonAsync<SortPagedResponse<CommentOrder>>() ?? throw new InvalidOperationException();

        }
        public async Task RemoveRangeAsync(IEnumerable<Guid> listId)
        {
            await _httpClient.PostAsJsonAsync("api/OrderComment/RemoveRange", listId);
        }
        public async Task SaveCommentAsync(CommentOrder comment)
        {
            await _httpClient.PostAsJsonAsync("api/OrderComment/SaveComment", comment);
        }
        public async Task<int> SetLogUsersVisitAsync(LogUsersVisitToOrderComments log)
        {
            var response = await _httpClient.PostAsJsonAsync("api/OrderComment/SetLogUsersVisit", log);
            return await response.Content.ReadFromJsonAsync<int>();
        }
        public async Task<LogUsersVisitToOrderComments?> GetLogUsersVisitAsync(Guid orderId)
        {
            return await _httpClient.GetFromJsonAsync<LogUsersVisitToOrderComments>
                ($"api/OrderComment/GetLogUsersVisit/{orderId}");
        }   
        public async Task<List<CommentOrder>> GetWarningCommentsAsync(GetWarningCommentsToOrderRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/OrderComment/GetWarningComments", request) ?? throw new InvalidOperationException();
            return await response.Content.ReadFromJsonAsync<List<CommentOrder>>() ?? throw new InvalidOperationException();

        }
    }
}
