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
    public class StorageController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public StorageController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Storages.Include(i => i.Address).Include(i=>i.Products).AsNoTracking().ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var dev = await _context.Storages.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
            return Ok(dev);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Storage storage)
        {
            _context.Add(storage);
            await _context.SaveChangesAsync();
            return Ok(storage.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Storage storage)
        {
            _context.Entry(storage).State = EntityState.Modified;
            if (storage.Address != null)
            {
                _context.Entry(storage.Address).State = storage.Address.Id != Guid.Empty ? EntityState.Modified : EntityState.Added;
            }
            //_context.Update(entityToBeUpdated);
            await _context.SaveChangesAsync();
            return Ok(storage);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dev = await _context.Storages.Include(i=>i.Address).FirstOrDefaultAsync(a => a.Id == id);
            _context.Remove(dev!);
            _context.Remove(dev!.Address!);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
