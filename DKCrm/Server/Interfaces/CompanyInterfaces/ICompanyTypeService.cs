using DKCrm.Shared.Models.CompanyModels;

namespace DKCrm.Server.Interfaces.CompanyInterfaces
{
    public interface ICompanyTypeService
    {
        Task<IEnumerable<CompanyType>> GetAsync();
        Task<CompanyType> GetAsync(Guid id);
        Task<Guid> PostAsync(CompanyType company);
        Task<Guid> PutAsync(CompanyType company);
        Task<int> PutRangeAsync(IEnumerable<CompanyType> companies);
        Task<int> DeleteAsync(Guid id);
        Task<int> DeleteRangeAsync(IEnumerable<CompanyType> companies);
    }
}
