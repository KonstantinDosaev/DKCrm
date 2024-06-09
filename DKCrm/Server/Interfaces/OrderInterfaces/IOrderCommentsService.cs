using DKCrm.Shared.Models;
using DKCrm.Shared.Models.OrderModels;
using System.Security.Claims;

namespace DKCrm.Server.Interfaces.OrderInterfaces
{
    public interface IOrderCommentsService
    {
        Task<IEnumerable<CommentOrder>> GetAllCommentsFromOrderAsync(Guid orderId, ClaimsPrincipal user);
        Task<SortPagedResponse<CommentOrder>> GetBySortPagedSearchAsync(SortPagedRequest<FilterOrderCommentTuple> request);
        Task<int> SaveCommentAsync(CommentOrder comment, ClaimsPrincipal user);
        Task<int> RemoveRangeAsync(IEnumerable<Guid> listId, ClaimsPrincipal user);
    }
}
