using DKCrm.Server.Data;
using DKCrm.Server.Interfaces.CompanyInterfaces;
using DKCrm.Shared.Models.CompanyModels;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Services.CompanyServices
{
    public class CompanyTagsService : ICompanyTagsService
    {
        private readonly ApplicationDBContext _context;

        public CompanyTagsService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TagsCompany>> GetAsync()
        {
            return await _context.TagsCompanies.ToListAsync();
        }

        public async Task<TagsCompany> GetAsync(Guid id)
        {
            var tagsCompany = await _context.TagsCompanies.FirstOrDefaultAsync(a => a.Id == id);
            return tagsCompany ?? throw new InvalidOperationException();
        }

        public async Task<Guid> PostAsync(TagsCompany tagsCompany)
        {
            _context.Add(tagsCompany);
            await _context.SaveChangesAsync();
            return tagsCompany.Id;
        }

        public async Task<Guid> PutAsync(TagsCompany tagsCompany)
        {
            _context.Entry(tagsCompany).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return tagsCompany.Id;
        }

        public async Task<int> PutRangeAsync(IEnumerable<TagsCompany> tagsCompanies)
        {
            _context.TagsCompanies.UpdateRange(tagsCompanies);
           return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var dev = new TagsCompany { Id = id };
            _context.Remove(dev);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteRangeAsync(IEnumerable<TagsCompany> tagsCompanies)
        {
            _context.RemoveRange(tagsCompanies);
            return await _context.SaveChangesAsync();
        }
    }
}
