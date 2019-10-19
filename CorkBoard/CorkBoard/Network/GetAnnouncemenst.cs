using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
namespace CorkBoard.Network
{
    class GetAnnouncements
    {
        public List<Announcement> getAnnouncements(String url)
        {

            String text = new Net().getWebText(url);

            Console.WriteLine("Text plain is " + text);

            JavaScriptSerializer json_serializer = new JavaScriptSerializer();


            List<Announcement> announcementList = JsonConvert.DeserializeObject<List<Announcement>>(text);

            return announcementList;
        }
    }
}