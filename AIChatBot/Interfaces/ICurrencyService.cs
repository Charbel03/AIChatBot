namespace AIChatBot.Interfaces
{
    public interface ICurrencyService
    {
        Task<string> CurrencyExchange(string baseCurrency, string diffCurrency, double amount);

    }
}
