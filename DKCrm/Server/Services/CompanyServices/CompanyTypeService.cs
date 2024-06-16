using DKCrm.Server.Data;
using DKCrm.Server.Interfaces.CompanyInterfaces;
using DKCrm.Shared.Models.CompanyModels;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Services.CompanyServices
{
    public class CompanyTypeService : ICompanyTypeService
    {
        private readonly ApplicationDBContext _context;

        public CompanyTypeService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CompanyType>> GetAsync()
        {
            return await _context.CompanyTypes.ToListAsync();
        }

        public async Task<CompanyType> GetAsync(Guid id)
        {
            var companyType = await _context.CompanyTypes.FirstOrDefaultAsync(a => a.Id == id);
            return companyType ?? throw new InvalidOperationException();
        }

        public async Task<Guid> PostAsync(CompanyType company)
        {
            _context.Add(company);
            await _context.SaveChangesAsync();
            return company.Id;
        }

        public async Task<Guid> PutAsync(CompanyType company)
        {
            _context.Entry(company).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return company.Id;
        }

        public async Task<int> PutRangeAsync(IEnumerable<CompanyType> companies)
        {
            //_context.Entry(product).State = EntityState.Modified;
            _context.CompanyTypes.UpdateRange(companies);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var dev = new CompanyType { Id = id };
            _context.Remove(dev);
            return await _context.SaveChangesAsync(); ;
        }

        public async Task<int> DeleteRangeAsync(IEnumerable<CompanyType> companies)
        {
            _context.RemoveRange(companies);
            return await _context.SaveChangesAsync();
        }
    }
}
