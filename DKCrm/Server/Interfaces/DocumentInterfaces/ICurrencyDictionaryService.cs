using DKCrm.Shared.Models;

namespace DKCrm.Server.Interfaces.DocumentInterfaces
{
    public interface ICurrencyDictionaryService
    {
        Task<IEnumerable<CurrencyDictionary>> GetAllAsync();
        Task<CurrencyDictionary> GetOneByCharCodeAsync(string charCode);
        Task<Guid> PostAsync(CurrencyDictionary currencyDictionary);
        Task<Guid> PutAsync(CurrencyDictionary currencyDictionary);
        Task<int> DeleteAsync(Guid id);
    }
}
