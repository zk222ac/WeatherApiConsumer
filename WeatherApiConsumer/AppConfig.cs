using System.Net;

namespace WeatherApiConsumer
{
    public class AppConfig
    {
        public AppConfig()
        {
            
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
                //return $"{ApiUrlBody}?zip={param},de&mode={ResponseMode}&appid={ApiKey}&units={UnitType}";
                 return $"{ApiUrlBody}?zip={param}&appid={ApiKey}&units={UnitType}";

            }

        }
    }
}
