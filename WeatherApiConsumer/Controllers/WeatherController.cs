using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeatherApiConsumer.Model;
using WeatherApiConsumer.Services;

namespace WeatherApiConsumer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        [Route("[Action]/{cityName}")]
        [HttpGet]
        public async Task<ActionResult<Weathers>> GetByCityName(string cityName,[FromServices] IWeatherServices wsvc) =>await wsvc.GetWeatherResults(cityName, true);

        [Route("[Action]/{zipCode}")]
        [HttpGet]
        public async Task<ActionResult<Weathers>> GetByZipCode(string zipCode, [FromServices] IWeatherServices wsvc) => await wsvc.GetWeatherResults(zipCode, false);

    }
}