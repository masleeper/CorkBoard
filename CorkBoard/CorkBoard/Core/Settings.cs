using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace CorkBoard.Core
{
    class Settings
    {
        private string wxurl; //Full URL for weather API in our zone. In the future, maybe replace with zip code and structure an appropriate API link on our own?
        private string imgurl; //Full URL for image to display.
        private int wxrefresh = 300; //Time in seconds to refresh weather - Lower for quicker refresh. BEWARE RATE LIMITING.
        private int imgrefresh = 120; //Time in seconds to refresh image - Lower means quicker refresh.
        private int clkrefresh = 5; //Time in seconds to check clock - Lower means higher accuracy but potentially more problems. Not sure this should be a user-defined global, but adding it in for now.
        private int[] btheme = new int[12]; //Contains a basic color scheme, four RGB values. Background1, Text1, Background2, Color2.

        public Settings()
        {
            getSettings();
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

        public bool getSettings() //Reads CorkBoard.ini and populates the globals with accurate values.
        //returns true if initializers were read okay, false if any issue arose.
        {
            Console.WriteLine("Begin reading .INI values.");
            string[] inilines = File.ReadAllLines(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "/CorkBoard.ini");
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
