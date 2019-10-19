using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace CorkBoard.Network
{
    class Announcement
    {
        [JsonProperty("title")]
        public String title
        {
            get;
            set;
        }
        [JsonProperty("body")]
        public String body
        {
            get;
            set;
        }

        public Announcement()
        {
            this.title = "";
            this.body = "";
        }
        public Announcement(String title, String body)
        {
            this.title = title;
            this.body = body;
        }

        public String getTitle()
        {
            return this.title;
        }
        public String getBody()
        {
            return this.body;
        }
    }
}
