using System.Security.Claims;
using DKCrm.Server.Data;
using DKCrm.Server.Interfaces;
using DKCrm.Server.Interfaces.CompanyInterfaces;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models;
using DKCrm.Shared.Models.CompanyModels;
using DKCrm.Shared.Models.Products;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using OpenXmlPowerTools;

namespace DKCrm.Server.Services.CompanyServices
{
    public class CompanyService : ICompanyService
    {
        private readonly ApplicationDBContext _context;
        private readonly IAccessRestrictionService _accessRestrictionService;
        public CompanyService(ApplicationDBContext context, IAccessRestrictionService accessRestrictionService)
        {
            _context = context;
            _accessRestrictionService = accessRestrictionService;
        }
        public async Task<IEnumerable<Company>> GetAsync(ClaimsPrincipal user)
        {
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            if (userId == null) return Array.Empty<Company>();
            var companyList = await _context.Companies.AsNoTracking().Select(s => new Company()
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
                Phone = s.Phone,
                Email = s.Email,
                PhoneAdditional = s.PhoneAdditional,
                EmailAdditional = s.EmailAdditional,
            }).ToListAsync();
            var result = new List<Company>();
            foreach (var company in companyList)
            {
                if (await _accessRestrictionService.CheckExistAccessAndContainsUserInArrayToComponentAsync(company.Id, user))
                    result.Add(company);
            }
            return result;
        }
        public async Task<IEnumerable<Company>> GetCompaniesByTypeAsync(string companyType, ClaimsPrincipal user)
        {
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            if (userId == null) return Array.Empty<Company>();

            var companyTypeId = _context.CompanyTypes.FirstOrDefault(f => f.Name == companyType)!.Id;
            var companyList = await _context.Companies.Where(w=>w.CompanyTypeId==companyTypeId).AsNoTracking().Select(s => new Company()
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
                Phone = s.Phone,
                Email = s.Email,
                PhoneAdditional = s.PhoneAdditional,
                EmailAdditional = s.EmailAdditional,
            }).ToListAsync();
            var result = new List<Company>();
            foreach (var company in companyList)
            {
                if(await _accessRestrictionService.CheckExistAccessAndContainsUserInArrayToComponentAsync(company.Id, user))
                    result.Add(company);
            }
            return result;
        }
        public async Task<Company> GetAsync(Guid id, ClaimsPrincipal user)
        {
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            if (userId == null || !await _accessRestrictionService.CheckExistAccessAndContainsUserInArrayToComponentAsync(id, user)) return new Company();

            var company = await _context.Companies.AsNoTracking().Include(i => i.ActualAddress).
                Include(i => i.CompanyType).
                Include(i => i.BankDetails).Include(i => i.FnsRequest).
                Include(i => i.Employees).
                Include(i => i.TagsCompanies).AsSingleQuery()
                .FirstOrDefaultAsync(a => a.Id == id);

            return company ?? throw new InvalidOperationException();
        }
        public async Task<SortPagedResponse<Company>> GetBySortPagedSearchChapterAsync(SortPagedRequest<FilterCompanyTuple> request, ClaimsPrincipal user)
        {
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            if (userId == null) return new SortPagedResponse<Company>();
            var accessCollection =  _context.AccessRestrictions;
            var accessedComponents = accessCollection.Select(s => s.AccessedComponentId);
            var data =  _context.Companies.AsNoTracking().Select(s => new Company()
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
                Phone = s.Phone,
                Email = s.Email,
                PhoneAdditional = s.PhoneAdditional,
                EmailAdditional = s.EmailAdditional,
            });
            data = data.Where(w => (
                ((!accessedComponents.Contains((Guid)w.Id) || (
                    accessedComponents.Contains((Guid)w.Id)
                    && accessCollection.First(f => f.AccessedComponentId == w.Id).AccessUsersToComponent
                        .Contains(userId))))));
            if (!string.IsNullOrEmpty(request.GlobalFilterValue))
            {
                switch (request.GlobalFilterType)
                {
                    case GlobalFilterTypes.Company:
                        data = data.Where(w => w.Name.ToLower().Contains(request.GlobalFilterValue.ToLower()));
                        break;
                    /*case GlobalFilterTypes.ExportedOrder:
                    {
                        var searchedListId = await _context.ExportedProducts
                            .Where(w => w.ExportedOrder != null && w.ExportedOrder.Number!.Contains(request.GlobalFilterValue))
                            .Select(s => s.ProductId).ToListAsync();
                        data = data.Where(w => searchedListId.Contains(w.Id));
                        break;
                    }
                    case GlobalFilterTypes.ImportedOrder:
                    {
                        var searchedListId = await _context.ImportedProducts
                            .Where(w => w.ImportedOrder != null && w.ImportedOrder.Number!.Contains(request.GlobalFilterValue))
                            .Select(s => s.ProductId).ToListAsync();
                        data = data.Where(w => searchedListId.Contains(w.Id));
                        break;
                    }*/
                }
            }
            if (!string.IsNullOrEmpty(request.SearchString))
            {
                request.SearchString = request.SearchString.Trim().ToLower();
                data = data.Where(w =>  (w.Name.ToLower().Contains(request.SearchString)
                                                              ||w.Director!.ToLower().Contains(request.SearchString))||
                                       w.ActualAddress!.City.ToLower().Contains(request.SearchString) 
                                       || w.ActualAddress.Country.ToLower().Contains(request.SearchString));
            }

            if (request.FilterTuple != null)
            {
                if (request.FilterTuple.CompanyTypeId != null)
                {
                    data = data.Where(o => o.CompanyTypeId == request.FilterTuple.CompanyTypeId);
                }
                /*if (request.FilterTuple.CategoryId != null && request.FilterTuple.CategoryId != Guid.Empty && request.Chapter != ProductFromChapterNames.Category)
                {
                    data = data.Where(o => o.CategoryId == request.FilterTuple.CategoryId);
                }
                if (request.FilterTuple.BrandIdList != null && request.FilterTuple.BrandIdList.Any() && request.Chapter != ProductFromChapterNames.Brand)
                {
                    data = data.Where(o => request.FilterTuple.BrandIdList.Contains((Guid)o.BrandId!));
                }
                if (request.FilterTuple.ProductOptions != null && request.FilterTuple.ProductOptions.Any())
                {
                    var productsId = _context.ProductOptions
                        .Where(w => request.FilterTuple.ProductOptions.Contains(w.Id))
                        .Select(o => o.ProductId).Distinct();
                    data = data.Where(w => productsId.Contains(w.Id));
                }*/
            }

            var totalItems = data.Count();

            switch (request.SortLabel)
            {
                case "name_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.Name);
                    break;
                case "director_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.Director);
                    break;
                case "country_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.ActualAddress!.Country);
                    break;
                case "city_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.ActualAddress!.City);
                    break;
            }
            data = data.Skip(request.PageIndex * request.PageSize).Take(request.PageSize);

            return new SortPagedResponse<Company>() { TotalItems = totalItems, Items = await data.AsSingleQuery().ToListAsync() };

        }
        public async Task<Guid> PostAsync(Company company)
        {
            _context.Add(company);
            await _context.SaveChangesAsync();
            return company.Id;
        }
        public async Task<Guid> AddBankDetails(BankDetails bankDetails)
        {
            _context.BankDetails.Add(bankDetails);
            await _context.SaveChangesAsync();
            return bankDetails.Id;
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
