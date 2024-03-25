using DKCrm.Shared.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Interfaces.ProductInterfaces
{
    public interface IBrandService
    { 
        Task<IEnumerable<Brand>> GetAsync();
       Task<Brand> GetAsync(Guid id);
       Task<Guid> PostAsync(Brand brand);
       Task<Guid> PutAsync(Brand brand);
       Task<int> DeleteAsync(Guid id);
       Task<int> DeleteRangeAsync(IEnumerable<Brand> brands);
    }
}
