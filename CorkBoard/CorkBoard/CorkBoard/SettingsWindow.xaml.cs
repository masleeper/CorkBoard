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
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using CorkBoard.Core;

namespace CorkBoard
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private Settings mainSettings;
        private MainWindow mainWindow;

        public SettingsWindow(Settings mainSettings, MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainSettings = mainSettings;
            this.mainWindow = mainWindow;

            if (mainSettings.isTimeVisible())
            {
                showTime.IsChecked = true;
            } else
            {
                hideTime.IsChecked = true;
            }

            if (mainSettings.isDateVisible())
            {
                showDate.IsChecked = true;
            } else
            {
                hideTime.IsChecked = true;
            }

            if (mainSettings.isWeatherVisible())
            {
                showWeather.IsChecked = true;
            } else
            {
                hideWeather.IsChecked = true;
            }
            
            if (mainSettings.isImageVisible())
            {
                showImage.IsChecked = true;
            } else
            {
                hideImage.IsChecked = true;
            }

            OuterBg.Text = mainSettings.getOuterColor().ToString();
            InnerBg.Text = mainSettings.getInnerColor().ToString();
            InnerText.Text = mainSettings.getInnerTextColor().ToString();
            OuterText.Text = mainSettings.getOuterTextColor().ToString();
        }

        public void setMainSettings(Settings mainSettings)
        {
            this.mainSettings = mainSettings;
        }

        public void setMainWindow(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public void saveSettings(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void saveSettings()
        {
            mainSettings.setTimeVisible((bool)showTime.IsChecked);
            mainSettings.setDateVisible((bool)showDate.IsChecked);
            mainSettings.setWeatherVisible((bool)showWeather.IsChecked);
            mainSettings.setImageVisible((bool)showImage.IsChecked);
            Regex colorRegex = new Regex("#[0-9A-Fa-f]{8}$");

            Match m = colorRegex.Match(OuterBg.Text.Trim());
            if (m.Success)
            {
                mainSettings.setOuterColor(OuterBg.Text.Trim());
            }

            m = colorRegex.Match(InnerBg.Text.Trim());
            if (m.Success)
            {
                mainSettings.setInnerColor(InnerBg.Text.Trim());
            }

            m = colorRegex.Match(OuterText.Text.Trim());
            if (m.Success) {
                mainSettings.setOuterTextColor(OuterText.Text.Trim());
            }

            m = colorRegex.Match(InnerText.Text.Trim());
            if (m.Success)
            {
                mainSettings.setInnerTextColor(InnerText.Text.Trim());
            }
        }

        public void settingsClose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            saveSettings();
            mainWindow.updateSettings(mainSettings);
        }
    }
}
