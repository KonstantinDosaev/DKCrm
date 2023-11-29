using DKCrm.Shared.Models.CompanyModels;

namespace DKCrm.Client.Services.CompanyServices
{
    public interface ICompanyTypeManager
    {
        Task<List<CompanyType>> GetAsync();
        Task<CompanyType> GetDetailsAsync(Guid id);
        Task<bool> UpdateAsync(CompanyType companyType);
        Task<bool> AddAsync(CompanyType companyType);
        Task<bool> RemoveRangeAsync(IEnumerable<CompanyType> companyTypes);
        Task<bool> RemoveAsync(Guid id);
    }
}
