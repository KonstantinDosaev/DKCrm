using DKCrm.Shared.Constants;
using DKCrm.Shared.Models.CompanyModels;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DKCrm.Server.Interfaces
{
    public interface ICompanyCommentsService
    {
        Task<IEnumerable<CompanyComment>> GetAllCommentsFromCompanyAsync(Guid companyId, ClaimsPrincipal user);
        Task<int> SaveCommentAsync(CompanyComment comment, ClaimsPrincipal user);
        Task<SortPagedResponse<CommentOrder>> GetBySortPagedSearchAsync(SortPagedRequest<FilterOrderCommentTuple> request);
        Task<int> RemoveRangeAsync(IEnumerable<Guid> listId, ClaimsPrincipal user);
    }
}
