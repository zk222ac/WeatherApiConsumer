using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace WeatherApiConsumer.Model
{
    /// <summary>
    /// Each property annotated with corresponding JSON key of target API
    /// This prevents breaking APIs while refactoring because property name is not affecting generated response JSON
    /// </summary>
    public class OpenWeatherForecastModel
    {
        [JsonProperty("cod")]
        internal string Cod { get; set; }

        [JsonProperty("message")]
        internal string Message { get; set; }

        [JsonProperty("cnt")]
        internal int Count { get; set; }
        /// <summary>
        /// Eases to fill date combobox
        /// </summary>
        [JsonProperty("includedDates")]
        internal List<DateTime> IncludedDates =>
            WeatherDatas.GroupBy(x => x.HumanReadableTime.Date).Select(x => x.Key).ToList();

        [JsonProperty("list")]
        internal List<WeatherData> WeatherDatas { get; set; }

        [JsonProperty("city")]
        internal City City { get; set; }

        [JsonProperty("queryTime")]
        internal DateTime QueryTime => DateTime.Now;

        [JsonProperty("queryTerm")]
        internal string QueryTerm { get; set; }
        /// <summary>
        /// Returns a list of average results of temperature, humidity and wind speed for all hour spans per day.
        /// It's grouped by easy-formatted date string so eases to check and filter relevant results on UI.
        /// </summary>
        [JsonProperty("aggregatedResults")]
        internal List<AggregatedResult> AggregatedResults
        {
            get
            {
                var aggregatedResults = WeatherDatas.GroupBy(x => x.HumanReadableTime.Date.ToString("dd.MM.yyyy"),
                    (date, weatherDatas) =>
                    {
                        // Due to weatherDatas is an IEnumerable; in order to avoid using Average with same multiple IEnumerables
                        // .ToList() evaluates current weatherDatas while .Average() does this for triple times.
                        // Maybe not so important in our case but while working with DB or heavy-weight collection, this causes considerable performance problems
                        var weatherDataList = weatherDatas.ToList();
                        return new AggregatedResult
                        {
                            GroupedDate = date,
                            AggreagatedWeatherData = new AggregatedWeatherData
                            {
                                AggregatedHumidity = Math.Round(weatherDataList.Average(x => x.MainData.Humidity), 2),
                                AggregatedWindSpeed = Math.Round(weatherDataList.Average(x => x.Wind.Speed), 2),
                                AggregatedTemp = Math.Round(weatherDataList.Average(x => x.MainData.Temperature), 2)
                            }
                        };
                    }).ToList();

                return aggregatedResults;
            }
        }
    }

    internal class WeatherData
    {
        [JsonProperty("dt")]
        internal uint EpochTime { get; set; }
        /// <summary>
        /// Converts UNIX epoch time to ease usage of Moment
        /// Also provides readable date in order to debugging purposes
        /// </summary>
        [JsonProperty("hrt")]
        internal DateTime HumanReadableTime
        {
            get
            {
                var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(EpochTime).ToLocalTime();
                return dtDateTime;
            }
        }

        [JsonProperty("main")]
        internal MainData MainData { get; set; }

        [JsonProperty("wind")]
        internal Wind Wind { get; set; }

        [JsonProperty("weather")]
        internal List<WeatherDetail> WeatherDetail { get; set; }
    }

    internal class WeatherDetail
    {
        [JsonProperty("id")]
        internal uint WeatherId { get; set; }

        [JsonProperty("main")]
        public string MainDesc { get; set; }

        [JsonProperty("description")]
        public string DetailedDesc { get; set; }

        [JsonProperty("icon")]
        public string IconCode { get; set; }
        /// <summary>
        /// String interpolation to ease getting weather image of current weather condition on UI
        /// </summary>
        [JsonProperty("iconUrl")]
        public string IconUrl => $"http://openweathermap.org/img/w/{IconCode}.png";
    }

    internal class MainData
    {
        [JsonProperty("temp")]
        internal decimal Temperature { get; set; }

        [JsonProperty("temp_min")]
        internal decimal TemperatureMin { get; set; }

        [JsonProperty("temp_max")]
        internal decimal TemperatureMax { get; set; }

        [JsonProperty("humidity")]
        internal decimal Humidity { get; set; }
    }

    internal class Wind
    {
        private static readonly string[] Cardinals = { "N", "NE", "E", "SE", "S", "SW", "W", "NW", "N" };

        [JsonProperty("speed")]
        internal decimal Speed { get; set; }

        // Wind direction, degrees (meteorological)
        [JsonProperty("deg")]
        internal float Degree { get; set; }
        /// <summary>
        /// Converts wind angle degree to cardinals
        /// </summary>
        [JsonProperty("cardinal")]
        internal string Cardinal => Cardinals[(int)Math.Round((double)Degree % 360 / 45)];
    }

    internal class City
    {
        [JsonProperty("name")]
        internal string CityName { get; set; }
        [JsonProperty("country")]
        internal string CountryCode { get; set; }
    }

    internal class AggregatedResult
    {
        [JsonProperty("keyDate")]
        internal string GroupedDate { get; set; }
        [JsonProperty("aggreagatedWeatherData")]
        internal AggregatedWeatherData AggreagatedWeatherData { get; set; }
    }

    internal class AggregatedWeatherData
    {
        [JsonProperty("aggregatedTemp")]
        internal decimal AggregatedTemp { get; set; }
        [JsonProperty("aggregatedWindSpeed")]
        internal decimal AggregatedWindSpeed { get; set; }
        [JsonProperty("aggregatedHumidity")]
        internal decimal AggregatedHumidity { get; set; }
    }
}