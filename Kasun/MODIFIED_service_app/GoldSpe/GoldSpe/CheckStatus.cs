using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GoldSpe
{
    class CheckStatus
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public void Check()
        {


            RetrieveRobots rt = new RetrieveRobots();   //// Retriving target sites

            int index = 0;


            List<string> AvailableRobots = rt.GetRobots();


            foreach (string line in AvailableRobots)
            {

                index++;
                var Today = DateTime.Now;            ////SET DATE TO TODAY
                var Yesterday = DateTime.Now.AddDays(-1);       ////SET DATE TO YESTERDAY
                string x = Today.ToString("yyyy-MM-dd");
                string y = Yesterday.ToString("yyyy-MM-dd");


                try
                {
                    string documentServerRepoUrl = ConfigurationManager.AppSettings["documentServerRepoUrl"].ToString();//GET DOCUMENT REPOSITORY SERVER URL
                    string getdata = string.Format(documentServerRepoUrl + "\"" + "{0}" + "\"" + " AND PublishDate:" + "[" + y + " TO " + x + "]&sortByField=PublishDate&sortDescending=true", line.Trim());  //GET CRAWLED DATA FROM TARGETED NEWS SITES


                    var response2 = new WebClient().DownloadString(getdata);




                    JObject jObj1 = JObject.Parse(response2);

                    if (jObj1["Documents"] != null && jObj1["Documents"].HasValues == true)//CHECK WHETHER TARGETED SITE HAS CRAWLED"
                    {
                        string id = (string)jObj1["Documents"][0]["_id"];

                        if (response2 != null && string.IsNullOrEmpty(id) == false)  //CHECK WHETHER TARGETED SITE HAS "ATLEAST ONE DATA"
                        {


                            logger.Info("The url of Robot= " + line + "=====is  working\n");
                        }
                        else
                        {

                            logger.Error (line +"===== DOESN'T HAVE CRAWLED DATA \n");
                        }
                    }
                    else
                    {

                        
                        logger.Error( line + "===== DIDNT  CRAWLED \n");
                    }


                }
                catch (Exception e)
                {
                    logger.Error(e);
                }


            }

        }

    }
}
