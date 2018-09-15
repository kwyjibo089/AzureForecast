using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace WeatherForecastFunction.Utils
{
    public class LogEntry : TableEntity
    {
        public LogEntry(string id)
        {
            this.PartitionKey = "WeatherSettings";
            this.RowKey = id.ToString();
            this.Id = id;
            this.TimeStamp = DateTime.Now;
        }

        public LogEntry()
        {

        }

        public String Id { get; set; }
        public String Color { get; set; }
        public bool WillItRain { get; set; }
        private DateTime TimeStamp { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }

    }
}
