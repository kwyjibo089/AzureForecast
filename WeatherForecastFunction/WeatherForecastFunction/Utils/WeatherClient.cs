using OpenWeatherMapSharp;
using OpenWeatherMapSharp.Enums;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecastFunction.Utils
{
    public class WeatherClient
    {
        private readonly OpenWeatherMapService client;

        private readonly string webApiKey;
        private readonly int cityId;
        private readonly int forecastHours;

        public WeatherClient()
        {
            // store these in local.settings.json or in Azure
            webApiKey = ConfigurationManager.AppSettings["OpenWeatherMapApiKey"];
            cityId = int.Parse(ConfigurationManager.AppSettings["CityId"]);
            forecastHours = int.Parse(ConfigurationManager.AppSettings["ForecastHours"]);

            client = new OpenWeatherMapService(webApiKey);
        }

        public async Task<bool> WillItRainAsync()
        {
            var forecast = await client.GetForecastAsync(cityId, LanguageCode.DE, Unit.Metric);

            if (forecast.IsSuccess)
            {
                if (forecast.Response.Items.Where(i => DateTime.Parse(i.Date) < DateTime.Now.AddHours(forecastHours))
                    .Any(f => f.Weather.First().Main.ToUpperInvariant().Contains("RAIN")))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
