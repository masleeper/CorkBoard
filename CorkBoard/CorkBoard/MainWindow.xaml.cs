using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
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
        private System.Windows.Forms.Timer timer;
        private int clktimer;
        private int wxtimer;
        private int imgtimer;
        private string lastWrittenTime;
        private Settings settings;

        public MainWindow()
        {
            InitializeComponent();
            settings = new Settings();

            clktimer = 0;
            wxtimer = 0;
            imgtimer = 0;
            lastWrittenTime = DateTime.Now.ToString("hh:mm tt");

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

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // 1 second
            timer.Tick += new System.EventHandler(timerTick);
            timer.Start();
        }


        private void timerTick(object sender, EventArgs e)
        {
            clktimer = ++clktimer % settings.getClockRefresh();
            if (clktimer == 0)
            {
                string currentTime = DateTime.Now.ToString("h:mm tt");
                if (lastWrittenTime.CompareTo(currentTime) != 0)
                {
                    lastWrittenTime = currentTime;
                    updateTime(lastWrittenTime);
                    Console.WriteLine("Update clock to " + lastWrittenTime);
                }
            }

            wxtimer = ++wxtimer % settings.getWeatherRefresh();
            if (wxtimer == 0)
            {
                Console.WriteLine("Update weather.");
            }

            imgtimer = ++imgtimer % settings.getImgRefresh();
            if (imgtimer == 0)
            {
                Console.WriteLine("Update image.");
            }

        }
        public void updateTime(string time)
        {
            TimeBlock.Text = time;
        }

        public void updateDay(string day)
        {
            DayBlock.Text = day;
        }

        public void updateDate(string date)
        {
            DateBlock.Text = date;
        }

        public void updateTemp(string temp)
        {
            TempBlock.Text = temp;
        }
    }
}
