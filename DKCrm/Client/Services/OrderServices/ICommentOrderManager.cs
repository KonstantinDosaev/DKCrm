﻿using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models;
using DKCrm.Shared.Requests;

namespace DKCrm.Client.Services.OrderServices
{
    public interface ICommentOrderManager
    {
        Task<List<CommentOrder>> GetAllForOrderAsync(Guid orderId);
        Task<SortPagedResponse<CommentOrder>> GetBySortFilterPaginationAsync(SortPagedRequest<FilterOrderCommentTuple> request);
        Task RemoveRangeAsync(IEnumerable<Guid> listId);
        Task SaveCommentAsync(CommentOrder comment);
        Task<int> SetLogUsersVisitAsync(LogUsersVisitToOrderComments log);
        Task<LogUsersVisitToOrderComments?> GetLogUsersVisitAsync(Guid orderId);
        Task<List<CommentOrder>> GetWarningCommentsAsync(GetWarningCommentsToOrderRequest request);
    }
}
