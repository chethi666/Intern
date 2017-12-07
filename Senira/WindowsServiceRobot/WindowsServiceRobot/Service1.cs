using System;
using System.Collections.Generic;
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

namespace WindowsServiceRobot
{
    class Service1
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public void OnDebug()
        {
            OnStart(null);
            OnStop();
        }

        protected void OnStart(string[] args)
        {

            GetClient();
            GetRobotInfo();
            
        }
        List<string> listurl = new List<string>();
        public void GetClient()
        {
            var documentServerEntityCard = ConfigurationManager.AppSettings["documentServerEntityCard"].ToString(); ///GET DOCUMENT SERVER ENTITY CARD SERVER URL

            string itemsPerPage = ConfigurationManager.AppSettings["itemsPerPage"].ToString();
            var response = new WebClient().DownloadString(documentServerEntityCard + "&itemsPerPage=" + itemsPerPage);

            
            JObject jObj = JObject.Parse(response);

            int length = (int)jObj["DocumentCount"];

            for (int i = 0; i < length; i++)
            {
                string url = (string)jObj["Documents"][i]["sourceUrl"];

                if (string.IsNullOrEmpty(url.Trim()) == false) //CHECK WHETHER "Documnets" ARRAY OF JSON HAS URL OF TARGETS
                {

                    listurl.Add(url);   // ADDING URLs
                }
            }
        }
        public void GetRobotInfo()
        {
            string[] lines = listurl.ToArray();
            string[] lines2 = new String[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                lines2[i] = lines[i].Trim();
            }

            foreach (string line1 in lines)
            {
                int pos = Array.IndexOf(lines2, line1.Trim());
                if (pos > -1)
                {
                    String line = lines[pos];
                    var Today = DateTime.Now;//SET DATE TO TODAY
                    var Yesterday = DateTime.Now.AddDays(-1);// SET DATE TO YESTERDAY
                    string x = Today.ToString("yyyy-MM-dd");
                    string y = Yesterday.ToString("yyyy-MM-dd");

                    
                    try
                    {
                        string documentServerRepoUrl = ConfigurationManager.AppSettings["documentServerRepoUrl"].ToString();//GET DOCUMENT REPOSITORY SERVER URL
                        string getdata = string.Format(documentServerRepoUrl + "\"" + "{0}" + "\"" + " AND PublishDate:" + "[" + y + " TO " + x + "]&sortByField=PublishDate&sortDescending=true", line1.Trim());  //GET CRAWLED DATA FROM TARGETED NEWS SITES


                        var response2 = new WebClient().DownloadString(getdata);

                         JObject jObj1 = JObject.Parse(response2);// C# object of JSON

                        try
                        {
                            if (jObj1["Documents"] != null && jObj1["Documents"].HasValues == true)//CHECK WHETHER TARGETED SITE HAS CRAWLED"
                            {
                                string id = (string)jObj1["Documents"][0]["_id"];

                                if (response2 != null && string.IsNullOrEmpty(id) == false)//CHECK WHETHER TARGETED SITE HAS "ATLEAST ONE DATA"
                                {
                                    //log.Info("ROBOID\tSUCCESS");
                                    logger.Info("\nInfo:" + line + "====== Robot is Working And Has Data\n\t");
                                }
                                else
                                {
                                    logger.Info("\nInfo:" + line + "====== Robot is Not Having Data\n\t");//DOESN'T HAVE CRAWLED DATA
                                }
                            }
                            else
                            {
                                logger.Info("\nInfo:" + line + "====== Robot is Not Crawling\n\t");//DIDNT  CRAWLED
                            }
                        }
                        catch (Exception e)
                        {
                            logger.ErrorException("\nCant access to" + line + "\n", e);
                        }

                    }

                    catch (Exception e)
                    {
                        logger.ErrorException("\nCant access to" + line + "\n", e);
                    }

                }

            }// onStart is finished
            
        }
        protected void OnStop()
        {
            logger.Fatal("Fatal: Service is stoped\n");
           
        }

    }
}
