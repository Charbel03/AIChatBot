namespace AIChatBot.Interfaces
{
    public interface IWeatherService
    {
        Task<string> GetForecast(string location, int days);

    }
}
