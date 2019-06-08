using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WeatherApiConsumer.Model;

namespace WeatherApiConsumer.Services
{
    public class WeatherService :IWeatherServices
    {
        private readonly IOptions<AppConfig> _appconfig;
        private readonly IHttpClientFactory _httpClientFactory;

        // Dependency Injection : Inject the services
        public WeatherService(IOptions<AppConfig> appConfig, IHttpClientFactory httpClientFactory)
        {
            _appconfig = appConfig;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<Weathers> GetWeatherResults(string cityName, bool isCityName)
        {
            var client = _httpClientFactory.CreateClient();
            string path = _appconfig.Value.BuildUrl(cityName, isCityName);
            var response = await client.GetStringAsync(path);
            var retVal = JsonConvert.DeserializeObject<Weathers>(response);
            retVal.QueryTerm = cityName;
            return retVal;
        }
    }
}
