using AIChatBot.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AIChatBot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimeZoneController : ControllerBase
    {
        private readonly ITimeZoneService _timeZoneService;

        public TimeZoneController(ITimeZoneService timeZoneService)
        {
            _timeZoneService = timeZoneService;
        }

        [HttpGet("GetTimeByIanaZone")]
        public async Task<IActionResult> GetForecast([FromQuery] string Iana)
        {
            var timeZoneString = await _timeZoneService.GetDateByIanaZone(Iana);
            return Content(timeZoneString);
        }
    }
}
