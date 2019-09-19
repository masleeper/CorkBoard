using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CorkBoard.UnitTests;

namespace CorkBoard
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // uncomment everything below to use the debug windows
/*            MainWindow mw = new MainWindow();
            string cmd = "";
            Console.Out.WriteLine("This is a debug window for the application.");
            Console.Out.WriteLine("Available commands are:");
            Console.Out.WriteLine("TEST ALL\nTest NET\nTEST WEATHER\nSHOW WEATHER [<station code>]\nNETGET <URL>");
            Console.Out.Write(">");
            cmd = Console.In.ReadLine();
            while (false)
            {
                if (cmd.ToLower().Equals("exit"))
                {
                    Environment.Exit(0);
                }
                if (cmd.ToLower().Equals("test all"))
                {
                    Test.runTests();
                }
                if (cmd.ToLower().Equals("test net"))
                {
                    new NetworkTests().runNetworkTests();
                }
                if (cmd.ToLower().Equals("test weather"))
                {
                    new NetworkTests().runWeatherTests();
                }
                if (cmd.ToLower().Contains("show weather"))
                {
                    string location = "";
                    if (cmd.Split(' ').Length < 3)
                    {
                        location = "KLAF";
                    }
                    else
                    {
                        location = cmd.ToLower().Split(' ')[2].ToUpper();
                    }

                    Network.Weather w = new Network.Weather();
                    Network.Weather.WeatherInfo wi = w.getWeather("https://api.weather.gov/stations/" + location + "/observations?limit=1");
                    Console.WriteLine("Location: " + location);
                    if (wi.temp == -12345)
                    {
                        Console.Out.WriteLine("INVALID LOCATION");
                    }
                    else
                    {
                        Console.Out.WriteLine("Temperature: " + wi.temp);
                        Console.Out.WriteLine("Humidity: " + wi.humidity);
                        Console.Out.WriteLine("Wind Direction: " + wi.windDirection);
                        Console.Out.WriteLine("Wind Speed: " + wi.windSpeed);
                    }

                }
                if (cmd.ToLower().Contains("netget"))
                {
                    if (cmd.Split(' ').Length == 2)
                    {
                        string url = cmd.Split(' ')[1];
                        Console.WriteLine(new Network.Net().getWebText(url));
                    }
                    else
                    {
                        Console.Out.WriteLine("INVALID SYNTAX");
                    }
                }

                Console.Out.Write(">");
              cmd = Console.In.ReadLine();
            } */
        }

    }
}
