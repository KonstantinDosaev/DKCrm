using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models;

namespace DKCrm.Client.Services.OrderServices
{
    public interface ICommentOrderManager
    {
        Task<List<CommentOrder>> GetAllForOrderAsync(Guid orderId);
        Task<SortPagedResponse<CommentOrder>> GetBySortFilterPaginationAsync(SortPagedRequest<FilterOrderCommentTuple> request);
        Task RemoveRangeAsync(IEnumerable<Guid> listId);
        Task SaveCommentAsync(CommentOrder comment);
    }
}
