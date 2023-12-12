using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Client.Services.OrderServices
{
    public interface IExportedOrderStatusManager
    {
        Task<List<ExportedOrderStatus>> GetAsync();
        Task<ExportedOrderStatus> GetDetailsAsync(Guid id);
        Task<bool> UpdateAsync(ExportedOrderStatus item);
        Task<bool> AddAsync(ExportedOrderStatus item);
        Task<bool> RemoveRangeAsync(IEnumerable<ExportedOrderStatus> items);
        Task<bool> RemoveAsync(Guid id);
    }
}
