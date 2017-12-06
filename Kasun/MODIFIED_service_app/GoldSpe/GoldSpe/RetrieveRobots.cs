using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using RestSharp;
using System;
using System.Configuration;

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

       
        public List<string> GetRobots()
        {
         

            List<string> Robo = new List<string>();
            try
            {
                var documentServerEntityCard = ConfigurationManager.AppSettings["documentServerEntityCard"].ToString(); ///GET DOCUMENT SERVER ENTITY CARD SERVER URL
              
                string itemsPerPage= ConfigurationManager.AppSettings["itemsPerPage"].ToString();
                var response = new WebClient().DownloadString(documentServerEntityCard + "&itemsPerPage="+ itemsPerPage);


                JObject jObj = JObject.Parse(response);

                int length = (int)jObj["DocumentCount"];
           
              

                for (int i = 0; i < length; i++)
                {
                    string url = (string)jObj["Documents"][i]["sourceUrl"];

                    if (string.IsNullOrEmpty(url.Trim()) == false)    ///CHECK WHETHER "Documnets" ARRAY OF JSON HAS URL OF TARGETS
                    {
                        
                        Robo.Add(url);      ///ADDING URLs
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
