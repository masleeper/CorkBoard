using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace CorkBoard.Network
{
    class Net
    {
        public string getWebText(string url)
        {
            WebClient wc = new WebClient();
            HttpRequestHeader requestHeader;
            wc.Headers.Add("user-agent", "CorkBoard");
            try
            {
                string text = wc.DownloadString(url);

                return text;
            }catch (Exception)
            {
                return "error";
            }

            
        }
    }
}
