using DKCrm.Shared.Models.CompanyModels;

namespace DKCrm.Client.Services.CurrencyService
{
    public interface ICurrencyManager
    {
        Task<List<(string charCode, string name)>> GetCurrencyCharCode();
    }
}
