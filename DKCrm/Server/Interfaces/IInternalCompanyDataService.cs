using DKCrm.Shared.Models;

namespace DKCrm.Server.Interfaces
{
    public interface IInternalCompanyDataService
    {
        Task<InternalCompanyData> GetAsync();
        Task<Guid> PostAsync(InternalCompanyData data);
        Task<Guid> PutAsync(InternalCompanyData data);
    }
}
