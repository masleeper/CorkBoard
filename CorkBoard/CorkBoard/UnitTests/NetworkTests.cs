using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorkBoard.Network;

namespace CorkBoard.UnitTests
{
    class NetworkTests
    {
        public void runNetworkTests()
        {
            Console.WriteLine("------ Starting Network Tests ------");
            Test.PrintTest("Valid URL: ", testValidCall());
            Test.PrintTest("Invalid URL: ", testInvalidCall());
            Test.PrintTest("Empty URL: ", testEmptyCall());
            Console.WriteLine("-------- Network Tests End -------\n\n");
        }

        public void runWeatherTests()
        {
            Console.WriteLine("----- Starting Weather Tests -----");
            Test.PrintTest("Valid Weather API: ", testValidWeather());
            Test.PrintTest("Invalid Weather API: ", testInvalidWeather());
            Test.PrintTest("Empty Weather API: ", testEmptyWeather());

            Console.WriteLine("------- Weather Tests End -------\n\n");
            
        }

        public void runForecastTests()
        {
            Console.WriteLine("----- Starting Forecast Tests -----");
            Test.PrintTest("Valid Forecast API: ", testValidForecast());
            Test.PrintTest("Invalid Forecast API: ", testInvalidForecast());
            Test.PrintTest("Empty Forecast API: ", testEmptyForecast());

            Console.WriteLine("------- Forecast Tests End -------\n\n");

        }

        //test web section
        #region
        private bool testValidCall()
        {
            string a = new Net().getWebText("https://api.weather.gov/");
            if (!a.Equals("error"))
            {
                return true;
            }
            return false;
        }

        private bool testInvalidCall()
        {
            string a = new Net().getWebText("https://api2.weather.gov/");
            if (a.Equals("error"))
            {
                return true;
            }
            return false;
        }
        private bool testEmptyCall()
        {
            string a = new Net().getWebText("");
            if (a.Equals("error"))
            {
                return true;
            }
            return false;
        }
        #endregion

        //test get weather
        #region
            public bool testValidWeather()
        {
            Weather w = new Weather();
            Weather.WeatherInfo info = w.getWeather("https://api.weather.gov/stations/KSBN/observations?limit=1");
            //if one value is null all is null
            //only need to check 1 value to exist
            if(info.temp != -12345)
            {
                return true;
            }

            return false;
        }
        public bool testInvalidWeather()
        {
            Weather w = new Weather();
            Weather.WeatherInfo info = w.getWeather("https://api.weather.gov/stations/QSBN/observations?limit=1");
            //if one value is null all is null
            //only need to check 1 value to exist
            if (info.temp == -12345)
            {
                return true;
            }

            return false;
        }
        public bool testEmptyWeather()
        {
            Weather w = new Weather();
            Weather.WeatherInfo info = w.getWeather("");
            //if one value is null all is null
            //only need to check 1 value to exist
            if (info.temp == -12345)
            {
                return true;
            }

            return false;
        }
        #endregion

        //test get announcements

        //test get forecast
        #region
        public bool testValidForecast()
        {
            Forecast f = new Forecast();
            List<Forecast.ForecastData> data = f.getForecast("https://api.weather.gov/gridpoints/IND/40,70/forecast?units=us");
            //if one value is null all is null
            //only need to check 1 value to exist
            if (data.Count > 1)
            {
                return true;
            }

            return false;
        }
        public bool testInvalidForecast()
        {
            Forecast f = new Forecast();
            List<Forecast.ForecastData> data = f.getForecast("https://api.weather.gov/gridpoints/INZ/40,70/forecast?units=us");
            //if one value is null all is null
            //only need to check 1 value to exist
            if (data.Count == 0)
            {
                return true;
            }

            return false;
        }
        public bool testEmptyForecast()
        {
            Forecast f = new Forecast();
            List<Forecast.ForecastData> data = f.getForecast("");
            //if one value is null all is null
            //only need to check 1 value to exist
            if (data.Count == 0)
            {
                return true;
            }

            return false;
        }
        #endregion


    }
}
