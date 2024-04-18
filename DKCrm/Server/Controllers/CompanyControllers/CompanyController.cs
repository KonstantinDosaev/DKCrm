using DKCrm.Server.Interfaces.CompanyInterfaces;
using DKCrm.Shared.Models.CompanyModels;
using Microsoft.AspNetCore.Mvc;


namespace DKCrm.Server.Controllers.CompanyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _companyService.GetAsync());
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _companyService.GetAsync(id));
        }
        [HttpGet("byType/{type}")]
        public async Task<IActionResult> GetCompaniesByType(string type)
        {
            return Ok(await _companyService.GetCompaniesByTypeAsync(type));
        }
        [HttpPost]
        public async Task<IActionResult> Post(Company company)
        {
            return Ok(await _companyService.PostAsync(company));
        }

        [HttpPut]
        public async Task<IActionResult> Put(Company company)
        {
            return Ok(await _companyService.PutAsync(company));
        }

        [HttpPut("range")]
        public async Task<IActionResult> PutRange(IEnumerable<Company> companies)
        {
            return Ok(await _companyService.PutRangeAsync(companies));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _companyService.DeleteAsync(id));
        }
        [HttpPost("removerange")]
        public async Task<IActionResult> DeleteRange(IEnumerable<Company> companies)
        {
            return Ok(await _companyService.DeleteRangeAsync(companies));
        }

        [HttpDelete("bank-details/{id}")]
        public async Task<IActionResult> DeleteBankDetails(Guid id)
        {
            return Ok(await _companyService.DeleteBankDetailsAsync(id));
        }

        [HttpPost("updateTags/{tagRequest}")]
        public async Task<IActionResult> UpdateTagsCompany(TagsRequest tagRequest)
        {
            return Ok(await _companyService.UpdateTagsCompanyAsync(tagRequest));
        }

        [HttpPut("addemployee")]
        public async Task<IActionResult> AddEmployee(Company company)
        {
            return Ok(await _companyService.AddEmployeeAsync(company));
        }
    }
}
