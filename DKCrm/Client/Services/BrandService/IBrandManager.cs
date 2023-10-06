using DKCrm.Shared.Models.Products;

namespace DKCrm.Client.Services.BrandService
{
    public interface IBrandManager
    {
        Task<List<Brand>> GetAsync();
        Task<Brand> GetDetailsAsync(string id);
        Task UpdateAsync(Brand brand);
        Task AddAsync(Brand brand);
        Task RemoveRangeAsync(IEnumerable<Brand> brands);
    }
}
