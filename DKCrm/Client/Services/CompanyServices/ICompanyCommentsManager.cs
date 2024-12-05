using DKCrm.Shared.Models.CompanyModels;
using System.Net.Http;

namespace DKCrm.Client.Services.CompanyServices
{
    public interface ICompanyCommentsManager
    {
        Task<List<CompanyComment>> GetAllForCompanyAsync(Guid companyId);
        Task RemoveRangeAsync(IEnumerable<Guid> listId);
        Task SaveCommentAsync(CompanyComment comment);
    }
}
