using System.Text;
using AIChatBot.Enums;
using AIChatBot.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AIChatBot.Services
{
    public class AiService: IAiService
    {

        private static string apiKey;
        private static string chatGPTUrl;
        private static HttpClient client = new HttpClient();
        private static JArray messages = new JArray();
        private readonly IFunctionDefinitionService _functionDefinitionService;
        private readonly IWeatherService _weatherService;
        private readonly ICurrencyService _currencyService;
        private readonly ITimeZoneService _timeZoneService;
        private string aiSystemContext;

        public AiService(IFunctionDefinitionService functionDefinitionService, IWeatherService weatherService, ICurrencyService currencyService, ITimeZoneService timeZoneService)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Configuration/appsettings.json");

            var config = builder.Build();
            apiKey = config["OpenAiSecretKey"];
            chatGPTUrl = config["chatGPTUrl"];

            _functionDefinitionService = functionDefinitionService;
            _weatherService = weatherService;
            _currencyService = currencyService;
            _timeZoneService = timeZoneService;

            var contextFilePath = Path.Combine(Directory.GetCurrentDirectory(), "TextsForAI", "SystemContextAI.txt");
            
            aiSystemContext = File.ReadAllText(contextFilePath, Encoding.UTF8);

        }

        public void resetChatMessages()
        {
            Console.WriteLine("Clearing chat messages");
            messages.Clear();
        }

        private static async Task<String> SendChatCompletionRequest(string model, object messages, object functions = null)
        {
            var json = new
            {
                model = model,
                messages = messages,
                functions = functions,
            };

            var jsonString = JsonConvert.SerializeObject(json, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            var data = new StringContent(jsonString, Encoding.UTF8, "application/json");

            Console.WriteLine(jsonString);

            try
            {
                client.DefaultRequestHeaders.Remove("Authorization");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                var response = await client.PostAsync(chatGPTUrl, data);
                var result = response.Content.ReadAsStringAsync().Result;

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error when trying to return message from AI: " + ex);
                throw;
            }
        }

        public async Task<string> StartChat(string question)
        {
            if (!messages.Any(m => m["role"]?.ToString() == ChatGptRolesEnum.system.ToString()))
            {
                messages.Add(JToken.FromObject(new
                {
                    role = ChatGptRolesEnum.system.ToString(),
                    content = aiSystemContext
                }));
            }

            messages.Add(JToken.FromObject(new { role = ChatGptRolesEnum.user.ToString(), content = question }));

            var result = string.Empty;
            do
            {
                result = await SendChatCompletionRequest(
                    "gpt-3.5-turbo-1106",
                    messages,
                    _functionDefinitionService.GetFunctionDefs()
                );

                Console.WriteLine(result);

                messages.Add(JObject.Parse(result)["choices"][0]["message"]);

                if (JObject.Parse(result)["choices"][0]["finish_reason"].ToString() == "function_call")
                {
                    string functionCallResult = string.Empty;
                    JObject arguments = JObject.Parse(JObject.Parse(result)["choices"][0]["message"]["function_call"]["arguments"].ToString());
                    string functionName = JObject.Parse(result)["choices"][0]["message"]["function_call"]["name"].ToString();

                    switch (functionName)
                    {
                        case "GetWeatherForecast":
                            Console.WriteLine("Running Get weather forcast");
                            int numOfDays = arguments["NumberOfDays"].ToObject<int>();
                            string location = arguments["Location"].ToObject<string>();
                            functionCallResult = _weatherService.GetForecast(location, numOfDays).Result;
                            break;
                        case "GetCurrencyExchange":
                            Console.WriteLine("Running Get currency exchange");
                            string baseCurrency = arguments["BaseCurrency"].ToObject<string>();
                            string secondaryCurrency = arguments["SecondaryCurrency"].ToObject<string>();
                            double amount = arguments["Amount"].ToObject<double>();
                            functionCallResult = _currencyService.CurrencyExchange(baseCurrency, secondaryCurrency, amount).Result;
                            break;
                        case "GetTimeAndDate":
                            Console.WriteLine("Running Get Time And Date");
                            string ianaZone = arguments["IanaZone"].ToObject<string>();
                            Console.WriteLine("IANA: " + ianaZone);
                            functionCallResult = _timeZoneService.GetDateByIanaZone(ianaZone).Result;
                            break;
                    }

                    Console.WriteLine("Function call result: " + functionCallResult);


                    messages.Add(JToken.FromObject(
                        new
                        {
                            role = ChatGptRolesEnum.function.ToString(),
                            name = functionName,
                            content = functionCallResult
                        }

                     ));

                }
                else
                {
                    break;
                }

            }
            while (true);

            return JObject.Parse(result)["choices"][0]["message"]["content"].ToString();

        }
    }
}
