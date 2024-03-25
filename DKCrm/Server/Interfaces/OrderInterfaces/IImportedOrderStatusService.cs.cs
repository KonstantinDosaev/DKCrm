using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Server.Interfaces.OrderInterfaces
{
    public interface IImportedOrderStatusService
    {
        Task<IEnumerable<ImportedOrderStatus>> GetAsync();
        Task<ImportedOrderStatus> GetDetailAsync(Guid id);
        Task<Guid> PostAsync(ImportedOrderStatus importedOrderStatus);
        Task<Guid> PutAsync(ImportedOrderStatus importedOrderStatus);
        Task<int> PutRangeAsync(IEnumerable<ImportedOrderStatus> importedOrderStatus);
        Task<int> DeleteAsync(Guid id);
        Task<int> DeleteRangeAsync(IEnumerable<ImportedOrderStatus> status);
        bool Initialize();
    }
}
