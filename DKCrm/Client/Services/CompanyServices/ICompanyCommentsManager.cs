using DKCrm.Shared.Models.CompanyModels;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Requests;
using System.Net.Http;
using DKCrm.Shared.Requests.OrderService;

namespace DKCrm.Client.Services.CompanyServices
{
    public interface ICompanyCommentsManager
    {
        Task<GetCommentsForPaginationResponse<CompanyComment>> GetAllForCompanyAsync(
            GetCommentsForPaginationRequest request);
        Task RemoveRangeAsync(IEnumerable<Guid> listId);
        Task SaveCommentAsync(CompanyComment comment);
        Task<int> SetLogUsersVisitAsync(LogUsersVisitToCompanyComments log);
        Task<LogUsersVisitToCompanyComments?> GetLogUsersVisitAsync(Guid companyId);
        Task<GetCommentsForPaginationResponse<CompanyComment>> GetWarningCommentsAsync(GetWarningCommentsToCompanyRequest request);
    }
}
