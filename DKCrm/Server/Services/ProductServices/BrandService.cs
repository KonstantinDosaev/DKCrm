using DKCrm.Server.Data;
using DKCrm.Server.Interfaces.ProductInterfaces;
using DKCrm.Shared.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Services.ProductServices
{
    public class BrandService : IBrandService
    {
        private readonly ApplicationDBContext _context;
        public BrandService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Brand>> GetAsync()
        {
            return await _context.Brands.ToListAsync();
        }

        public async Task<Brand> GetAsync(Guid id)
        {
            var brand = await _context.Brands.FirstOrDefaultAsync(a => a.Id == id);
            return brand ?? throw new InvalidOperationException();
        }

        public async Task<Guid> PostAsync(Brand brand)
        {
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
            return brand.Id;
        }

        public async Task<Guid> PutAsync(Brand brand)
        {
            _context.Entry(brand).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return brand.Id;
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var dev = new Brand { Id = id };
            _context.Remove(dev);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteRangeAsync(IEnumerable<Brand> brands)
        {
            _context.RemoveRange(brands);
            return await _context.SaveChangesAsync();
        }
    }
}
