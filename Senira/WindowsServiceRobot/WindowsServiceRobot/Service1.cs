using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using Topshelf;
using NLog;

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

        //StringBuilder strbuffer = new StringBuilder();
        protected void OnStart(string[] args)
        {

            GetClient();
            GetRobotInfo();
            
        }
        public void GetClient()
        {

            var response = new WebClient().DownloadString("http://checkmate.ukwest.cloudapp.azure.com:9100/documentServer/mongo/EntityCard?query=_schema:WebSource&itemsPerPage=50");

            JObject jObj = JObject.Parse(response);

            int length = jObj.Count;

            for (int i = 0; i < length; i++)
            {
                string url = (string)jObj["Documents"][i]["sourceUrl"];

                if (string.IsNullOrEmpty(url) == false)
                {
                    String ms = String.Format("{0} {1}", url, Environment.NewLine);
                    File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "ServerSampleRobots.txt", ms);
                }
            }
        }
        public void GetRobotInfo()
        {
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\MPOC082\source\repos\ConsoleApp2\ConsoleApp2\bin\Debug\ServerSampleRobots.txt");

            string[] linesOfMyRobots = System.IO.File.ReadAllLines(@"C:\Users\MPOC082\source\repos\ConsoleApp2\ConsoleApp2\bin\Debug\Daily_Sheduled_Robots.txt");

            //String timeStamp = GetTimestamp(DateTime.Now);
            //String filename = "Result_On_" + timeStamp + ".txt";

            string[] lines2 = new String[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                lines2[i] = lines[i].Trim();
            }

            foreach (string line1 in linesOfMyRobots)
            {
                int pos = Array.IndexOf(lines2, line1.Trim());
                if (pos > -1)
                {
                    String line = lines[pos];
                    var Today = DateTime.Now;
                    var Yesterday = DateTime.Now.AddDays(-1);
                    string x = Today.ToString("yyyy-MM-dd");
                    string y = Yesterday.ToString("yyyy-MM-dd");

                    string v = string.Format("http://checkmate.ukwest.cloudapp.azure.com:9100/documentServer/documentRepository/documents?query=SourceUrl:" + "\"" + "{0}" + "\"" + " AND PublishDate:" + "[" + y + " TO " + x + "]&sortByField=PublishDate&sortDescending=true", line);  //Adding Date

                    try
                    {

                        var response2 = new WebClient().DownloadString(v);
                        JObject jObj1 = JObject.Parse(response2);// C# object of JSONJObject jObj1 = JObject.Parse(response2);// C# object of JSON

                        try
                        {
                            if (jObj1["Documents"] != null && jObj1["Documents"].HasValues == true && response2 != null)
                            {
                                string id = (string)jObj1["Documents"][1]["_id"];

                                if (string.IsNullOrEmpty(id) == false)
                                {
                                    //log.Info("ROBOID\tSUCCESS");
                                    //strbuffer.Append("[ " + line + "====== Robot is Working\n]\t");
                                    //Console.WriteLine("[ " + line + "====== Robot is Working\n]\t");
                                    logger.Info("\nInfo:" + line + "====== Robot is Working\n\t");


                                }
                                else
                                {
                                    //strbuffer.Append("[ " + line + "======== Robot is Not Working\n]\t");
                                    //Console.WriteLine("[ " + line + "======== Robot is Not Working\n]\t");
                                    logger.Info("\nInfo:" + line + "====== Robot is Not Working\n\t");
                                }
                            }
                            else
                            {
                                logger.Info("\nInfo:" + line + "====== Robot is Not Working\n\t");
                                //strbuffer.Append("[ " + line + "======== Robot is Not Working\n]\t");
                                //Console.WriteLine("[ " + line + "======== Robot is Not Working\n]\t");
                            }
                        }
                        catch (Exception e)
                        {
                            logger.ErrorException("\nCant access to" + line + "\n", e);
                            //strbuffer.Append("Cant access to" + line + " due to " + e);
                        }

                    }

                    catch (Exception e)
                    {
                        logger.ErrorException("\nCant access to" + line + "\n", e);
                        //strbuffer.Append("Cant access to" + line + " due to " + e);

                    }

                }

            }// onStart is finished
            // File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + filename, strbuffer.ToString());
        }
        protected void OnStop()
        {
            logger.Fatal("Fatal: Service is stoped\n");
            //strbuffer.Append(" Service is stoped\n");
        }

        //public String GetTimestamp(DateTime value)
        //{
        //    return value.ToString("yyyyMMddHHmmss");
        //}
    }
}
