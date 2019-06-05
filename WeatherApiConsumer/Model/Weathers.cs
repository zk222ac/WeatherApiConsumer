using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace WeatherApiConsumer.Model
{
    public class Weathers
    {
        [JsonProperty("cod")]
        internal string Cod { get; set; }

        [JsonProperty("message")]
        internal string Message { get; set; }

        [JsonProperty("cnt")]
        internal int Count { get; set; }
        [JsonProperty("CurrentTime")]
        // Current Date and time
        internal DateTime CurrentDate => DateTime.Now;

        [JsonProperty("queryTerm")]
        internal string Query { get; set; }

        [JsonProperty("list")]
        //List of weatherData  
         internal List<WeatherData> WeatherDatas { get; set; }
        /// Eases to fill date combobox
        /// we need Dates as a name and Id as a key
         [JsonProperty("includedDates")]
         internal List<DateTime> IncludedDates => WeatherDatas.GroupBy(x => x.Hrt.Date).Select(x => x.Key).ToList();

        [JsonProperty("aggregatedResults")]
        internal List<AggregatedResult> AggregatedResults
        {
            get
            {
                var avgResults = WeatherDatas.GroupBy(x => x.Hrt.Date.ToString("dd.MM.yyyy"),
                    (d, wt) =>
                    {
                        // d --> date
                        // wt -> weather data
                        var weatherDataList = wt.ToList();
                        return new AggregatedResult
                        {
                            GrpDate = d,
                            AvgWeatherData = new AvgWeatherData()
                            {
                                AvgHumidity = Math.Round(weatherDataList.Average(x => x.MainData.Humidity), 2),
                                AvgWindSpeed = Math.Round(weatherDataList.Average(x => x.Wind.Speed), 2),
                                AvgTemp = Math.Round(weatherDataList.Average(x => x.MainData.Temp), 2)
                            }
                        };
                    }).ToList();

                return avgResults;
            }
        }

    }


    public class WeatherData
    {
        // #region Properties ..................................

        [JsonProperty("dt")]
        // Unix epoch time
        internal uint ETime { get; set; }
        // Converts UNIX epoch time to ease usage of Moment
        // Also provides readable date in order to debugging purposes
        [JsonProperty("hrt")]
        // hrt --> human understandable time
        internal DateTime Hrt
        {
            get
            {
                // Coordinated universal time 
                var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dt = dt.AddSeconds(ETime).ToLocalTime();
                return dt;
            }
        }
        [JsonProperty("main")]
        internal MainData MainData { get; set; }

        [JsonProperty("wind")]
        internal Wind Wind { get; set; }

        
        //#endregion
    }

    #region Model Classes ................................................
        internal class MainData
        {
            [JsonProperty("temp")]
            internal decimal Temp { get; set; }

            [JsonProperty("temp_min")]
            internal decimal TempMin { get; set; }

            [JsonProperty("temp_max")]
            internal decimal TempMax { get; set; }

            [JsonProperty("humidity")]
            internal decimal Humidity { get; set; }
        }
        // since we have limited requirement to fill the properties
        internal class City
        {
            [JsonProperty("name")]
            internal string CityName { get; set; }
            [JsonProperty("country")]
            internal string CountryCode { get; set; }
        }
        internal class AvgWeatherData
        {
            [JsonProperty("avgTemp")]
            internal decimal AvgTemp { get; set; }
            [JsonProperty("avgWindSpeed")]
            internal decimal AvgWindSpeed { get; set; }
            [JsonProperty("avgHumidity")]
            internal decimal AvgHumidity { get; set; }
        }
        internal class AggregatedResult
        {
            [JsonProperty("grpDate")]
            internal string GrpDate { get; set; }
            [JsonProperty("avgWeatherData")]
            internal AvgWeatherData AvgWeatherData { get; set; }
        }
        internal class Wind
        {
            [JsonProperty("speed")]
            internal decimal Speed { get; set; }
        }

        #endregion
}
