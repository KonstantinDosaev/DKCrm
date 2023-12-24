using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Client.Services.OrderServices
{
    public interface IExportedProductManager
    {
        Task<List<ExportedProduct>> GetAsync();
        Task<List<ExportedProduct>> GetNotEquippedAsync();
        Task<ExportedProduct> GetDetailsAsync(Guid id);
        Task<bool> UpdateAsync(ExportedProduct item);
        Task<bool> AddAsync(ExportedProduct item);
        Task<bool> RemoveRangeAsync(IEnumerable<ExportedProduct> items);
        Task<bool> RemoveAsync(Guid id);
    }
}
