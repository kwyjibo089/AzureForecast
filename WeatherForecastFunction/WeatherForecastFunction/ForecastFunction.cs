using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
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
            
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                CloudTable table = tableClient.GetTableReference("SettingsTable");
                table.CreateIfNotExists();

                TableOperation retrieve = TableOperation.Retrieve<LogEntry>("WeatherSettings", ConfigurationManager.AppSettings["WeatherSettingId"]);
                var lastLog = (LogEntry)table.Execute(retrieve).Result;

                var forecastClient = new WeatherClient();
                var willItRain = await forecastClient.WillItRainAsync();

                var logEntry = new LogEntry(ConfigurationManager.AppSettings["WeatherSettingId"]);
                
                if (lastLog != null && lastLog.WillItRain == willItRain)
                {
                    logEntry.Message = "No change detected";
                    TableOperation update = TableOperation.InsertOrReplace(logEntry);
                    table.Execute(update);

                    return;
                }


                if (willItRain)
                {
                    MoodLight.Rainbow(5000);
                    MoodLight.SetColor(Color.Blue);

                    logEntry.Color = Color.Blue;
                    logEntry.WillItRain = true;
                }
                else
                {
                    try
                    {
                        var colorString = ConfigurationManager.AppSettings["DefaultColor"];
                        MoodLight.Rainbow(5000);
                        MoodLight.SetColor(Color.FromName(colorString));
                        logEntry.Color = Color.FromName(colorString);
                    }
                    catch (Exception e)
                    {
                        MoodLight.SetColor(Color.Red);
                        logEntry.Color = Color.Red;
                        logEntry.Message = e.Message;
                    }
                    finally
                    {
                        logEntry.WillItRain = false;
                    }
                }

                TableOperation insert = TableOperation.InsertOrReplace(logEntry);
                table.Execute(insert);
            }
            catch (Exception e)
            {
                log.Error("Exception occured",  e);                
            }
        }
    }
}
