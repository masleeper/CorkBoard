using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using CorkBoard.Core;

namespace CorkBoard.Network
{
    class Weather
    {
        public struct WeatherInfo
        {
            public int temp;
            public int humidity;
            public int windDirection;
            public int windSpeed;
            public string msg;
        }

        public WeatherInfo getWeather(string apiCall)
        {
            WeatherInfo info;
            info.temp = -12345; //value for checking errors
            info.humidity = -12345; //value for checking error
            info.windDirection = -12345;
            info.windSpeed = -12345;
            info.msg = "-12345";
            if(apiCall == null || apiCall == "")
            {
                return info;
            }
            string text = new Net().getWebText(apiCall);
            dynamic wJson = HelperFunctions.convertToDynamic(text);

            try
            {
                //check properties exist
                if (wJson == null ||
                    wJson["features"] == null ||
                    wJson["features"][0] == null ||
                    wJson["features"][0]["properties"] == null)
                {
                    return info;
                }
                else
                {
                    wJson = (dynamic)wJson["features"][0]["properties"];
                }

                //check weather properties exist
                if (wJson["temperature"] == null || wJson["temperature"]["value"] == null ||
                    wJson["relativeHumidity"] == null || wJson["relativeHumidity"]["value"] == null ||
                    wJson["textDescription"] == null || wJson["textDescription"] == null ||
                    wJson["windDirection"] == null || wJson["windDirection"]["value"] == null ||
                    wJson["windSpeed"] == null || wJson["windSpeed"]["value"] == null)
                {
                    return info;
                }
                else
                {
                    info.temp = (int)((float)wJson["temperature"]["value"] * 1.8) + 32;
                    info.humidity = (int)wJson["relativeHumidity"]["value"];
                    info.msg = (String)wJson["textDescription"];
                    info.windDirection = (int)wJson["windDirection"]["value"];
                    info.windSpeed = (int)wJson["windSpeed"]["value"];

                }
            }catch (Exception)
            {
                return info;
            }



            return info;
        }
    }
}
