using DKCrm.Server.Data;
using DKCrm.Shared.Models.CompanyModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyTypeController : ControllerBase
    {
        private readonly CompanyDbContext _context;

        public CompanyTypeController(CompanyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.CompanyTypes.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var dev = await _context.CompanyTypes.FirstOrDefaultAsync(a => a.Id == id);
            return Ok(dev);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CompanyType company)
        {
            _context.Add(company);
            await _context.SaveChangesAsync();
            return Ok(company.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Put(CompanyType company)
        {
            _context.Entry(company).State = EntityState.Modified;
            //_context.Update(entityToBeUpdated);
            await _context.SaveChangesAsync();
            return Ok(company);
        }

        [HttpPut("range")]
        public async Task<IActionResult> PutRange(IEnumerable<CompanyType> companies)
        {
            //_context.Entry(product).State = EntityState.Modified;
            _context.CompanyTypes.UpdateRange(companies);
            await _context.SaveChangesAsync();
            return Ok(companies.Count());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dev = new CompanyType { Id = id };
            _context.Remove(dev);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("removerange")]
        public async Task<IActionResult> DeleteRange(IEnumerable<CompanyType> companies)
        {
            _context.RemoveRange(companies);
            await _context.SaveChangesAsync();
            return Ok(companies.Count());
        }
    }
}
