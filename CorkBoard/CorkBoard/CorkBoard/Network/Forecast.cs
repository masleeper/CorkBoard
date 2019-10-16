using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorkBoard.Core;

namespace CorkBoard.Network
{
    public class Forecast
    {
        public struct ForecastData
        {
            public string period;
            public string windDirection;
            public string windSpeed;
            public int temperature;
            public string cloud;
        }

        public Forecast()
        {

        }

        public List<ForecastData> getForecast(string zone)
        {
            List<ForecastData> data = new List<ForecastData>();

            string URL = "https://api.weather.gov/gridpoints/" + zone + "/40,70/forecast?units=us";
            if (zone == null || zone == "")
            {
                return data;
            }
            

            try
            {
                string webText = new Net().getWebText(URL);
                dynamic wJson = HelperFunctions.convertToDynamic(webText);
                if (wJson != null &&
                    wJson["properties"] != null &&
                    wJson["properties"]["periods"] != null)
                {
                    int i = 0;
                    while(wJson["properties"]["periods"][i] != null)
                    {
                        ForecastData data1;
                        data1.period = wJson["properties"]["periods"][i]["name"] != null ? (string)wJson["properties"]["periods"][i]["name"] : "";
                        if(data1.period == "")
                        {
                            break;
                        }
                        data1.temperature = wJson["properties"]["periods"][i]["temperature"] != null ? (int)wJson["properties"]["periods"][i]["temperature"] : -12345;
                        data1.windSpeed = wJson["properties"]["periods"][i]["windSpeed"] != null ? (string)wJson["properties"]["periods"][i]["windSpeed"] : "0 mph";
                        data1.windDirection = wJson["properties"]["periods"][i]["windDirection"] != null? (string)wJson["properties"]["periods"][i]["windDirection"] : "NO WIND";
                        data1.cloud = wJson["properties"]["periods"][i]["shortForecast"] != null ? (string)wJson["properties"]["periods"][i]["shortForecast"] : "NULL";
                        data.Add(data1);

                        i++;
                    }
                }



            }catch (Exception)
            {
                
            }
            return data;
        }

    }
}
