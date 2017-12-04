using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GoldSpe
{
    class RetrieveRobots
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        int l = 0;
        public List<string> GetRobots()
        {
         

            List<string> Robo = new List<string>();
            try
            {


                var response = new WebClient().DownloadString(" http://checkmate.ukwest.cloudapp.azure.com:9100/documentServer/mongo/EntityCard?query=_schema:WebSource&itemsPerPage=500");


                JObject jObj = JObject.Parse(response);

                int length = (int)jObj["DocumentCount"];
                l = length;
              

                for (int i = 0; i < length; i++)
                {
                    string url = (string)jObj["Documents"][i]["sourceUrl"];

                    if (string.IsNullOrEmpty(url.Trim()) == false)
                    {
                        


                        Robo.Add(url);
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }



            return Robo;

        }

    }
}
