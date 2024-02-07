using DKCrm.Server.Data;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models.CompanyModels;
using DKCrm.Shared.Models.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DKCrm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController: ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public EmployeeController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Employees.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var dev = await _context.Employees.FirstOrDefaultAsync(a => a.Id == id);
            return Ok(dev);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Employee employee)
        {
            _context.Entry(employee).State = EntityState.Added;

            if (employee.Companies != null)
            {
               foreach (var item in employee.Companies)
               {
                    _context.Entry(item).State = EntityState.Modified;
               }
            }
            
            await _context.SaveChangesAsync();
            return Ok(employee.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Employee employee)
        {
            _context.Entry(employee).State = EntityState.Modified;

            //if (employee.Companies != null)
            //{
            //    if (!employee.IsOurEmployee)
            //    {
            //        foreach (var item in employee.Companies)
            //        {
            //            _context.Entry(item).State =  EntityState.Modified;
            //        }
            //    }
            //    else
            //    {
            //        var employeeInCompaniesDb = _context.Employees.AsNoTracking().FirstOrDefault(f => f.Id == employee.Id)!.Companies!.ToList();
            //        var employeeInCompanies = employee.Companies;
            //        foreach (var item in employeeInCompanies)
            //        {
            //            if (employeeInCompaniesDb.Select(s => s.Id).Contains(item.Id))
            //            {
            //                _context.Entry(item).State = EntityState.Modified;
            //            }
            //            else
            //            {
            //                _context.Entry(item).State = EntityState.Added;
            //            }

            //        }
                    

            //        foreach (var employee in companyDb.Employees!)
            //        {
            //            if (!company.Employees.Select(s => s.Id).Contains(employee.Id))
            //            {
            //                employee.Companies!.Remove(company);
            //                _context.Entry(employee).State = EntityState.Modified;
            //            }
            //        }
            //    }
            //}

            await _context.SaveChangesAsync();
            return Ok(employee);
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dev = new Employee { Id = id };
            _context.Remove(dev);
            await _context.SaveChangesAsync();
            return NoContent();
        }


    }
}
