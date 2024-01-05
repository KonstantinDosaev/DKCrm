using DKCrm.Shared.Models.CompanyModels;

namespace DKCrm.Client.Services.CurrencyService
{
    public interface ICurrencyManager
    {
        Task<List<(string charCode, string name)>> GetCurrencyCharCode();

        Task<List<(string charCod, decimal?)>> CurrencyConverter(string currencyToConvert, decimal? firstPrice,
            double currencyPercent, IEnumerable<string> currencyCharCodesToConvert);
    }
}
