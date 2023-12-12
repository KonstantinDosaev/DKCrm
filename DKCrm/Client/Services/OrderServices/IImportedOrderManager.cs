using DKCrm.Shared.Models.CompanyModels;
using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Client.Services.OrderServices
{
    public interface IImportedOrderManager
    {
        Task<List<ImportedOrder>> GetAsync();
        Task<ImportedOrder> GetDetailsAsync(Guid id);
        Task<bool> UpdateAsync(ImportedOrder item);
        Task<bool> AddAsync(ImportedOrder item);
        Task<bool> RemoveRangeAsync(IEnumerable<ImportedOrder> items);
        Task<bool> RemoveAsync(Guid id);
    }
}
