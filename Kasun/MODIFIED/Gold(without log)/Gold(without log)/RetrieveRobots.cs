using Newtonsoft.Json.Linq;
using System;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Net;

namespace Gold_without_log_
{
    class RetrieveRobots
    {

        public string[] GetRobots() {

           
            var response = new WebClient().DownloadString("http://checkmate.ukwest.cloudapp.azure.com:9100/documentServer/mongo/EntityCard?query=_schema:WebSource&itemsPerPage=500");


            JObject jObj = JObject.Parse(response);


            int length = jObj.Count;
            string[] Robo;
            Robo= new String[length];

            for (int i = 0; i < length; i++)
            {
                string url = (string)jObj["Documents"][i]["sourceUrl"];

                if (string.IsNullOrEmpty(url) == false)
                {
                    //String ms = String.Format("{0} {1}", url, Environment.NewLine);
                    //File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "ServerSampleRobots.txt", ms);

                    
                    Robo[i] = url;
                }
            }


            
                return Robo;
            

        }
     
            
    }
}
