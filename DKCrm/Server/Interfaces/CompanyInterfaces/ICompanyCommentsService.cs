using DKCrm.Shared.Constants;
using DKCrm.Shared.Models.CompanyModels;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DKCrm.Shared.Requests;

namespace DKCrm.Server.Interfaces.CompanyInterfaces
{
    public interface ICompanyCommentsService
    {
        Task<IEnumerable<CompanyComment>> GetAllCommentsFromCompanyAsync(Guid companyId, ClaimsPrincipal user);
        Task<int> SaveCommentAsync(CompanyComment comment, ClaimsPrincipal user);
        Task<SortPagedResponse<CommentOrder>> GetBySortPagedSearchAsync(SortPagedRequest<FilterOrderCommentTuple> request);
        Task<int> RemoveRangeAsync(IEnumerable<Guid> listId, ClaimsPrincipal user);
        Task<int> SetLogUserVisit(LogUsersVisitToCompanyComments newLog, ClaimsPrincipal user);
        Task<LogUsersVisitToCompanyComments> GetLogUserVisitAsync(Guid companyId, ClaimsPrincipal user);
        Task<IEnumerable<CompanyComment>> GetWarningCommentsAsync(GetWarningCommentsToCompanyRequest request,
            ClaimsPrincipal user);
    }
}
