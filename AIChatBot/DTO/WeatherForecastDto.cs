namespace AIChatBot.DTO
{
    public class WeatherForecastDto
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string LocalTime { get; set; }
        public List<ForecastDayDto> ForecastDays { get; set; }
    }
}
