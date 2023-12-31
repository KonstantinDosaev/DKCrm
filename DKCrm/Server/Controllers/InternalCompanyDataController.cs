using DKCrm.Server.Data;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models;
using DKCrm.Shared.Models.CompanyModels;
using DKCrm.Shared.Models.OrderModels;
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
           var data = await _context.InternalCompanyData.FirstOrDefaultAsync();
            if (data == null )
            {
                if (Initialize())
                    data = await _context.InternalCompanyData.FirstOrDefaultAsync();
                else
                    throw new InvalidOperationException();
            }
            return Ok(data);
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
        public bool Initialize()
        {
            _context.InternalCompanyData.Add(
                new InternalCompanyData(){Id = Guid.NewGuid(),CurrencyPercent = 0, KeyFns = "", LocalCurrency = "RUB"}
            );
            return _context.SaveChangesAsync().IsCompletedSuccessfully;
        }

    }
}
