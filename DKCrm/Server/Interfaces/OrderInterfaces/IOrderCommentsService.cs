using DKCrm.Shared.Models;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Requests;
using System.Security.Claims;
using DKCrm.Shared.Requests.OrderService;

namespace DKCrm.Server.Interfaces.OrderInterfaces
{
    public interface IOrderCommentsService
    {
        Task<GetCommentsForPaginationResponse<CommentOrder>> GetAllCommentsFromOrderAsync(GetCommentsForPaginationRequest request, ClaimsPrincipal user);
        Task<SortPagedResponse<CommentOrder>> GetBySortPagedSearchAsync(SortPagedRequest<FilterOrderCommentTuple> request);
        Task<int> SaveCommentAsync(CommentOrder comment, ClaimsPrincipal user);
        Task<int> RemoveRangeAsync(IEnumerable<Guid> listId, ClaimsPrincipal user);
        Task<int> SetLogUserVisit(LogUsersVisitToOrderComments newLog, ClaimsPrincipal user);
        Task<LogUsersVisitToOrderComments> GetLogUserVisitAsync(Guid orderId, ClaimsPrincipal user);

        Task<IEnumerable<CommentOrder>> GetWarningCommentsAsync(GetWarningCommentsToOrderRequest request,
            ClaimsPrincipal user);
    }
}
