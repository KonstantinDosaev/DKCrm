using DKCrm.Server.Data;
using DKCrm.Shared.Models;
using DKCrm.Shared.Models.CompanyModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InternalCompanyDataController: ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public InternalCompanyDataController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.InternalCompanyData.SingleOrDefaultAsync());
        }


        [HttpPost]
        public async Task<IActionResult> Post(InternalCompanyData data)
        {
            _context.Add(data);
            await _context.SaveChangesAsync();
            return Ok(data.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Put(InternalCompanyData data)
        {
            _context.Entry(data).State = EntityState.Modified;
            //_context.Update(entityToBeUpdated);
            await _context.SaveChangesAsync();
            return Ok(data);
        }

    }
}
