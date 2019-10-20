using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CorkBoard.Network
{
    class Net
    {
        public string getWebText(string url)
        {

            WebClient wc = new WebClient();
            wc.Headers.Add("user-agent", "CorkBoard");
            try
            {
                string text = wc.DownloadString((url.Contains("://") || url.Contains(":\\"))? url : "http://" + url);
                
                return text;
            }catch (Exception)
            {
                return "error";
            }

            
        }
        public BitmapImage getWebImage(string url)
        {

            try
            {
                BitmapImage bmi = new BitmapImage();
                bmi.BeginInit();
                bmi.UriSource = new System.Uri(url);
                bmi.EndInit();
                return bmi;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public string getWebFile(string url)
        {

            WebClient wc = new WebClient();
            HttpRequestHeader requestHeader;
            wc.Headers.Add("user-agent", "CorkBoard");
            string fileType = url.Split('.')[url.Split('.').Length - 1];
            try
            {
                string rnd = ".\\FILE_" + new Random().Next(111111, 99999999)+"." + fileType;
                wc.DownloadFile((url.Contains("://") || url.Contains(":\\")) ? url : "http://" + url, rnd);

                return rnd;
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
