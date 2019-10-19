﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Reflection;
using System.Windows.Media;

namespace CorkBoard.Core
{
    public class Settings
    {
        private string wxurl; //Full URL for weather API in our zone. In the future, maybe replace with zip code and structure an appropriate API link on our own?
        private string imgurl; //Full URL for image to display.
        private int wxrefresh = 300; //Time in seconds to refresh weather - Lower for quicker refresh. BEWARE RATE LIMITING.
        private int imgrefresh = 120; //Time in seconds to refresh image - Lower means quicker refresh.
        private int clkrefresh = 5; //Time in seconds to check clock - Lower means higher accuracy but potentially more problems. Not sure this should be a user-defined global, but adding it in for now.
        private int[] btheme = new int[12]; //Contains a basic color scheme, four RGB values. Background1, Text1, Background2, Color2.
        private Color outerColor, outerTextColor, innerColor, innerTextColor;
        private bool timeVisible, dateVisible, weatherVisible, imageVisible;
     
        public Settings()
        {
            outerColor = new Color();
            outerTextColor = new Color();
            innerColor = new Color();
            innerTextColor = new Color();
            getSettings();
            timeVisible = true;
            dateVisible = true;
            weatherVisible = true;
            imageVisible = true;
        }

        public bool isTimeVisible()
        {
            return timeVisible;
        }

        public void setTimeVisible(bool timeVisible)
        {
            this.timeVisible = timeVisible;
        }

        public bool isDateVisible()
        {
            return dateVisible;
        }

        public void setDateVisible(bool dateVisible)
        {
            this.dateVisible = dateVisible;
        }

        public bool isWeatherVisible()
        {
            return weatherVisible;
        }

        public bool isImageVisible()
        {
            return imageVisible;
        }

        public void setImageVisible(bool imageVisible)
        {
            this.imageVisible = imageVisible;
        }

        public void setWeatherVisible(bool weatherVisible)
        {
            this.weatherVisible = weatherVisible;
        }

        public void setWeatherUrl(string url)
        {
            wxurl = url;
        }

        public string getWeatherUrl()
        {
            return wxurl;
        }

        public void setImgUrl(string url)
        {
            imgurl = url;
        }

        public string getImgUrl()
        {
            return imgurl;
        }

        public void setWeatherRefresh(int rate)
        {
            wxrefresh = rate;
        }

        public int getWeatherRefresh()
        {
            return wxrefresh;
        }

        public void setImgRefresh(int rate)
        {
            imgrefresh = rate;
        }

        public int getImgRefresh()
        {
            return imgrefresh;
        }

        public void setClockRefresh(int rate)
        {
            clkrefresh = rate;
        }

        public int getClockRefresh()
        {
            return clkrefresh;
        }

        public Color getOuterColor()
        {
            return outerColor;
        }

        public void setOuterColor(string hexColor)
        {
            outerColor = (Color)ColorConverter.ConvertFromString(hexColor);
        }

        public Color getOuterTextColor()
        {
            return outerTextColor;
        }

        public void setOuterTextColor(string hexColor)
        {
            outerTextColor = (Color)ColorConverter.ConvertFromString(hexColor);
        }

        public Color getInnerColor()
        {
            return innerColor;
        }

        public void setInnerColor(string hexColor)
        {
            innerColor = (Color)ColorConverter.ConvertFromString(hexColor);
        }

        public Color getInnerTextColor()
        {
            return innerTextColor;
        }

        public void setInnerTextColor(string hexColor)
        {
            innerTextColor = (Color)ColorConverter.ConvertFromString(hexColor);
        }


        public bool getSettings() //Reads CorkBoard.ini and populates the globals with accurate values.
        //returns true if initializers were read okay, false if any issue arose.
        {
            Console.WriteLine("Begin reading .INI values.");
            Console.WriteLine(Assembly.GetEntryAssembly().Location);
            string[] inilines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "/CorkBoard.ini");
            foreach (string iniline in inilines)
            {
                if (iniline.Length > 0 && iniline[0] != '#' && iniline.Contains(" ")) //Filter out badly formatted settings
                {
                    Console.WriteLine("Parsing: " + iniline);
                    string[] iniargs = iniline.Split(' ');
                    switch (iniargs[0])
                    {
                        case "imgurl": //Todo: Input Validation
                            imgurl = iniargs[1]; //URL, single string.
                            break;
                        case "wxurl": //Todo: Input Validation
                            wxurl = iniargs[1]; //URL, single string.
                            break;
                        case "imgrefresh": //Todo: Input Validation
                            imgrefresh = int.Parse(iniargs[1]);
                            break;
                        case "wxrefresh": //Todo: Input Validation
                            wxrefresh = int.Parse(iniargs[1]);
                            break;
                        case "clkrefresh": //Todo: Input Validation
                            clkrefresh = int.Parse(iniargs[1]);
                            break;
                        case "btheme": //Input validation should be complete
                            if (iniargs.Length == 13)
                            {
                                try
                                {
                                    for (int i = 0; i < 12; i++)
                                    {
                                        btheme[i] = int.Parse(iniargs[i + 1]);
                                        if (btheme[i] < 0 || btheme[i] > 255) //Outside RGB range
                                        {
                                            Console.WriteLine("Color out of bounds!");
                                            return false;
                                        }
                                    }

                                    outerColor = (Color) ColorConverter.ConvertFromString(colorString(btheme[0], btheme[1], btheme[2]));
                                    outerTextColor = (Color)ColorConverter.ConvertFromString(colorString(btheme[3], btheme[4], btheme[5]));
                                    innerColor = (Color)ColorConverter.ConvertFromString(colorString(btheme[6], btheme[7], btheme[8]));
                                    innerTextColor = (Color)ColorConverter.ConvertFromString(colorString(btheme[9], btheme[10], btheme[11]));
                                }
                                catch (FormatException) //If it's not a valid int
                                {
                                    Console.WriteLine("Can't parse theme!");
                                    return false;
                                }
                            } else //Not enough or too many values for a theme
                            {
                                Console.WriteLine("Bad theme!");
                                return false;
                            }
                            break;
                        default:
                            Console.WriteLine("(No match - Ignored)");
                            break;
                    }
                } else
                {
                    Console.WriteLine("Commented out or Ignored: " + iniline);
                }
            }
            Console.WriteLine("End reading .INI values.");
            return true;
        }

        private string colorString(int r, int g, int b)
        {
            return "#FF" + r.ToString("X2") + g.ToString("X2") + b.ToString("X2");
        }

        public void writeSettings()
        {
            // TODO: write settings to ini.
        }
    }
}