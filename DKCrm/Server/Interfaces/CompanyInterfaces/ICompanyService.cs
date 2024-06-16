using DKCrm.Shared.Models.CompanyModels;

namespace DKCrm.Server.Interfaces.CompanyInterfaces
{
    public interface ICompanyService
    {
     Task<IEnumerable<Company>> GetAsync();
     Task<IEnumerable<Company>> GetCompaniesByTypeAsync(string companyType);
     Task<Company> GetAsync(Guid id);
     Task<Guid> PostAsync(Company company);
     Task<Guid> PutAsync(Company company);
     Task<int> PutRangeAsync(IEnumerable<Company> companies);
     Task<int> DeleteAsync(Guid id);
     Task<int> DeleteRangeAsync(IEnumerable<Company> companies);
     Task<int> DeleteBankDetailsAsync(Guid id);
     Task<int> UpdateTagsCompanyAsync(TagsRequest tagRequest);
     Task<int> AddEmployeeAsync(Company company);
    }
}
