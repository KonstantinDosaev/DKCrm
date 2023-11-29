using DKCrm.Shared.Models.CompanyModels;

namespace DKCrm.Client.Services.CompanyServices
{
    public interface IEmployeeManager
    {
        Task<List<Employee>> GetAsync();
        Task<Employee> GetDetailsAsync(Guid id);
        Task<bool> UpdateAsync(Employee employee);
        Task<bool> AddAsync(Employee employee);
        Task<bool> RemoveAsync(Guid id);
    }
}
