using DKCrm.Server.Data;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models.CompanyModels;
using DKCrm.Shared.Models.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DKCrm.Server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public CompanyController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //return Ok(await _context.Companies.Include(i=>i.ActualAddress)
            //    .Include(i=>i.CompanyType)
            //    .Include(i=>i.Employees)
            //    .Include(i=>i.TagsCompanies).ToListAsync());
            return Ok(await _context.Companies.AsNoTracking().Select(s=>new
            {
                s.Id,s.ActualAddress,s.ActualAddressId,s.Name,s.Director,s.TagsCompanies,s.CompanyTypeId,s.FnsRequestId,s.CompanyType,s.Employees,s.Inn,
            }).ToListAsync());
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var dev = await _context.Companies.AsNoTracking().Include(i => i.ActualAddress).
                Include(i => i.CompanyType).
                Include(i => i.BankDetails).Include(i => i.FnsRequest).
                Include(i => i.Employees).
                Include(i => i.TagsCompanies).AsSingleQuery()
                .FirstOrDefaultAsync(a => a.Id == id);
            return Ok(dev);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Company company)
        {
            _context.Add(company);
            await _context.SaveChangesAsync();
            return Ok(company.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Company company)
        {
            

            _context.Entry(company).State = EntityState.Modified;
            if (company.Employees!=null)
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
                    var exception = companyDb.Where(w=> !company.Employees.Select(s=>s.Id).Contains(w.Id)).ToList();
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
            return Ok(company);
        }
        [HttpPut("range")]
        public async Task<IActionResult> PutRange(IEnumerable<Company> companies)
        {
            //_context.Entry(product).State = EntityState.Modified;
            _context.Companies.UpdateRange(companies);
            await _context.SaveChangesAsync();
            return Ok(companies.Count());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
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
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost("removerange")]
        public async Task<IActionResult> DeleteRange(IEnumerable<Company> companies)
        {
            _context.RemoveRange(companies);
            await _context.SaveChangesAsync();
            return Ok(companies.Count());
        }

        [HttpDelete("bank-details/{id}")]
        public async Task<IActionResult> DeleteBankDetails(Guid id)
        {
            _context.Remove(new BankDetails() { Id = id });
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("updateTags/{tagRequest}")]
        public async Task<IActionResult> UpdateTagsCompany(TagsRequest tagRequest)
        {
         
                var changeCompany = await _context.Companies.Include(i=>i.TagsCompanies).FirstOrDefaultAsync(f => f.Id == tagRequest.CompanyId);
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
                
                await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("addemployee")]
        public async Task<IActionResult> AddEmployee(Company company)
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
            await _context.SaveChangesAsync();
            return Ok(company);
        }
    }
}
