using DKCrm.Server.Data;
using DKCrm.Shared.Models.CompanyModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyTagsController : ControllerBase
    {
        private readonly CompanyDbContext _context;

        public CompanyTagsController(CompanyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.TagsCompanies.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var dev = await _context.TagsCompanies.FirstOrDefaultAsync(a => a.Id == id);
            return Ok(dev);
        }

        [HttpPost]
        public async Task<IActionResult> Post(TagsCompany tagsCompany)
        {
            _context.Add(tagsCompany);
            await _context.SaveChangesAsync();
            return Ok(tagsCompany.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Put(TagsCompany tagsCompany)
        {
            _context.Entry(tagsCompany).State = EntityState.Modified;
            //_context.Update(entityToBeUpdated);
            await _context.SaveChangesAsync();
            return Ok(tagsCompany);
        }

        [HttpPut("range")]
        public async Task<IActionResult> PutRange(IEnumerable<TagsCompany> tagsCompanies)
        {
            //_context.Entry(product).State = EntityState.Modified;
            _context.TagsCompanies.UpdateRange(tagsCompanies);
            await _context.SaveChangesAsync();
            return Ok(tagsCompanies.Count());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dev = new TagsCompany { Id = id };
            _context.Remove(dev);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("removerange")]
        public async Task<IActionResult> DeleteRange(IEnumerable<TagsCompany> tagsCompanies)
        {
            _context.RemoveRange(tagsCompanies);
            await _context.SaveChangesAsync();
            return Ok(tagsCompanies.Count());
        }
    }
}
