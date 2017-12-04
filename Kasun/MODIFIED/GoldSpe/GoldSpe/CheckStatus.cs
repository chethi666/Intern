using Newtonsoft.Json.Linq;
using NLog;
using System;
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
           
           
            RetrieveRobots rt = new RetrieveRobots();
            
            int index = 0;


            List<string> AvailableRobots = rt.GetRobots();


            foreach (string line in AvailableRobots)
            {
               
                index++;
                var Today = DateTime.Now;
                var Yesterday = DateTime.Now.AddDays(-1);
                string x = Today.ToString("yyyy-MM-dd");
                string y = Yesterday.ToString("yyyy-MM-dd");
               

                try
                {
                    string v = string.Format("http://checkmate.ukwest.cloudapp.azure.com:9100/documentServer/documentRepository/documents?query=SourceUrl:" + "\"" + "{0}" + "\"" + " AND PublishDate:" + "[" + y + " TO " + x + "]&sortByField=PublishDate&sortDescending=true", line.Trim());  //Adding Date


                    var response2 = new WebClient().DownloadString(v);




                    JObject jObj1 = JObject.Parse(response2);

                    if (jObj1["Documents"] != null && jObj1["Documents"].HasValues == true)
                    {
                        string id = (string)jObj1["Documents"][0]["_id"];

                        if (response2 != null && string.IsNullOrEmpty(id) == false)
                        {



                            logger.Info("The url of Robot= " + line + "=====is  working\n");
                        }
                        else
                        {
                       
                            logger.Error("The url of Robot= " + line + "=====is not working due to no Robot_ID values \n");
                        }
                    }
                    else
                    {

                  
                        logger.Error("The url of Robot= " + line + "=====is not working due to no Document values \n");
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
