using DKCrm.Server.Data;
using DKCrm.Server.Interfaces.CompanyInterfaces;
using DKCrm.Shared.Models.CompanyModels;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Services.CompanyServices
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDBContext _context;

        public EmployeeService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee> GetAsync(Guid id)
        {
            var employee = await _context.Employees.Select(s=> new Employee()
            {
                Id = s.Id,
                FirstName = s.FirstName,
                MiddleName  = s.MiddleName,
                LastName = s.LastName,
                Position = s.Position,
                Phone = s.Phone,
                Email = s.Email,
                Description = s.Description,
                IsOurEmployee = s.IsOurEmployee,
                Companies = s.Companies,
                UserId = s.UserId,
            }).FirstOrDefaultAsync(a => a.Id == id);
            return employee ?? throw new InvalidOperationException();
        }

        public async Task<Guid> PostAsync(Employee employee)
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
            return employee.Id;
        }

        public async Task<Guid> PutAsync(Employee employee)
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
            return employee.Id;
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var employee = new Employee { Id = id };
            _context.Remove(employee);
            return await _context.SaveChangesAsync();
        }

    }
}
