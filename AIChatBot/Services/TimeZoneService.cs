using AIChatBot.Interfaces;
using Newtonsoft.Json.Linq;

namespace AIChatBot.Services
{
    public class TimeZoneService : ITimeZoneService
    {
        private readonly HttpClient _httpClient;
        private readonly string _timeZoneUrl;

        public TimeZoneService(HttpClient httpClient)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Configuration/appsettings.json");

            var config = builder.Build();
            _timeZoneUrl = config["TimeZoneUrl"];
            _httpClient = httpClient;
        }

        public async Task<string> GetDateByIanaZone(string iana)
        {
            var url = $"{_timeZoneUrl}/Time/current/zone?timeZone={iana}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return "Error: Couldnt fetch the Time zone, try again later.)";
            }

            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);

            var dateTime = "The date (year-month-day) and time from " + iana + " is " + json["dateTime"]?.ToString();

            return dateTime ;
        }
    }
}
