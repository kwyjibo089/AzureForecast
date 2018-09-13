using System.Collections.Specialized;
using System.Configuration;
using System.Drawing;
using System.Net;
using System.Threading;

namespace WeatherForecastFunction.Utils
{
    public static class MoodLight
    {
        public static void SetColor(Color color)
        {
            PostRequest(MoodLightFunction.color, $"{color.R.ToString("D3")}{color.G.ToString("D3")}{color.B.ToString("D3")}");
        }

        public static void Rainbow(int delay)
        {
            PostRequest(MoodLightFunction.rainbow, "1");
            Thread.Sleep(delay);
        }

        private static void PostRequest(MoodLightFunction function, string value)
        {
            using (WebClient client = new WebClient())
            {
                byte[] response = client.UploadValues(ConfigurationManager.AppSettings["SparkUrl"] + function, new NameValueCollection()
                   {
                       { "access_token", ConfigurationManager.AppSettings["access_token"] },
                       { "parms", value }
                   });
            }
        }

    }
}
