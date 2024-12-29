using DKCrm.Shared.Models.CompanyModels;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Requests;
using System.Net.Http;

namespace DKCrm.Client.Services.CompanyServices
{
    public interface ICompanyCommentsManager
    {
        Task<List<CompanyComment>> GetAllForCompanyAsync(Guid companyId);
        Task RemoveRangeAsync(IEnumerable<Guid> listId);
        Task SaveCommentAsync(CompanyComment comment);
        Task<int> SetLogUsersVisitAsync(LogUsersVisitToCompanyComments log);
        Task<LogUsersVisitToCompanyComments?> GetLogUsersVisitAsync(Guid companyId);
        Task<List<CompanyComment>> GetWarningCommentsAsync(GetWarningCommentsToCompanyRequest request);
    }
}
