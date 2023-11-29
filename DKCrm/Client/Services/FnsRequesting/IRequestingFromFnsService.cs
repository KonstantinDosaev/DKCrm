using DKCrm.Shared.Models.CompanyModels;


namespace DKCrm.Client.Services.FnsRequesting
{
    public interface IRequestingFromFnsService
    {
        Task <IEnumerable<FnsRequest>> GetCompanyByInn(string inn);
    }
}
