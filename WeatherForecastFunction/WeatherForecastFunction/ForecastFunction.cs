using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Drawing;
using System.Net;
using System.Threading;
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
                Rainbow(5000);
                SetColor(Color.Blue);
            }

            try
            {
                var colorString = ConfigurationManager.AppSettings["DefaultColor"];
                SetColor(Color.FromName(colorString));
            }
            catch (Exception)
            {
                SetColor(Color.Red);
            }
        }

        private static void SetColor(Color color)
        {
            using (WebClient client = new WebClient())
            {
                var function = "color";
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["SparkUrl"] + function, new NameValueCollection()
                   {
                       { "access_token", ConfigurationManager.AppSettings["access_token"] },
                       { "parms", $"{color.R}{color.B}{color.G}" }
                   });
            }
        }

        private static void Rainbow(int delay)
        {
            using (WebClient client = new WebClient())
            {
                var function = "rainbow";
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["SparkUrl"] + function, new NameValueCollection()
                   {
                       { "access_token", ConfigurationManager.AppSettings["access_token"] },
                       { "parms", "1" }
                   });
            }

            Thread.Sleep(delay);
        }
    }
}
