using AIChatBot.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AIChatBot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet("CurrencyExchange")]
        public async Task<IActionResult> GetForecast([FromQuery] string baseCurrency, [FromQuery] string diffCurrency, [FromQuery] int amount)
        {
            var currencyExchange = await _currencyService.CurrencyExchange(baseCurrency, diffCurrency, amount);
            return Content(currencyExchange);
        }
    }
}
