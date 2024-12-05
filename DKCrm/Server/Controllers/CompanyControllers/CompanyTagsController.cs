using DKCrm.Server.Interfaces.CompanyInterfaces;
using DKCrm.Shared.Models.CompanyModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DKCrm.Server.Controllers.CompanyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyTagsController : ControllerBase
    {
        private readonly ICompanyTagsService _companyTagsService;

        public CompanyTagsController(ICompanyTagsService companyTagsService)
        {
            _companyTagsService = companyTagsService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _companyTagsService.GetAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _companyTagsService.GetAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post(TagsCompany tagsCompany)
        {
            return Ok(await _companyTagsService.PostAsync(tagsCompany));
        }

        [HttpPut]
        public async Task<IActionResult> Put(TagsCompany tagsCompany)
        {
            return Ok(await _companyTagsService.PutAsync(tagsCompany));
        }

        [HttpPut("range")]
        public async Task<IActionResult> PutRange(IEnumerable<TagsCompany> tagsCompanies)
        {
            return Ok(await _companyTagsService.PutRangeAsync(tagsCompanies));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _companyTagsService.DeleteAsync(id));
        }

        [HttpPost("removerange")]
        public async Task<IActionResult> DeleteRange(IEnumerable<TagsCompany> tagsCompanies)
        {
            return Ok(await _companyTagsService.DeleteRangeAsync(tagsCompanies));
        }
    }
}
