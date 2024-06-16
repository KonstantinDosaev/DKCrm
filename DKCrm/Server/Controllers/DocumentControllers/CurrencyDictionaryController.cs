using DKCrm.Server.Interfaces.DocumentInterfaces;
using DKCrm.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace DKCrm.Server.Controllers.DocumentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyDictionaryController : ControllerBase
    {
        private readonly ICurrencyDictionaryService _currencyDictionaryService;


        public CurrencyDictionaryController(ICurrencyDictionaryService currencyDictionaryService)
        {
            _currencyDictionaryService = currencyDictionaryService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _currencyDictionaryService.GetAllAsync());
        }

        [HttpGet("{charCode}")]
        public async Task<IActionResult> Get(string charCode)
        {
            return Ok(await _currencyDictionaryService.GetOneByCharCodeAsync(charCode));
        }

        [HttpPost]
        public async Task<IActionResult> Post(CurrencyDictionary currencyDictionary)
        {
            return Ok(await _currencyDictionaryService.PostAsync(currencyDictionary));
        }

        [HttpPut]
        public async Task<IActionResult> Put(CurrencyDictionary currencyDictionary)
        {
            return Ok(await _currencyDictionaryService.PutAsync(currencyDictionary));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _currencyDictionaryService.DeleteAsync(id));
        }

    }
}
