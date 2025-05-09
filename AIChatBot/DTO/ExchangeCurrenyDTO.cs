namespace AIChatBot.DTO
{
    public class ExchangeCurrenyDTO
    {
        public string BaseCurrency { get; set; }
        public string? SecondaryCurrency { get; set; }
        public double ConversionRate { get; set; }
        public double? ConversionResult { get; set; }
    }
}