using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorkBoard.Network;
namespace CorkBoard.UnitTests
{
    class AnnouncementTests
    {
        public void runAnnouncementsTest()
        {
            Console.WriteLine("------ Starting Announcement Tests ------");
            Test.PrintTest("Gets Valid Announcements: ", testAnnouncementsCall());
            Test.PrintTest("Gets Invalid Announcements: ", testInvalidAnnouncement());
            Console.WriteLine("-------- Announcement Tests End -------\n\n");

           
        }

        public bool testAnnouncementsCall()
        {
            List<Announcement> test_list = new GetAnnouncements().getAnnouncements("https://kassarl.github.io/corkboardjson/announcements.json");
            if (test_list == null)
            {
                return false;
            }
            return true;
        }

        public bool testInvalidAnnouncement()
        {
            List<Announcement> test_list = new List<Announcement>();
            try
            {
                 test_list = new GetAnnouncements().getAnnouncements("https://kassarl.github.io/corkboardjson/invalid.json");

            } catch
            {
                return false;
            }
            if (test_list.Count != 1)
            {
                return false;
            }
            if(test_list[0].body.Equals("Please use valid announcement format"))
            {
                return true;
            }
            return false;
        }

    }
}
