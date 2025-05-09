using System.Text.Json;
using AIChatBot.DTO;
using AIChatBot.Interfaces;

namespace AIChatBot.Services
{
    public class CurrencyService: ICurrencyService
    {

        private readonly HttpClient _httpClient;
        private string _apiKey;
        private string _exchangeUrlUrl;

        public CurrencyService(HttpClient httpClient)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Configuration/appsettings.json");

            var config = builder.Build();
            _apiKey = config["ExchangeApiKey"];
            _exchangeUrlUrl = config["ExchangeUrl"];

            _httpClient = httpClient;
        }

        public async Task<string> CurrencyExchange(string baseCurrency, string diffCurrency, double amount)
        {
            var url = $"{_exchangeUrlUrl}/{_apiKey}/pair/{baseCurrency}/{diffCurrency}/{amount}";

            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            if (!response.IsSuccessStatusCode)
            {
                return "Error: Couldnt fetch the Currency Exchange, try again later.)";
            }

            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var currencyExchangeDTO = new ExchangeCurrenyDTO
            {
                BaseCurrency = root.GetProperty("base_code").GetString(),
                SecondaryCurrency = root.GetProperty("target_code").GetString(),
                ConversionRate = root.GetProperty("conversion_rate").GetDouble(),
                ConversionResult = root.GetProperty("conversion_result").GetDouble()
            };

            var textForAI = $"1 {currencyExchangeDTO.BaseCurrency} = {currencyExchangeDTO.ConversionRate} {currencyExchangeDTO.SecondaryCurrency}. And {amount} {currencyExchangeDTO.BaseCurrency} = {currencyExchangeDTO.ConversionResult} {currencyExchangeDTO.SecondaryCurrency}";
            return textForAI;
        }

    }
}
