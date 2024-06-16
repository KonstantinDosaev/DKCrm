using DKCrm.Server.Data;
using DKCrm.Server.Interfaces.CompanyInterfaces;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models.CompanyModels;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Services.CompanyServices
{
    public class CompanyService : ICompanyService
    {
        private readonly ApplicationDBContext _context;
        public CompanyService(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Company>> GetAsync()
        {
            return await _context.Companies.AsNoTracking().Select(s => new Company()
            {
                Id = s.Id,
                ActualAddress = s.ActualAddress,
                ActualAddressId = s.ActualAddressId,
                Name = s.Name,
                Director = s.Director,
                TagsCompanies = s.TagsCompanies,
                CompanyTypeId = s.CompanyTypeId,
                FnsRequestId = s.FnsRequestId,
                CompanyType = s.CompanyType,
                Employees = s.Employees,
                Inn = s.Inn,
            }).ToListAsync();
        }
        public async Task<IEnumerable<Company>> GetCompaniesByTypeAsync(string companyType)
        {
            var companyTypeId = _context.CompanyTypes.FirstOrDefault(f => f.Name == companyType)!.Id;
            return await _context.Companies.Where(w=>w.CompanyTypeId==companyTypeId).AsNoTracking().Select(s => new Company()
            {
                Id = s.Id,
                ActualAddress = s.ActualAddress,
                ActualAddressId = s.ActualAddressId,
                Name = s.Name,
                Director = s.Director,
                TagsCompanies = s.TagsCompanies,
                CompanyTypeId = s.CompanyTypeId,
                FnsRequestId = s.FnsRequestId,
                CompanyType = s.CompanyType,
                Employees = s.Employees,
                Inn = s.Inn,
            }).ToListAsync();
        }
        public async Task<Company> GetAsync(Guid id)
        {
            var company = await _context.Companies.AsNoTracking().Include(i => i.ActualAddress).
                Include(i => i.CompanyType).
                Include(i => i.BankDetails).Include(i => i.FnsRequest).
                Include(i => i.Employees).
                Include(i => i.TagsCompanies).AsSingleQuery()
                .FirstOrDefaultAsync(a => a.Id == id);
            return company ?? throw new InvalidOperationException();
        }


        public async Task<Guid> PostAsync(Company company)
        {
            _context.Add(company);
            await _context.SaveChangesAsync();
            return company.Id;
        }

        public async Task<Guid> PutAsync(Company company)
        {
            _context.Entry(company).State = EntityState.Modified;
            if (company.Employees != null)
            {
                if (company.CompanyType!.Name != TypeCompanyNames.OurCompanies)
                {
                    foreach (var item in company.Employees)
                    {
                        if (item.Id == Guid.Empty)
                            _context.Entry(item).State = EntityState.Added;
                    }
                }
                else
                {
                    var companyDb = _context.Companies.AsNoTracking().
                        Include(i => i.Employees).
                        FirstOrDefault(i => i.Id == company.Id)!.Employees!.ToList();
                    foreach (var item in company.Employees)
                    {
                        if (companyDb.Select(s => s.Id).Contains(item.Id)) continue;
                        _context.Entry(item).State = item.Id != Guid.Empty ? EntityState.Modified : EntityState.Added;
                        companyDb.Add(item);
                    }
                    var exception = companyDb.Where(w => !company.Employees.Select(s => s.Id).Contains(w.Id)).ToList();
                    if (exception.Any())
                    {
                        foreach (var employee in exception)
                        {
                            var emp = _context.Employees.Include(i => i.Companies)
                                .FirstOrDefault(s => s.Id == employee.Id);
                            if (emp == null) continue;
                            emp.Companies?.Remove(emp.Companies.FirstOrDefault(w => w.Id == company.Id)!);
                            _context.Entry((object)emp).State = EntityState.Modified;
                        }
                    }
                }
            }

            if (company.BankDetails != null)
            {
                foreach (var item in company.BankDetails)
                {
                    _context.Entry(item).State = item.Id != Guid.Empty ? EntityState.Modified : EntityState.Added;
                }
            }

            if (company.FnsRequest != null)
            {
                _context.Entry(company.FnsRequest).State = company.FnsRequest.Id != Guid.Empty ? EntityState.Modified : EntityState.Added;
            }
            if (company.ActualAddress != null)
            {
                _context.Entry(company.ActualAddress).State = company.ActualAddress.Id != Guid.Empty ? EntityState.Modified : EntityState.Added;
            }

            await _context.SaveChangesAsync();
            return company.Id;
        }

        public async Task<int> PutRangeAsync(IEnumerable<Company> companies)
        {
            //_context.Entry(product).State = EntityState.Modified;
            _context.Companies.UpdateRange(companies);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var dev = await _context.Companies.Include(i => i.ActualAddress).
                Include(i => i.CompanyType).
                Include(i => i.BankDetails).
                Include(i => i.FnsRequest).
                Include(i => i.Employees).
                Include(i => i.TagsCompanies)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (dev!.ActualAddress != null) _context.Remove((object)dev.ActualAddress);
            _context.Remove(dev!);
            return await _context.SaveChangesAsync();
        }
       
        public async Task<int> DeleteRangeAsync(IEnumerable<Company> companies)
        {
            _context.RemoveRange(companies);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteBankDetailsAsync(Guid id)
        {
            _context.Remove(new BankDetails() { Id = id });
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateTagsCompanyAsync(TagsRequest tagRequest)
        {
            var changeCompany = await _context.Companies.Include(i => i.TagsCompanies).FirstOrDefaultAsync(f => f.Id == tagRequest.CompanyId);
            var tagsList = _context.TagsCompanies.Where(w => tagRequest.TagCollection.Contains(w.Id)).ToList();
            foreach (var item in tagsList)
            {
                if (changeCompany is { TagsCompanies: { } } && !changeCompany.TagsCompanies.Contains(item))
                {
                    changeCompany.TagsCompanies.Add(item);
                }
            }
            var except = changeCompany!.TagsCompanies!.Except(tagsList).ToList();
            if (except != null && except.Any())
            {
                foreach (var tag in except)
                {
                    changeCompany.TagsCompanies!.Remove(tag);
                }
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddEmployeeAsync(Company company)
        {
            _context.Entry(company).State = EntityState.Modified;

            if (company.Employees != null)
            {
                var empGuid = _context.Employees.Select(s => s.Id)
                    .Where(w => company.Employees.Select(s => s.Id).Contains(w));
                foreach (var item in company.Employees)
                {
                    _context.Entry(item).State = empGuid.Contains(item.Id) ? EntityState.Modified : EntityState.Added;
                }
            }
            return await _context.SaveChangesAsync();
        }
    }
}
