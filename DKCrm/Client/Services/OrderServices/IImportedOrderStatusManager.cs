using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Client.Services.OrderServices
{
    public interface IImportedOrderStatusManager
    {
        Task<List<ImportedOrderStatus>> GetAsync();
        Task<ImportedOrderStatus> GetDetailsAsync(Guid id);
        Task<bool> UpdateAsync(ImportedOrderStatus item);
        Task<bool> AddAsync(ImportedOrderStatus item);
        Task<bool> RemoveRangeAsync(IEnumerable<ImportedOrderStatus> items);
        Task<bool> RemoveAsync(Guid id);
    }
}
