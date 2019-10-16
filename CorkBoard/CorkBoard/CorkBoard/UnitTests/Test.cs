using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorkBoard.UnitTests
{
    
    public static class Test
    {
        public static bool PrintTest(string name, bool result)
        {
            if (result)
                Console.WriteLine(name + ": Passed");
            else
                Console.WriteLine(name + ": Failed");

            return result;
        }

        public static void runTests()
        {
            new NetworkTests().runNetworkTests();
            new NetworkTests().runWeatherTests();
            new NetworkTests().runForecastTests();
            new NetworkTests().runAlertTests();
        }
    }
}
