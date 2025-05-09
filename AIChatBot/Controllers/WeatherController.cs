using AIChatBot.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AIChatBot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("forecast")]
        public async Task<IActionResult> GetForecast([FromQuery] string location, [FromQuery] int days)
        {
            var forecastJson = await _weatherService.GetForecast(location, days);
            return Content(forecastJson);
        }
    }
}
