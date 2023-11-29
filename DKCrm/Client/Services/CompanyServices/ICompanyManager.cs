using DKCrm.Shared.Models.CompanyModels;
using DKCrm.Shared.Models.Products;

namespace DKCrm.Client.Services.CompanyServices
{
    public interface ICompanyManager
    {
        Task<List<Company>> GetAsync();
        Task<Company> GetDetailsAsync(Guid id);
        Task<bool> UpdateAsync(Company company);
        Task<bool> AddAsync(Company company);
        Task<bool> RemoveRangeAsync(IEnumerable<Company> companies);
        Task<bool> RemoveAsync(Guid id);
        Task<bool> RemoveBankDetailsAsync(Guid id);
        Task<bool> UpdateTagsAsync(TagsRequest tagsRequest);
    }
}
