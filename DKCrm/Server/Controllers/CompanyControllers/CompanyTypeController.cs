using DKCrm.Server.Data;
using DKCrm.Server.Interfaces.CompanyInterfaces;
using DKCrm.Shared.Models.CompanyModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Controllers.CompanyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyTypeController : ControllerBase
    {
        private readonly ICompanyTypeService _companyTypeService;

        public CompanyTypeController(ICompanyTypeService companyTypeService)
        {
            _companyTypeService = companyTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _companyTypeService.GetAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _companyTypeService.GetAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post(CompanyType company)
        {
            return Ok(await _companyTypeService.PostAsync(company));
        }

        [HttpPut]
        public async Task<IActionResult> Put(CompanyType company)
        {
            return Ok(await _companyTypeService.PutAsync(company));
        }

        [HttpPut("range")]
        public async Task<IActionResult> PutRange(IEnumerable<CompanyType> companies)
        {
            return Ok(await _companyTypeService.PutRangeAsync(companies));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _companyTypeService.DeleteAsync(id));
        }

        [HttpPost("removerange")]
        public async Task<IActionResult> DeleteRange(IEnumerable<CompanyType> companies)
        {
            return Ok(await _companyTypeService.DeleteRangeAsync(companies));
        }
    }
}
