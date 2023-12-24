using DKCrm.Shared.Models;
using DKCrm.Shared.Models.Products;

namespace DKCrm.Client.Services.InternalCompanyDataService
{
    public interface IInternalCompanyDataManager
    {
        Task<InternalCompanyData> GetAsync();
        Task<bool> UpdateAsync(InternalCompanyData storage);
        Task<bool> AddAsync(InternalCompanyData storage);
    }
}
