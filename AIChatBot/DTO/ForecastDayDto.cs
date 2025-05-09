namespace AIChatBot.DTO
{
    public class ForecastDayDto
    {
        public string Date { get; set; }
        public double MaxTempC { get; set; }
        public double AvgTempC { get; set; }
        public double MinTempC { get; set; }
        public string Condition { get; set; }
        public List<ForecastHourDto> Hourly { get; set; }
    }

}
