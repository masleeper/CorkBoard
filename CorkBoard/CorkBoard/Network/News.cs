using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using CorkBoard.Core;


namespace CorkBoard.Network
{
    class News
    {
        public string[] getNews(int count, string apiCall)
        {
            string[] list = new string[count];

            for(int a = 0; a < count; a++)
            {
                list[a] = "";
            }

            if (apiCall == null || apiCall == "")
            {
                return list;
            }
            string text = new Net().getWebText(apiCall);
            dynamic wJson = HelperFunctions.convertToDynamic(text);

            try
            {
                //check properties exist
                if (wJson == null ||
                    wJson["totalResults"] == null ||
                    wJson["articles"] == null)
                    
                {
                    return list;
                }
                
                if (count > (int)wJson["totalResults"])
                {
                    count = (int)wJson["totalResults"];
                }

                for(int a = 0; a < count; a++)
                {
                    if(wJson["articles"][a] == null || 
                       wJson["articles"][a]["title"] == null)
                    {
                        return list;
                    }

                    list[a] = (string)wJson["articles"][a]["title"];
                }
                
            }
            catch (Exception)
            {
                return list;
            }


            return list;
        }
    }
}
