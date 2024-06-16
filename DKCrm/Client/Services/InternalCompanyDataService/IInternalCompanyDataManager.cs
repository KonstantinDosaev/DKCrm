using DKCrm.Shared.Models;

namespace DKCrm.Client.Services.InternalCompanyDataService
{
    public interface IInternalCompanyDataManager
    {
        Task<InternalCompanyData> GetAsync();
        Task<bool> UpdateAsync(InternalCompanyData storage);
        Task<bool> AddAsync(InternalCompanyData storage);
    }
}
