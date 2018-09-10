using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherForecastFunction.Utils;

namespace WeatherForecastFunction
{
    public static class ForecastFunction
    {
        [FunctionName("Function1")]
        public static async Task Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");

            using (WebClient client = new WebClient())
            {
                var function = "color";
                var uri = string.Format(ConfigurationManager.AppSettings["SparkUri"], function);
                var baseUrl = new Uri(ConfigurationManager.AppSettings["SparkBaseUrl"]);

                byte[] response = client.UploadValues(baseUrl + uri, new NameValueCollection()
                {
                    { "access_token", "6ee01ca181ed80a6ba832b4efba366bb7f0d92f6" },
                    { "parms", "000255000" }
                });
            }

            //using (var client = new HttpClient())
            //{
            //    var function = "color";
            //    var uri = string.Format(ConfigurationManager.AppSettings["SparkUri"], function);
            //    var baseUrl = new Uri(ConfigurationManager.AppSettings["SparkBaseUrl"]);


            //    log.Info(resultContent);
            //}

            //var forecastClient = new WeatherClient();
            //if (await forecastClient.WillItRainAsync())
            //{
            //    using (var client = new HttpClient())
            //    {
            //        var function = "color";
            //        var uri = string.Format(ConfigurationManager.AppSettings["SparkUri"], function);
            //        client.BaseAddress = new Uri(ConfigurationManager.AppSettings["SparkBaseUrl"]);
            //        var body = new { EntityState = new { @params = "000000255" } };

            //        var result = await client.PostAsJsonAsync(uri, body);
            //        string resultContent = await result.Content.ReadAsStringAsync();

            //        log.Info(resultContent);
            //    }
            //}
        }
    }
}
