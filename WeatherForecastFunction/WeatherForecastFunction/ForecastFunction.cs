using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Configuration;
using System.Drawing;
using System.Threading.Tasks;
using WeatherForecastFunction.Utils;

namespace WeatherForecastFunction
{
    public static class ForecastFunction
    {
        [FunctionName("Function1")]
        public static async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");

            var forecastClient = new WeatherClient();
            if (await forecastClient.WillItRainAsync())
            {
                MoodLight.Rainbow(5000);
                MoodLight.SetColor(Color.Blue);
            }
            else
            {
                try
                {
                    var colorString = ConfigurationManager.AppSettings["DefaultColor"];
                    MoodLight.SetColor(Color.FromName(colorString));
                }
                catch (Exception)
                {
                    MoodLight.SetColor(Color.Red);
                }
            }
        }
    }
}
