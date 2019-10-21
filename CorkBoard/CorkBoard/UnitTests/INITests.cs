using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorkBoard.Core;

namespace CorkBoard.UnitTests
{
    class IniTests
    {
        public void runIniTests()
        {
            Console.WriteLine("------ Starting INI Parser Tests ------");
            Test.PrintTest("Valid INI: ", testValidIni());
            Test.PrintTest("No INI: ", testNoIni());
            Test.PrintTest("Bad Timer Values INI: ", testBadTimerValues());
            Test.PrintTest("Bad URLs: ", testURLCorrectness());
            Test.PrintTest("Hiding Display Elements: ", testHiding());
            Test.PrintTest("Bad Color Schemes: ", testBadColorScheme());
            Console.WriteLine("-------- INI Parser Tests End -------\n\n");
        }

        public bool testValidIni()
        {
            Settings settings = new Settings("testvalidini.ini");
            if (!settings.getImgUrl().Equals("http://www.domain.com/image.png")) return false;
            if (!settings.getAnnouncementsUrl().Equals("http://www.pastebin.com/text.txt")) return false;
            if (!settings.getNewsSource().Equals("cnn-news")) return false;
            if (!settings.getWeatherZone().Equals("KORD")) return false;
            if (!settings.getForecastZone().Equals("CHI")) return false;
            if (!settings.getAlertState().Equals("IL")) return false;
            if (!settings.getAlertCounty().Equals("McHenry")) return false;
            if (settings.getImgRefresh() != 60) return false;
            if (settings.getWeatherRefresh() != 60) return false;
            if (settings.getClockRefresh() != 10) return false;
            if (settings.getAncRefresh() != 60) return false;
            if (settings.getNewsRefresh() != 60) return false;
            if (settings.getNewsCount() != 15) return false;
            if (!settings.getOuterColor().ToString().Equals("#FF010101")) return false;
            if (!settings.getOuterTextColor().ToString().Equals("#FF020202")) return false;
            if (!settings.getInnerColor().ToString().Equals("#FF030303")) return false;
            if (!settings.getInnerTextColor().ToString().Equals("#FF040404")) return false;

            return true;
        }

        public bool testNoIni() //With no INI, parser should fall back to default values.
        {
            Settings settings = new Settings("nofile.NOFILE");
            if (settings.parseResults == true) return false;
            if (!settings.getImgUrl().Equals("https://www.logolynx.com/images/logolynx/93/93557b69a559fe77bf1df92cb5bed880.jpeg")) return false;
            if (!settings.getAnnouncementsUrl().Equals("https://pastebin.com/raw/jGbcxxkv")) return false;
            if (!settings.getNewsSource().Equals("bbc-news")) return false;
            if (!settings.getWeatherZone().Equals("KLAF")) return false;
            if (!settings.getForecastZone().Equals("IND")) return false;
            if (!settings.getAlertState().Equals("IN")) return false;
            if (!settings.getAlertCounty().Equals("Tippecanoe")) return false;
            if (settings.getImgRefresh() != 120) return false;
            if (settings.getWeatherRefresh() != 300) return false;
            if (settings.getClockRefresh() != 5) return false;
            if (settings.getAncRefresh() != 120) return false;
            if (settings.getNewsRefresh() != 120) return false;
            if (settings.getNewsCount() != 10) return false;
            if (!settings.getOuterColor().ToString().Equals("#FF000000")) return false;
            if (!settings.getOuterTextColor().ToString().Equals("#FF282828")) return false;
            if (!settings.getInnerColor().ToString().Equals("#FF505050")) return false;
            if (!settings.getInnerTextColor().ToString().Equals("#FF787878")) return false;
            if (!settings.isDateVisible()) return false;
            if (!settings.isImageVisible()) return false;
            if (!settings.isNewsVisible()) return false;
            if (!settings.isTimeVisible()) return false;
            if (!settings.isWeatherVisible()) return false;
            return true;
        }

        public bool testBadTimerValues()
        {
            Settings settings = new Settings("testbadtimers.ini");
            if (settings.getImgRefresh() != 45) return false;
            if (settings.getWeatherRefresh() != 300) return false;
            if (settings.getClockRefresh() != 5) return false;
            if (settings.getAncRefresh() != 120) return false;
            if (settings.getNewsRefresh() != 120) return false;
            if (settings.getNewsCount() != 10) return false;
            return true;

        }


        public bool testURLCorrectness()
        {
            Settings settings = new Settings("testURLs.ini");
            if (!settings.getImgUrl().Equals("https://www.logolynx.com/images/logolynx/93/93557b69a559fe77bf1df92cb5bed880.jpeg")) return false;
            if (!settings.getAnnouncementsUrl().Equals("http://www.pastebin.com/text.txt")) return false;
            return true;
        }


        public bool testHiding()
        {
            Settings settings = new Settings("testHiding.ini");
            if (settings.isDateVisible()) return false;
            if (settings.isImageVisible()) return false;
            if (settings.isNewsVisible()) return false;
            if (settings.isTimeVisible()) return false;
            if (settings.isWeatherVisible()) return false;
            return true;
        }


        public bool testBadColorScheme()
        {
            Settings settings = new Settings("testBadColorScheme.ini");

            if (!settings.getOuterColor().ToString().Equals("#FF010101")) return false;
            if (!settings.getOuterTextColor().ToString().Equals("#FF020202")) return false;
            if (!settings.getInnerColor().ToString().Equals("#FF030303")) return false;
            if (!settings.getInnerTextColor().ToString().Equals("#FF040404")) return false;

            return true;
        }

    }
}
