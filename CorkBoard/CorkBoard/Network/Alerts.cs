using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorkBoard.Core;

namespace CorkBoard.Network
{
    public class Alerts
    {
        public struct AlertData
        {
            public string severity;
            public string msg;
        }

        public Alerts()
        {

        }

        public List<AlertData> getAlerts(string state, string county)
        {
            List<AlertData> data = new List<AlertData>();
            string call = "https://api.weather.gov/alerts/active/area/" + state;
            if (county == null || county == "")
            {
                AlertData err;
                err.severity = "ERROR";
                err.msg = "Empty county value";
                data.Add(err);
                return data;
            }
            if (state == null || state == "")
            {
                AlertData err;
                err.severity = "NO API CALL";
                err.msg = "Empty state value";
                data.Add(err);
                return data;
            }
            string webText = "";
            dynamic wJson = null;
            try
            {
                webText = new Net().getWebText(call);
                wJson = HelperFunctions.convertToDynamic(webText);
            }
            catch (Exception e)
            {
                AlertData err;
                err.severity = "ERROR";
                err.msg = "Invalid API Call";
                data.Add(err);
            }

            try
            {
                
                if (wJson != null &&
                    wJson["features"] != null &&
                    wJson["features"][0] != null)
                {
                    int i = 0;
                    while (wJson["features"][i] != null)
                    {
                        AlertData data1;
                        if (((string)wJson["features"][i]["properties"]["areaDesc"]).ToLower().Contains(county.ToLower()))
                        {

                            data1.msg = wJson["features"][i]["properties"]["headline"] != null ? (string)wJson["features"][i]["properties"]["headline"] : "";
                            data1.severity = wJson["features"][i]["properties"]["severity"] != null ? (string)wJson["features"][i]["properties"]["severity"] : "";

                            data.Add(data1);
                        }

                        i++;
                    }
                }
                else
                {
                    AlertData err;
                    err.severity = "NONE";
                    err.msg = "No alerts found value";
                    data.Add(err);
                }



            }
            catch (Exception e)
            {
                
            }
            if(data.Count == 0)
            {
                AlertData noAlert;
                noAlert.msg = "No ALerts";
                noAlert.severity = "NONE";
                data.Add(noAlert);
            }
            return data;
        }

    }
}
