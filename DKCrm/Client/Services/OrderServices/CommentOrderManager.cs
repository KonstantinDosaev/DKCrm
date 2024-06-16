using System.Net.Http.Json;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models;

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
    }
}
