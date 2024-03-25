using DKCrm.Server.Data;
using DKCrm.Server.Interfaces.ProductInterfaces;
using DKCrm.Shared.Models.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Services.ProductServices
{
    public class CategoryOptionsService : ICategoryOptionsService
    {
        private readonly ApplicationDBContext _context;
        public CategoryOptionsService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryOption>> GetAsync()
        {
            return await _context.CategoryOptions
                .Include(i => i.Category)
                .Include(i => i.ProductOption)
                .ToListAsync();
        }

        public async Task<CategoryOption> GetAsync(Guid id)
        {
            var dev = await _context.CategoryOptions
                .Include(i => i.Category)
                .Include(i => i.ProductOption)
                .FirstOrDefaultAsync(a => a.Id == id);
            return dev ?? throw new InvalidOperationException();
        }
        
        public async Task<Guid> PostAsync(CategoryOption option)
        {
            _context.CategoryOptions.Add(option);
            await _context.SaveChangesAsync();
            return option.Id;
        }

        public async Task<Guid> PutAsync(CategoryOption option)
        {
            var options = _context.CategoryOptions.Select(s => s.Id);
            _context.Entry(option).State = options.Contains(option.Id) ? EntityState.Modified : EntityState.Added;
            await _context.SaveChangesAsync();
            return option.Id;
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var dev = _context.CategoryOptions.FirstOrDefault(f => f.Id == id);
            if (dev != null) _context.CategoryOptions.Remove(dev);
            return await _context.SaveChangesAsync();
        }
    }
}
