using DKCrm.Server.Data;
using DKCrm.Server.Interfaces.DocumentInterfaces;
using DKCrm.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Services.DocumentServices
{
    public class CurrencyDictionaryService: ICurrencyDictionaryService
    {
        private readonly ApplicationDBContext _context;

        public CurrencyDictionaryService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CurrencyDictionary>> GetAllAsync()
        {
            var currencies = await _context.CurrencyDictionaries.ToArrayAsync();
            if (!currencies.Any())
            {
                if (Initialize())
                    currencies = await _context.CurrencyDictionaries.ToArrayAsync();
                else
                    throw new InvalidOperationException();
            }
            return currencies;
        }

        public async Task<CurrencyDictionary> GetOneByCharCodeAsync(string charCode)
        {
            var currencyDictionary = await _context.CurrencyDictionaries.FirstOrDefaultAsync(f => f.CharCode == charCode);
            if (currencyDictionary == null)
            {
                if (Initialize())
                    currencyDictionary = await _context.CurrencyDictionaries.FirstOrDefaultAsync(f => f.CharCode == charCode);
            }
            return currencyDictionary ?? throw new InvalidOperationException();
        }

        public async Task<Guid> PostAsync(CurrencyDictionary currencyDictionary)
        {
            _context.Entry(currencyDictionary).State = EntityState.Added; 
            await _context.SaveChangesAsync();
            return currencyDictionary.Id;
        }
        public async Task<Guid> PutAsync(CurrencyDictionary currencyDictionary)
        {
            _context.Entry(currencyDictionary).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return currencyDictionary.Id;
        }
        public async Task<int> DeleteAsync(Guid id)
        {
            var item = _context.CurrencyDictionaries.FirstOrDefault(f => f.Id == id);
            _context.Entry(id).State = EntityState.Deleted;
            return await _context.SaveChangesAsync();
        }

        private bool Initialize()
        {
            _context.CurrencyDictionaries.AddRange(new[]
            {
                new CurrencyDictionary(){Id = Guid.NewGuid(), CharCode = "RUB", Male = true, MaleLoverNominal =false,
                    One = "рубль", Two = "рубля", Five = "рублей", 
                    OneLoverNominal = "копейка", TwoLoverNominal = "копейки", FiveLoverNominal = "копеек"},
                new CurrencyDictionary(){Id = Guid.NewGuid(), CharCode = "USD",Male = true, MaleLoverNominal = true,
                    One = "доллар", Two = "доллара", Five = "долларов",
                    OneLoverNominal = "цент", TwoLoverNominal = "цента", FiveLoverNominal = "центов"},
                new CurrencyDictionary(){Id = Guid.NewGuid(), CharCode = "EUR", Male = true, MaleLoverNominal = false,
                    One = "евро", Two = "евро", Five = "евро",
                    OneLoverNominal = "цент", TwoLoverNominal = "цента", FiveLoverNominal = "центов"},
            });
            return _context.SaveChangesAsync().IsCompletedSuccessfully;
        }
    }
}
