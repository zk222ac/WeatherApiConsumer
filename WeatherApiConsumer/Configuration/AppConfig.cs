using System.Net;

namespace WeatherApiConsumer.Configuration
{
    /// <summary>
    /// Wraps AppConfig section at appsettings.json
    /// IOptions<AppConfig> injected controllers can easily access all below properties and method.
    /// </summary>
    public class AppConfig
    {
        public AppConfig()
        {
            // Lazy loading can be observed by placing a break-point here.
        }
        public string ApiKey { get; set; }
        public string ApiUrlBody { get; set; }
        public string ResponseMode { get; set; }
        public string UnitType { get; set; }

        public string BuildUrl(string param, bool isCityName)
        {
            var urlEncodedCity = WebUtility.UrlEncode(param);
            if (isCityName)
            {
                return $"{ApiUrlBody}?q={urlEncodedCity}&mode={ResponseMode}&appid={ApiKey}&units={UnitType}";
            }
            else
            {
                return $"{ApiUrlBody}?zip={param},de&mode={ResponseMode}&appid={ApiKey}&units={UnitType}";
            }

        }
    }
}
