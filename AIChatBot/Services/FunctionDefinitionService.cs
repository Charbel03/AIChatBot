using AIChatBot.Interfaces;

namespace AIChatBot.Services
{
    public class FunctionDefinitionService: IFunctionDefinitionService
    {
        private Object[] functionDefs;

        public FunctionDefinitionService()
        {
            functionDefs = new object[]
            {
                new
                {
                    name = "GetWeatherForecast",
                    description = "Get the weather forcast for the specified number of days",
                    parameters = new
                    {
                        type = "object",
                        properties = new
                        {
                            NumberOfDays = new
                            {
                                Type = "integer",
                                description = "The number of days to check the weather forcast for"
                            },
                            Location = new
                            {
                                Type = "string",
                                description = "The City or Country to check the weather forcast for."
                            }
                        },
                        required = new[]{"NumberOfDays", "Location"},
                    }
                },
                new
                {
                    name = "GetCurrencyExchange",
                    description = "Get the Conversion rate and conversion result from one currency to a different currency by a specified number amount of money",
                    parameters = new
                    {
                        type = "object",
                        properties = new
                        {
                            BaseCurrency = new
                            {
                                Type = "string",
                                description = "The base currency to convert"
                            },
                            SecondaryCurrency = new
                            {
                                Type = "string",
                                description = "The converted currency"
                            },
                            Amount = new
                            {
                                Type = "double",
                                description = "The specified number amount of money"
                            }
                        },
                        required = new[]{ "BaseCurrency", "SecondaryCurrency", "Amount"},
                    }
                },
                new
                {
                    name = "GetTimeAndDate",
                    description = "Get the current date and time from a city/country via full IANA time zone names",
                    parameters = new
                    {
                        type = "object",
                        properties = new
                        {
                            IanaZone = new
                            {
                                Type = "string",
                                description = "Full IANA time zone name to check the date and time"
                            }
                        },
                        required = new[]{ "IanaZone"},
                    }
                }
            };
        }

        public object[] GetFunctionDefs()
        {
            return functionDefs;
        }
    }
}
