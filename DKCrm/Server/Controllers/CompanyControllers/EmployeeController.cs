using DKCrm.Server.Interfaces.CompanyInterfaces;
using DKCrm.Shared.Models.CompanyModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DKCrm.Server.Controllers.CompanyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _employeeService.GetAsync());
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _employeeService.GetAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post(Employee employee)
        {
            return Ok(await _employeeService.PostAsync(employee));
        }

        [HttpPut]
        public async Task<IActionResult> Put(Employee employee)
        {
            return Ok(await _employeeService.PutAsync(employee));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _employeeService.DeleteAsync(id));
        }
    }
}
