using DKCrm.Server.Data;
using DKCrm.Server.Interfaces.ProductInterfaces;
using DKCrm.Shared.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Services.ProductServices
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDBContext _context;
        public CategoryService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAsync()
        {
            return await _context.Categories
                .Include(i => i.Parent)
                .Include(i => i.Children)
                .Include(i => i.Products)
                .Include(i => i.CategoryOptions!).ThenInclude(t => t.ProductOption)
                .ToListAsync();
        }

        public async Task<Category> GetAsync(Guid id)
        {
            var category = await _context.Categories
                .Include(i => i.Parent)
                .Include(i => i.Children)
                .Include(i => i.Products)
                .Include(i => i.CategoryOptions!).ThenInclude(t => t.ProductOption)
                .FirstOrDefaultAsync(a => a.Id == id);
            return category ?? throw new InvalidOperationException();
        }

        public async Task<Guid> PostAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category.Id;
        }

        public async Task<Guid> PutAsync(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return category.Id;
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var dev = _context.Categories.FirstOrDefault(f => f.Id == id);

            if (dev != null) _context.Categories.Remove(dev);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteRangeAsync(IEnumerable<Category> categories)
        {
            var enumerable = categories.ToList();
            _context.RemoveRange(enumerable);
            return await _context.SaveChangesAsync();
        }
    }
}
