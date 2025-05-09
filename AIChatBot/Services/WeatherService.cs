using AIChatBot.DTO;
using System.Text.Json;
using AIChatBot.Interfaces;
using System.Text;

namespace AIChatBot.Services
{
    public class WeatherService: IWeatherService
    {
        private readonly HttpClient _httpClient;
        private string _apiKey;
        private string _weatherUrl;

        public WeatherService(HttpClient httpClient)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Configuration/appsettings.json");

            var config = builder.Build();
            _apiKey = config["WeatherApiKey"];
            _weatherUrl = config["WeatherUrl"];
            _httpClient = httpClient;
        }

        public async Task<string> GetForecast(string location, int days)
        {
            var url = $"{_weatherUrl}{_apiKey}&q={location}&days={days}&aqi=no&alerts=no";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            if (!response.IsSuccessStatusCode)
            {
                return "Error: Couldnt fetch the weather forcast, try again later.)";
            }

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var locationElement = root.GetProperty("location");
            var forecastArray = root.GetProperty("forecast").GetProperty("forecastday");

            var dto = new WeatherForecastDto
            {
                City = locationElement.GetProperty("name").GetString(),
                Country = locationElement.GetProperty("country").GetString(),
                LocalTime = locationElement.GetProperty("localtime").GetString(),
                ForecastDays = new List<ForecastDayDto>()
            };

            foreach (var day in forecastArray.EnumerateArray())
            {
                var dayInfo = day.GetProperty("day");
                var forecastDay = new ForecastDayDto
                {
                    Date = day.GetProperty("date").GetString(),
                    MaxTempC = dayInfo.GetProperty("maxtemp_c").GetDouble(),
                    AvgTempC = dayInfo.GetProperty("avgtemp_c").GetDouble(),
                    MinTempC = dayInfo.GetProperty("mintemp_c").GetDouble(),
                    Condition = dayInfo.GetProperty("condition").GetProperty("text").GetString(),
                    Hourly = new List<ForecastHourDto>()
                };

                var hourArray = day.GetProperty("hour");

                foreach (var hour in hourArray.EnumerateArray())
                {
                    forecastDay.Hourly.Add(new ForecastHourDto
                    {
                        Time = hour.GetProperty("time").GetString(),
                        TempC = hour.GetProperty("temp_c").GetDouble(),
                        Condition = hour.GetProperty("condition").GetProperty("text").GetString()
                    });
                }

                dto.ForecastDays.Add(forecastDay);
            }

            var sb = new StringBuilder();

            foreach (var day in dto.ForecastDays)
            {
                sb.AppendLine($"In the city of {dto.City}, on {day.Date}, the weather forecast shows a high of {day.MaxTempC}°C, a low of {day.MinTempC}°C, with overall conditions described as \"{day.Condition}\".");
                sb.AppendLine("Hourly forecast:");

                foreach (var hour in day.Hourly)
                {
                    sb.AppendLine($"  - At {hour.Time}, it will be {hour.TempC}°C and \"{hour.Condition}\".");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

    }
}
