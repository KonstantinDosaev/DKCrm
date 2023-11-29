using DKCrm.Server.Data;
using DKCrm.Shared.Models.CompanyModels;
using DKCrm.Shared.Models.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DKCrm.Server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly CompanyDbContext _context;
        public CompanyController(CompanyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //var е =  _context.Products.ToList();
            return Ok(await _context.Companies.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var dev = await _context.Companies.FirstOrDefaultAsync(a => a.Id == id);
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

            
            if (company.BankDetails != null)
            {
                foreach (var item in company.BankDetails)
                {
                    _context.Entry(item).State = item.Id != Guid.Empty ? EntityState.Modified : EntityState.Added;
                }
            }
            
            if (company.FnsRequest!=null)
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
            var dev = new Company { Id = id };
            _context.Remove(dev);
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
         
                var changeCompany = await _context.Companies.FirstOrDefaultAsync(f => f.Id == tagRequest.CompanyId);
                var tagsList = _context.TagsCompanies.Where(w => tagRequest.TagCollection.Contains(w.Id)).ToList();
                foreach (var item in tagsList)
                {
                    
                    if (changeCompany is { TagsCompanies: { } } && !changeCompany.TagsCompanies.Contains(item))
                    {
                        changeCompany.TagsCompanies.Add(item);
                   
                    }
                }
                var except = changeCompany.TagsCompanies!.Except(tagsList).ToList();
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
    }
}
