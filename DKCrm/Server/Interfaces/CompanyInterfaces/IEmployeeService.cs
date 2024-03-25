using DKCrm.Shared.Models.CompanyModels;

namespace DKCrm.Server.Interfaces.CompanyInterfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAsync();
        Task<Employee> GetAsync(Guid id);
        Task<Guid> PostAsync(Employee employee);
        Task<Guid> PutAsync(Employee employee);
        Task<int> DeleteAsync(Guid id);
    }
}
