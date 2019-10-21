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
    public class Settings
    {
        private string wxzone = "KLAF";
        private string fczone = "IND";
        private string alertstate = "IN";
        private string alertcounty = "Tippecanoe";
        private string imgurl = "https://www.logolynx.com/images/logolynx/93/93557b69a559fe77bf1df92cb5bed880.jpeg"; //Full URL for image to display.
        private string ancurl = "https://pastebin.com/raw/jGbcxxkv"; //Full URL for announcement text source.
        private string nwssrc = "bbc-news"; //Full URL for news JSON source.
        private int wxrefresh = 300; //Time in seconds to refresh weather - Lower for quicker refresh. BEWARE RATE LIMITING.
        private int imgrefresh = 120; //Time in seconds to refresh image - Lower means quicker refresh.
        private int ancrefresh = 120; //Time in seconds to refresh announcement text.
        private int nwsrefresh = 120; //Time in seconds to refresh news headlines.
        private int newscount = 10;

        private int clkrefresh = 5; //Time in seconds to check clock - Lower means higher accuracy but potentially more problems. Not sure this should be a user-defined global, but adding it in for now.

        private int[] btheme = new int[12]; //Contains a basic color scheme, four RGB values. Background1, Text1, Background2, Color2.
        private Color outerColor, outerTextColor, innerColor, innerTextColor;

        private bool weatherVisible = true;
        private bool imageVisible = true;
        private bool timeVisible = true;
        private bool dateVisible = true;
        private bool newsVisible = true;

        public bool parseResults;

        public Settings()
        {
            outerColor = new Color();
            outerTextColor = new Color();
            innerColor = new Color();
            innerTextColor = new Color();

            outerColor = (Color)ColorConverter.ConvertFromString(colorString(0, 0, 0));
            outerTextColor = (Color)ColorConverter.ConvertFromString(colorString(207, 181, 59));
            innerColor = (Color)ColorConverter.ConvertFromString(colorString(120, 120, 120));
            innerTextColor = (Color)ColorConverter.ConvertFromString(colorString(255, 255, 255));

            parseResults = getSettings("CorkBoard.ini");

            if (!parseResults) Console.WriteLine("An error has occurred while parsing the .ini file. CorkBoard will run with default settings.");
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

            if (!parseResults) Console.WriteLine("An error has occurred while parsing the .ini file. CorkBoard will run with default settings.");
        }

        public string getWeatherZone() { return wxzone; }

        public string getForecastZone() { return fczone; }

        public string getAlertState() { return alertstate; }

        public string getAlertCounty() { return alertcounty; }

        public string getImgUrl() { return imgurl; }

        public string getNewsSource() { return nwssrc; }

        public void setNewsSource(string src) { nwssrc = src; }

        public string getAnnouncementsUrl() { return ancurl; }

        public int getWeatherRefresh() { return wxrefresh; }

        public int getImgRefresh() { return imgrefresh; }

        public int getAncRefresh() { return ancrefresh; }

        public int getNewsRefresh() { return nwsrefresh; }

        public int getClockRefresh() { return clkrefresh; }

        public int getNewsCount() { return newscount; }

        public Color getOuterColor() { return outerColor; }

        public void setOuterColor(string c) { outerColor = (Color)ColorConverter.ConvertFromString(c); }

        public Color getOuterTextColor() { return outerTextColor; }

        public void setOuterTextColor(string c) { outerTextColor = (Color)ColorConverter.ConvertFromString(c); }

        public Color getInnerColor() { return innerColor; }

        public void setInnerColor(string c) { innerColor = (Color)ColorConverter.ConvertFromString(c); }

        public Color getInnerTextColor() { return innerTextColor; }

        public void setInnerTextColor(string c) { innerTextColor = (Color)ColorConverter.ConvertFromString(c); }

        public bool isTimeVisible() { return timeVisible; }

        public bool isDateVisible() { return dateVisible; }

        public bool isImageVisible() { return imageVisible; }

        public bool isWeatherVisible() { return weatherVisible; }

        public bool isNewsVisible() { return newsVisible; }

        public void setTimeVisible(bool status) { timeVisible = status; }

        public void setDateVisible(bool status) { dateVisible = status; }

        public void setWeatherVisible(bool status) { weatherVisible = status; }

        public void setImageVisible(bool status) { imageVisible = status; }

        public bool getSettings(string filename) //Reads CorkBoard.ini and populates the globals with accurate values.
        //returns true if initializers were read okay, false if any issue arose.
        {
            //Console.WriteLine("Begin reading .INI values.");
            //Console.WriteLine(Assembly.GetEntryAssembly().Location);

            //todo trycatch this

            string[] inilines;

            try //Attempt to read ini file into string array
            {
                inilines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "/" + filename);
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException)
                {
                    Console.WriteLine("Error: CorkBoard.ini is missing!");
                }
                else Console.WriteLine("Error: Unable to read .ini file: " + e.Message);

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
                                Console.WriteLine("Error: Your image URL is invalid!");
                                break;
                            }
                            imgurl = iniargs[1]; //URL, single string.                            
                            break;

                        case "ancurl":
                            if (!Uri.IsWellFormedUriString(iniargs[1], UriKind.Absolute))
                            {
                                Console.WriteLine("Error: Your announcement URL is invalid!");
                                break;
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

                        case "alertstate":
                            alertstate = iniargs[1]; //Small alphanumeric code, single string.
                            break;

                        case "alertcounty":
                            alertcounty = iniargs[1]; //Small alphanumeric code, single string.
                            break;

                        case "hide":
                            switch (iniargs[1])
                            {
                                case "weather":
                                    weatherVisible = false;
                                    break;
                                case "image":
                                    imageVisible = false;
                                    break;
                                case "time":
                                    timeVisible = false;
                                    break;
                                case "date":
                                    dateVisible = false;
                                    break;
                                case "news":
                                    newsVisible = false;
                                    break;
                                default:
                                    break;
                            }
                            break;

                        case "imgrefresh":
                            if (!int.TryParse(iniargs[1], out int imgoutval))
                            {
                                Console.WriteLine("Error: Image Refresh Parse Issue.");
                                break;
                            }
                            if (imgoutval < 15)
                            {
                                Console.WriteLine("Error: Image Refresh Value is too small!");
                                break;
                            }

                            imgrefresh = imgoutval;

                            break;

                        case "wxrefresh":
                            if (!int.TryParse(iniargs[1], out int wxoutval))
                            {
                                Console.WriteLine("Error: Weather Refresh Parse Issue.");
                                break;
                            }
                            if (wxoutval < 15)
                            {
                                Console.WriteLine("Error: Weather Refresh value is too small!");
                                break;
                            }

                            wxrefresh = wxoutval;

                            break;

                        case "clkrefresh":
                            if (!int.TryParse(iniargs[1], out int clkoutval))
                            {
                                Console.WriteLine("Error: Clock Refresh Parse Issue.");
                                break;
                            }
                            if (clkoutval > 60 || clkoutval < 1)
                            {
                                Console.WriteLine("Error: Clock Refresh value is out of bounds!");
                                break;
                            }

                            clkrefresh = clkoutval;

                            break;

                        case "ancrefresh":
                            if (!int.TryParse(iniargs[1], out int ancoutval))
                            {
                                Console.WriteLine("Error: Announcement Refresh Parse Issue.");
                                break;
                            }
                            if (ancoutval < 15)
                            {
                                Console.WriteLine("Error: Announcement Refresh value too small!");
                                break;
                            }

                            ancrefresh = ancoutval;

                            break;

                        case "nwsrefresh":
                            if (!int.TryParse(iniargs[1], out int nwsoutval))
                            {
                                Console.WriteLine("Error: News Refresh Parse Issue.");
                                break;
                            }
                            if (nwsoutval < 15)
                            {
                                Console.WriteLine("Error: News Refresh value too small!");
                                break;
                            }

                            nwsrefresh = nwsoutval;

                            break;

                        case "nwscount":
                            if (!int.TryParse(iniargs[1], out int ncntoutval))
                            {
                                Console.WriteLine("Error: News Count Parse Issue.");
                                break;
                            }
                            if (ncntoutval > 30 || ncntoutval < 1)
                            {
                                Console.WriteLine("Error: News Count value is out of bounds! (Must be between 1 and 30 inclusive)");
                                break;
                            }

                            newscount = ncntoutval;

                            break;

                        case "btheme": //Input validation should be complete
                            if (iniargs.Length == 13)
                            {
                                try
                                {
                                    bool errors = false;
                                    for (int i = 0; i < 12; i++)
                                    {
                                        if (!int.TryParse(iniargs[i + 1], out btheme[i]))
                                        {
                                            Console.WriteLine("Error: Color scheme can't be parsed!");
                                            errors = true;
                                            break;
                                        }
                                        if (btheme[i] < 0 || btheme[i] > 255) //Outside RGB range
                                        {
                                            Console.WriteLine("Error: Color out of bounds!");
                                            errors = true;
                                            break;
                                        }
                                    }
                                    if (errors) break;

                                    outerColor = (Color)ColorConverter.ConvertFromString(colorString(btheme[0], btheme[1], btheme[2]));
                                    outerTextColor = (Color)ColorConverter.ConvertFromString(colorString(btheme[3], btheme[4], btheme[5]));
                                    innerColor = (Color)ColorConverter.ConvertFromString(colorString(btheme[6], btheme[7], btheme[8]));
                                    innerTextColor = (Color)ColorConverter.ConvertFromString(colorString(btheme[9], btheme[10], btheme[11]));

                                }
                                catch (FormatException) //If it's not a valid int
                                {
                                    Console.WriteLine("Error: Can't parse theme!");
                                    break;
                                }
                            }
                            else //Not enough or too many values for a theme
                            {
                                Console.WriteLine("Error: Bad theme!");
                                break;
                            }
                            break;
                        default:
                            //Console.WriteLine("(No match - Ignored)");
                            break;
                    }
                }
                else
                {
                    //Console.WriteLine("Commented out or Ignored: " + iniline);
                }
            }
            //Console.WriteLine("End reading .INI values.");
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