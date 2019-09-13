using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace CorkBoard.Core
{
    public static class HelperFunctions
    {
        public static dynamic convertToDynamic(string json)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            dynamic jsonOut = jss.Deserialize<dynamic>(json);

            return jsonOut;
        }
    }
}
