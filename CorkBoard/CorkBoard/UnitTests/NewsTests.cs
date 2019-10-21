using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorkBoard.Network;
namespace CorkBoard.UnitTests
{
    class NewsTests
    {

        public void runNewsTests()
        {
            Console.WriteLine("------ Starting News Tests ------");
            Test.PrintTest("Gets Valid News: ", validNewsTest());
            Test.PrintTest("Gets Invalid News: ", invalidNewsTest());
            Console.WriteLine("-------- News Tests End -------\n\n");


        }

        public bool validNewsTest()
        {
            News news = new News();
            int count = 3;
            string[] list = new string[count];
            string url = "https://newsapi.org/v2/top-headlines?sources=" + "bbc-news" + "&apiKey=5969a901e08f42c7a532e0d93a039ffa";
            list = news.getNews(count, url);
            if(list == null)
            {
                return false;
            }
            return true;
        }

        public bool invalidNewsTest()
        {
            News news = new News();
            int count = 3;
            string[] list = new string[count];
            string url = "google.com";
            list = news.getNews(count, url);
            if (list[0] != "")
            {
                return false;
            }
            return true;


        }
    }
}
