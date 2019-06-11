using System.Threading.Tasks;
using WeatherApiConsumer.Model;

namespace WeatherApiConsumer.Services
{
    public interface IWeatherServices
    {
        Task<OpenWeatherForecastModel> GetWeatherResults(string cityName, bool isCityName);
    }
}
