using System.Threading.Tasks;
using WeatherApiConsumer.Model;

namespace WeatherApiConsumer.Services
{
    public interface IWeatherServices
    {
        Task<Weathers> GetWeatherResults(string cityName, bool isCityName);
    }
}
