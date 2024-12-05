using DKCrm.Server.Interfaces;
using DKCrm.Shared.Models.CompanyModels;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DKCrm.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AccessRestrictionController : ControllerBase
    {
        private readonly IAccessRestrictionService _accessRestrictionService;

        public AccessRestrictionController(IAccessRestrictionService accessRestrictionService)
        {
            _accessRestrictionService = accessRestrictionService;
        }

        [HttpGet("{componentId:guid}")]
        public async Task<IActionResult> CheckExistAccessAndContainsUserInArrayToComponent(Guid componentId) 
            => Ok(await _accessRestrictionService.CheckExistAccessAndContainsUserInArrayToComponentAsync(componentId, User));

        [HttpGet("{componentId:guid}")]
        public async Task<IActionResult> GetAccessFromComponent(Guid componentId)
            => Ok(await _accessRestrictionService.GetAccessFromComponentAsync(componentId));

        [HttpPost]
        public async Task<IActionResult> EditAccessToComponentAsync(AccessRestriction restriction)
            => Ok(await _accessRestrictionService.EditAccessToComponentAsync(restriction, User));

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAccess(Guid id)
        {
            return Ok(await _accessRestrictionService.RemoveAccessAsync(id));
        }
    }
}
