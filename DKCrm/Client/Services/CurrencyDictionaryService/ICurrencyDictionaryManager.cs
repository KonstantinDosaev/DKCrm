using DKCrm.Shared.Models;

namespace DKCrm.Client.Services.CurrencyDictionaryService
{
    public interface ICurrencyDictionaryManager
    {
        Task<List<CurrencyDictionary>> GetAsync();
        Task<CurrencyDictionary> GetDetailsAsync(string charCode);
        Task<bool> UpdateAsync(CurrencyDictionary currencyDictionary);
        Task<bool> AddAsync(CurrencyDictionary currencyDictionary);
        Task<bool> RemoveAsync(Guid id);
    }
}
