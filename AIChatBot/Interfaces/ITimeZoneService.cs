namespace AIChatBot.Interfaces
{
    public interface ITimeZoneService
    {
        Task<string> GetDateByIanaZone(string iana);
    }
}
