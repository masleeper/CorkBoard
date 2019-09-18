using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.IO;

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

        public static byte[] convertToByteArray(string src)
        {
            return Encoding.ASCII.GetBytes(src);
        }

        public static MemoryStream convertToMemStream(byte[] bytes)
        {
            return new MemoryStream(bytes);
        }

        public static MemoryStream convertToMemStream(string src)
        {
            return new MemoryStream(convertToByteArray(src));
        }
        
    }
}
