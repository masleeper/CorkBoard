using System;
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
    class Settings
    {
        private string wxzone = "KLAF";
        private string fczone = "IND";
        private string alerts = "IN";
        private string imgurl = "https://static.thenounproject.com/png/340719-200.png"; //Full URL for image to display.
        private string ancurl = "https://pastebin.com/raw/bnvDD1we"; //Full URL for announcement text source.
        private string nwssrc = "bbc-news"; //Full URL for news JSON source.
        private int wxrefresh = 300; //Time in seconds to refresh weather - Lower for quicker refresh. BEWARE RATE LIMITING.
        private int imgrefresh = 120; //Time in seconds to refresh image - Lower means quicker refresh.
        private int ancrefresh = 120; //Time in seconds to refresh announcement text.
        private int nwsrefresh = 120; //Time in seconds to refresh news headlines.

        private int clkrefresh = 5; //Time in seconds to check clock - Lower means higher accuracy but potentially more problems. Not sure this should be a user-defined global, but adding it in for now.
        private int[] btheme = new int[12]; //Contains a basic color scheme, four RGB values. Background1, Text1, Background2, Color2.

        private Color outerColor, outerTextColor, innerColor, innerTextColor;

        public bool parseResults;

        public Settings()
        {
            outerColor = new Color();
            outerTextColor = new Color();
            innerColor = new Color();
            innerTextColor = new Color();

            outerColor = (Color)ColorConverter.ConvertFromString(colorString(0, 0, 0));
            outerTextColor = (Color)ColorConverter.ConvertFromString(colorString(40, 40, 40));
            innerColor = (Color)ColorConverter.ConvertFromString(colorString(80, 80, 80));
            innerTextColor = (Color)ColorConverter.ConvertFromString(colorString(120, 120, 120));

            parseResults = getSettings("CorkBoard.ini");
        }

        public Settings(string filename)
        {
            outerColor = new Color();
            outerTextColor = new Color();
            innerColor = new Color();
            innerTextColor = new Color();

            outerColor = (Color)ColorConverter.ConvertFromString(colorString(0, 0, 0));
            outerTextColor = (Color)ColorConverter.ConvertFromString(colorString(40, 40, 40));
            innerColor = (Color)ColorConverter.ConvertFromString(colorString(80, 80, 80));
            innerTextColor = (Color)ColorConverter.ConvertFromString(colorString(120, 120, 120));

            parseResults = getSettings(filename);
        }

        public string getWeatherZone() { return wxzone; }

        public string getForecastZone() { return fczone; }

        public string getAlertZone() { return alerts; }

        public string getImgUrl() { return imgurl; }

        public string getNewsSource() { return nwssrc; }

        public void setNewsSource(string src) { nwssrc = src; }

        public string getAnnouncementsUrl() { return ancurl; }

        public int getWeatherRefresh() { return wxrefresh; }

        public int getImgRefresh() { return imgrefresh; }

        public int getAncRefresh() { return ancrefresh; }

        public int getNewsRefresh() { return nwsrefresh; }

        public int getClockRefresh() { return clkrefresh; }

        public Color getOuterColor() { return outerColor; }

        public Color getOuterTextColor() { return outerTextColor; }

        public Color getInnerColor() { return innerColor; }

        public Color getInnerTextColor() { return innerTextColor; }

        public bool getSettings(string filename) //Reads CorkBoard.ini and populates the globals with accurate values.
        //returns true if initializers were read okay, false if any issue arose.
        {
            Console.WriteLine("Begin reading .INI values.");
            //Console.WriteLine(Assembly.GetEntryAssembly().Location);

            //todo trycatch this

            string[] inilines;

            try //Attempt to read ini file into string array
            {
                inilines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "/" + filename);
            } catch(Exception e)
            {
                if (e is FileNotFoundException)
                {
                    Console.WriteLine("CorkBoard.ini is missing!");
                } else Console.WriteLine("Unable to read .ini file: " + e.Message);

                return false;
            }

            foreach (string iniline in inilines)
            {
                if (iniline.Length > 2 && iniline[0] != '#' && iniline.Contains(" ")) //Skip empty lines, comments, and lines without parameters
                {
                    //Console.WriteLine("Parsing: " + iniline);
                    string[] iniargs = iniline.Split(' '); //Split option from parameter(s)
                    switch (iniargs[0])
                    {
                        case "imgurl": //Todo: Input Validation
                            if (!Uri.IsWellFormedUriString(iniargs[1], UriKind.Absolute))
                            {
                                Console.WriteLine("Your image URL is invalid!");
                                return false;
                            }
                            imgurl = iniargs[1]; //URL, single string.                            
                            break;

                        case "ancurl":
                            if (!Uri.IsWellFormedUriString(iniargs[1], UriKind.Absolute))
                            {
                                Console.WriteLine("Your announcement URL is invalid!");
                                return false;
                            }
                            ancurl = iniargs[1]; //URL, single string.
                            break;

                        case "newssource":
                            nwssrc = iniargs[1]; //URL, single string.
                            break;

                        case "wxzone":
                            wxzone = iniargs[1]; //Small alphanumeric code, single string.
                            break;

                        case "fczone":
                            fczone = iniargs[1]; //Small alphanumeric code, single string.
                            break;

                        case "alerts":
                            alerts = iniargs[1]; //Small alphanumeric code, single string.
                            break;

                        case "imgrefresh": //Todo: Input Validation
                            if (!int.TryParse(iniargs[1], out int imgoutval))
                            {
                                Console.WriteLine("Image Refresh Parse Issue.");
                                return false;
                            }
                            if (imgoutval < 15)
                            {
                                return false;
                            }

                            imgrefresh = imgoutval;

                            break;

                        case "wxrefresh": //Todo: Input Validation
                            if (!int.TryParse(iniargs[1], out int wxoutval))
                            {
                                Console.WriteLine("Weather Refresh Parse Issue.");
                                return false;
                            }
                            if (wxoutval < 15)
                            {
                                return false;
                            }

                            wxrefresh = wxoutval;

                            break;

                        case "clkrefresh": //Todo: Input Validation
                            if (!int.TryParse(iniargs[1], out int clkoutval))
                            {
                                Console.WriteLine("Clock Refresh Parse Issue.");
                                return false;
                            }
                            if (clkoutval < 15)
                            {
                                return false;
                            }

                            clkrefresh = clkoutval;

                            break;

                        case "ancrefresh": //Todo: Input Validation
                            if (!int.TryParse(iniargs[1], out int ancoutval))
                            {
                                Console.WriteLine("Announcement Refresh Parse Issue.");
                                return false;
                            }
                            if (ancoutval < 15)
                            {
                                return false;
                            }

                            ancrefresh = ancoutval;

                            break;

                        case "nwsrefresh": //Todo: Input Validation
                            if (!int.TryParse(iniargs[1], out int nwsoutval))
                            {
                                Console.WriteLine("News Refresh Parse Issue.");
                                return false;
                            }
                            if (nwsoutval  < 15)
                            {
                                return false;
                            }
                            break;

                        case "btheme": //Input validation should be complete
                            if (iniargs.Length == 13)
                            {
                                try
                                {
                                    for (int i = 0; i < 12; i++)
                                    {
                                        if (!int.TryParse(iniargs[i + 1], out btheme[i]))
                                        {
                                            Console.WriteLine("Color scheme parse error!");
                                            return false;
                                        }
                                        if (btheme[i] < 0 || btheme[i] > 255) //Outside RGB range
                                        {
                                            Console.WriteLine("Color out of bounds!");
                                            return false;
                                        }
                                    }

                                    outerColor = (Color)ColorConverter.ConvertFromString(colorString(btheme[0], btheme[1], btheme[2]));
                                    outerTextColor = (Color)ColorConverter.ConvertFromString(colorString(btheme[3], btheme[4], btheme[5]));
                                    innerColor = (Color)ColorConverter.ConvertFromString(colorString(btheme[6], btheme[7], btheme[8]));
                                    innerTextColor = (Color)ColorConverter.ConvertFromString(colorString(btheme[9], btheme[10], btheme[11]));
                                }
                                catch (FormatException) //If it's not a valid int
                                {
                                    Console.WriteLine("Can't parse theme!");
                                    return false;
                                }
                            }
                            else //Not enough or too many values for a theme
                            {
                                Console.WriteLine("Bad theme!");
                                return false;
                            }
                            break;
                        default:
                            Console.WriteLine("(No match - Ignored)");
                            break;
                    }
                }
                else
                {
                    //Console.WriteLine("Commented out or Ignored: " + iniline);
                }
            }
            Console.WriteLine("End reading .INI values.");
            return true;
        }

        private string colorString(int r, int g, int b)
        {
            return "#FF" + r.ToString("X2") + g.ToString("X2") + b.ToString("X2");
        }
        /*public void checkData()
        {
            if (!getSettings())
            {
                Console.ReadLine(); //Show errors in reading before exit.
                return; //any issue with INI read will auto-exit the program.
            }

            Console.WriteLine("====================");
            //CREATE NET OBJECT WITH WXURL AND IMGURL IN CONSTRUCTORS

            //var weatherdata = net.getwx();
            //var imgdata = net.getimg();
            DateTime lastRecordedTime = DateTime.Now;

            //CREATE DISPLAY OBJECT WITH PROFILE, WX, IMG, TIME

            int clktimer = 0;
            int wxtimer = 0;
            int imgtimer = 0;
            string lastWrittenTime = lastRecordedTime.ToString("hh:mm tt");

            while (true)
            {
                clktimer = ++clktimer % clkrefresh;
                if (clktimer == 0)
                {
                    if (lastWrittenTime.CompareTo(DateTime.Now.ToString("hh:mm tt")) != 0)
                    {
                        lastRecordedTime = DateTime.Now;
                        lastWrittenTime = lastRecordedTime.ToString("hh:mm tt");
                        mainWindow.updateTime(lastWrittenTime);
                        Console.WriteLine("Update clock to " + lastWrittenTime);
                    }
                }

                wxtimer = ++wxtimer % wxrefresh;
                if (wxtimer == 0)
                {
                    Console.WriteLine("Update weather.");
                }

                imgtimer = ++imgtimer % imgrefresh;
                if (imgtimer == 0)
                {
                    Console.WriteLine("Update image.");
                }

                System.Threading.Thread.Sleep(1000);

            }

        }*/
    }
}