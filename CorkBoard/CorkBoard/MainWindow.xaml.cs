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
            Test.runTests();

            clktimer = 0;
            wxtimer = 0;
            imgtimer = 0;
            lastWrittenTime = DateTime.Now.ToString("hh:mm tt");
            OuterView.Background = new SolidColorBrush(settings.getOuterColor());
            MainView.Background = new SolidColorBrush(settings.getInnerColor());
            setOuterTextColor(settings.getOuterTextColor());
            setInnerTextColor(settings.getInnerTextColor());

            Weather weather = new Weather();
            Weather.WeatherInfo weatherInfo = weather.getWeather("https://api.weather.gov/stations/KLAF/observations?limit=1");
            updateTemp(weatherInfo.temp);

            //trying to create text boxes for posts 
            List<Announcement> announcements = new GetAnnouncements().getAnnouncements("https://kassarl.github.io/corkboardjson/announcements.json");
            TextBlock[] textBoxes = new TextBlock[announcements.Count];

            for(int i = 0; i < announcements.Count; i++) 
            {
                PostController post = new PostController();
                post.setBody(announcements[i].getBody());
                post.setTitle(announcements[i].getTitle());
                MainView.Children.Add(post); 
            }

            //ImageBox.Source = new BitmapImage(new Uri("../../surprise.PNG", UriKind.Relative));
            ImageBox.Source = new BitmapImage(new Uri(settings.getImgUrl()));
            TimeBlock.Text = DateTime.Now.ToString("h:mm tt");
            DayBlock.Text = DateTime.Now.DayOfWeek.ToString();
            DateBlock.Text = DateTime.Now.ToString("MMMM dd, yyyy", CultureInfo.InvariantCulture);
            Show();

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // 1 second
            timer.Tick += new System.EventHandler(timerTick);
            timer.Start();
        }
        
        private void setOuterTextColor(Color color)
        {
            TimeBlock.Foreground = new SolidColorBrush(color);
            DayBlock.Foreground = new SolidColorBrush(color);
            DateBlock.Foreground = new SolidColorBrush(color);
            TempBlock.Foreground = new SolidColorBrush(color);
        }

        private void setInnerTextColor(Color color)
        {
            // TODO: set all elements in mainview to color
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
                string currentDate = DateTime.Now.ToString("MMMM dd, yyyy", CultureInfo.InvariantCulture);
                if (DateBlock.Text.CompareTo(currentDate) != 0)
                {
                    updateDate(currentDate);
                }
            }

            wxtimer = ++wxtimer % settings.getWeatherRefresh();
            if (wxtimer == 0)
            {
                Weather weather = new Weather();
                Weather.WeatherInfo weatherInfo = weather.getWeather("https://api.weather.gov/stations/KLAF/observations?limit=1");
                updateTemp(weatherInfo.temp);
                Console.WriteLine("Update weather.");
            }

            imgtimer = ++imgtimer % settings.getImgRefresh();
            if (imgtimer == 0)
            {
                ImageBox.Source = new BitmapImage(new Uri(settings.getImgUrl()));
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

        public void updateTemp(int temp)
        {
            if (temp == -12345)
            {
                TempBlock.Text = "N/A";
            } else
            {
                TempBlock.Text = temp.ToString() + "\u00B0F";
            }
            
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
