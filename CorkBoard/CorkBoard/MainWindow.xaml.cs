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
using System.Globalization;

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
            if (weatherInfo.temp == -12345)
            {
                TempBlock.Text = "N/A";
            }
            else
            {
                TempBlock.Text = weatherInfo.temp + "\u00B0F";
            }

           
            ImageBox.Source = new BitmapImage(new Uri("../../surprise.PNG", UriKind.Relative));
            TimeBlock.Text = DateTime.Now.ToString("h:mm tt");
            DayBlock.Text = DateTime.Now.DayOfWeek.ToString();
            DateBlock.Text = DateTime.Now.ToString("MMMM dd,yyyy", CultureInfo.InvariantCulture);
            Show();
        }
    }
}
