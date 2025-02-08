using System.Security.Claims;
using DKCrm.Shared.Models;
using DKCrm.Shared.Models.CompanyModels;

namespace DKCrm.Server.Interfaces.CompanyInterfaces
{
    public interface ICompanyService
    {
     Task<IEnumerable<Company>> GetAsync(ClaimsPrincipal user);
     Task<IEnumerable<Company>> GetCompaniesByTypeAsync(string companyType,ClaimsPrincipal user);
     Task<Company> GetAsync(Guid id, ClaimsPrincipal user);

     Task<SortPagedResponse<Company>> GetBySortPagedSearchChapterAsync(
         SortPagedRequest<FilterCompanyTuple> request, ClaimsPrincipal user);
     Task<Guid> PostAsync(Company company);
     Task<Guid> PutAsync(Company company);
     Task<int> PutRangeAsync(IEnumerable<Company> companies);
     Task<int> DeleteAsync(Guid id);
     Task<int> DeleteRangeAsync(IEnumerable<Company> companies);
     Task<int> DeleteBankDetailsAsync(Guid id);
     Task<int> UpdateTagsCompanyAsync(TagsRequest tagRequest);
     Task<int> AddEmployeeAsync(Company company);
     Task<Guid> AddBankDetails(BankDetails bankDetails);
    }
}
