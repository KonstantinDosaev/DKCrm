using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Server.Interfaces.OrderInterfaces
{
    public interface IExportedOrderStatusServices
    {
       Task<IEnumerable<ExportedOrderStatus>> GetAsync();
       Task<ExportedOrderStatus> GetDetailAsync(Guid id);
        Task<Guid> PostAsync(ExportedOrderStatus exportedOrderStatus);
        Task<Guid> PutAsync(ExportedOrderStatus exportedOrderStatus);
        Task<int> PutRangeAsync(IEnumerable<ExportedOrderStatus> exportedOrderStatus);
        Task<int> DeleteAsync(Guid id);
        Task<int> DeleteRangeAsync(IEnumerable<ExportedOrderStatus> status);
        bool Initialize();
    }
}
