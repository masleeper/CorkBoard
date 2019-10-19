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
        private int newtimer;
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
            newtimer = 0;
            lastWrittenTime = DateTime.Now.ToString("hh:mm tt");

            updateUI();
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
                Forecast forecast = new Forecast();
                List<Forecast.ForecastData> data = forecast.getForecast("https://api.weather.gov/gridpoints/IND/40,70/forecast?units=us");
                updateTemp(weatherInfo.temp);
                updateForecast(data);
                Console.WriteLine("Update weather.");
            }

            imgtimer = ++imgtimer % settings.getImgRefresh();
            if (imgtimer == 0)
            {
                ImageBox.Source = new BitmapImage(new Uri(settings.getImgUrl()));
                Console.WriteLine("Update image.");
            }

            newtimer = ++newtimer % settings.getNewsRefresh();
            if (newtimer == 0)
            {
                News news = new News();
                string[] list = new string[5];
                int count = settings.getNewsCount();

                if (count == null | count <= 0)
                {
                }
                else
                {
                    string url = "https://newsapi.org/v2/top-headlines?" + settings.getNewsSource() + "apiKey=5969a901e08f42c7a532e0d93a039ffa";
                    list = news.getNews(count, url);
                    updateNews(count, list);
                    Console.WriteLine("Update News.");
                }
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

        public void updateNews(int count, string[] list)
        {
            NewsBlock.Children.Clear();
            TextBlock NewsTitle = new TextBlock();
            NewsTitle.Text = "News";
            NewsTitle.FontSize = 60;
            NewsTitle.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            NewsTitle.VerticalAlignment = 0;
            NewsTitle.Foreground = Brushes.White;
            NewsBlock.Children.Add(NewsTitle);
            string NewsString = "";
            for (int a = 0; a < count; a++)
            {
                Console.WriteLine(list[a]);
                NewsString = NewsString + list[a];
                NewsString = NewsString + "\n\n\n";
            }

            NewsString = NewsString.Replace("â€\u2122", "'");
            NewsString = NewsString.Replace("â€˜", "'");

            TextBlock NewsText = new TextBlock();
            NewsText.Text = NewsString;
            NewsText.FontSize = 10;
            NewsText.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            NewsText.Foreground = Brushes.White;
            NewsText.TextWrapping = TextWrapping.Wrap;
            NewsBlock.Children.Add(NewsText);

        }

        public void updateForecast(List<Forecast.ForecastData> data)
        {
            ForecastView.Children.Clear();
           
            int numShown = 0;
            for (int i = 0; i < data.Count; i++)
            {
                Forecast.ForecastData fdata = data[i];
                if (numShown == 3)
                {
                    break;
                }

                if (!fdata.period.ToLower().Contains("night")) 
                {
                    TextBlock forecastBlock = new TextBlock();
                    forecastBlock.TextWrapping = TextWrapping.Wrap;
                    forecastBlock.FontSize = 28;
                    forecastBlock.Foreground = new SolidColorBrush(settings.getInnerTextColor());
                    forecastBlock.Text = fdata.period + "\n" + fdata.temperature + "\u00B0 | " +
                        data[i + 1].temperature + "\u00B0\n" + fdata.cloud + "\n";
                    ForecastView.Children.Add(forecastBlock);
                    numShown++;
                }
            }
        }

        public void openSettingsView(object sender, RoutedEventArgs e)
        {
            SettingsWindow sw = new SettingsWindow(settings, this);
            sw.Show();
        }

        public void updateSettings(Settings newSettings)
        {
            this.settings = newSettings;
            updateUI();
        }
        
        /*
         * This method is solely to be used on startup and when new settings are loaded in
         */
        public void updateUI()
        {
            OuterView.Background = new SolidColorBrush(settings.getOuterColor());
            MainView.Background = new SolidColorBrush(settings.getInnerColor());
            setOuterTextColor(settings.getOuterTextColor());
            setInnerTextColor(settings.getInnerTextColor());

            if (settings.isWeatherVisible())
            {
                TempBlock.Visibility = Visibility.Visible;
                ForecastView.Visibility = Visibility.Visible;
                Weather weather = new Weather();
                Weather.WeatherInfo weatherInfo = weather.getWeather("https://api.weather.gov/stations/KLAF/observations?limit=1");
                updateTemp(weatherInfo.temp);

                Forecast forecast = new Forecast();
                List<Forecast.ForecastData> data = forecast.getForecast("https://api.weather.gov/gridpoints/IND/40,70/forecast?units=us");
                updateForecast(data);
            } else
            {
                TempBlock.Visibility = Visibility.Collapsed;
                ForecastView.Visibility = Visibility.Collapsed;
            }
            
            if (settings.isImageVisible())
            {
                ImageBox.Visibility = Visibility.Visible;
                ImageBox.Source = new BitmapImage(new Uri(settings.getImgUrl()));
            } else
            {
                ImageBox.Visibility = Visibility.Collapsed;
            }
           
            if (settings.isTimeVisible())
            {
                TimeBlock.Visibility = Visibility.Visible;
                TimeBlock.Text = DateTime.Now.ToString("h:mm tt");
            } else
            {
                TimeBlock.Visibility = Visibility.Collapsed;
            }
            
            if (settings.isDateVisible())
            {
                DayBlock.Visibility = Visibility.Visible;
                DateBlock.Visibility = Visibility.Visible;
                updateDay(DateTime.Now.DayOfWeek.ToString());
                updateDate(DateTime.Now.ToString("MMMM dd, yyyy", CultureInfo.InvariantCulture));
            } else
            {
                DateBlock.Visibility = Visibility.Collapsed;
                DayBlock.Visibility = Visibility.Collapsed;
            }

            if (settings.isNewsVisible())
            {
                News news = new News();
                int count = settings.getNewsCount();
                if (count == null | count <= 0)
                {
                }
                else
                {
                    string[] list = new string[count];
                    string url = "https://newsapi.org/v2/top-headlines?sources=" + settings.getNewsSource() + "&apiKey=5969a901e08f42c7a532e0d93a039ffa";
                    list = news.getNews(count, url);
                    updateNews(count, list);
                }
            }
        }
    }
}
