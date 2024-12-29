using DKCrm.Server.Interfaces.OrderInterfaces;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models;
using DKCrm.Shared.Models.CompanyModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DKCrm.Server.Interfaces.CompanyInterfaces;
using DKCrm.Shared.Requests;

namespace DKCrm.Server.Controllers.CompanyControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class CompanyCommentsController : ControllerBase
    {
        private readonly ICompanyCommentsService _companyCommentsService;

        public CompanyCommentsController(ICompanyCommentsService companyCommentsService)
        {
            _companyCommentsService = companyCommentsService;
        }

        [HttpGet("{companyId:guid}")]
        public async Task<IActionResult> GetAllForCompany(Guid companyId) => Ok(await _companyCommentsService.GetAllCommentsFromCompanyAsync(companyId, User));

        [HttpPost]
        public async Task<IActionResult> GetBySortPagedSearchChapterAsync(SortPagedRequest<FilterOrderCommentTuple> request)
            => Ok(await _companyCommentsService.GetBySortPagedSearchAsync(request));

        [HttpPost]
        public async Task<IActionResult> SaveComment(CompanyComment comment)
        {
            return Ok(await _companyCommentsService.SaveCommentAsync(comment, User));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRange(IEnumerable<Guid> listId)
        {
            return Ok(await _companyCommentsService.RemoveRangeAsync(listId, User));
        }
       
        [HttpPost]
        public async Task<IActionResult> SetLogUsersVisit(LogUsersVisitToCompanyComments newLog)
        {
            return Ok(await _companyCommentsService.SetLogUserVisit(newLog, User));
        }
        [HttpGet("{companyId:guid}")]
        public async Task<IActionResult> GetLogUsersVisit(Guid companyId) =>
            Ok(await _companyCommentsService.GetLogUserVisitAsync(companyId, User));
        [HttpPost]
        public async Task<IActionResult> GetWarningComments(GetWarningCommentsToCompanyRequest request)
            => Ok(await _companyCommentsService.GetWarningCommentsAsync(request, User));
    }
}
