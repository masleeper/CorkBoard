using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CorkBoard.UnitTests;
using CorkBoard.Network;
using CorkBoard.Core;
using System.Diagnostics;

namespace CorkBoard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
           

            Weather weather = new Weather();
            Weather.WeatherInfo weatherInfo = weather.getWeather("https://api.weather.gov/stations/KLAF/observations?limit=1");
            TextBlock weatherText = new TextBlock();
            if (weatherInfo.temp == -12345)
            {
                weatherText.Text = "Can't get weather data at this time.";
            }
            else
            {
                weatherText.Text = "temp: " + weatherInfo.temp + " humidity: " + weatherInfo.humidity;
            }

            IniFile settings = new IniFile("C:\\Users\\Me\\Documents\\cs408\\CorkBoard\\CorkBoard\\CorkBoard\\settings.ini");
            Debug.WriteLine("userimg path: " + settings.Read("userimg"));
            ImageBox.Source = new BitmapImage(new Uri("../../surprise.PNG", UriKind.Relative));
            WeatherPanel.Children.Add(weatherText);

            TextBlock timeText = new TextBlock();
            timeText.Text = DateTime.Now.ToString("h:mm tt");
            timeText.FontSize = 30;
            TimePanel.Children.Add(timeText);
            Show();
        }
    }
}
