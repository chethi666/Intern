using Newtonsoft.Json.Linq;
using System;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Net;

namespace Gold_without_log_
{
    class CheckStatus
    {
        public void Check()
        {
            int a = 0;
            CompareRobots cmp = new CompareRobots();
            RetrieveRobots rt = new RetrieveRobots();
            StringBuilder strbuffer = new StringBuilder();
            int index = 0;


            string[] linesOfMyRobots = System.IO.File.ReadAllLines(@"Daily_Sheduled_Robots.txt");
          

            string[] AvailableRobots = cmp.Compare();


                String timeStamp = GetTimestamp(DateTime.Now);
                String filename = "Result_On_" + timeStamp + ".txt";

                foreach (string line in AvailableRobots)
                {
                //    String line = lines[pos];
                index++;
                var Today = DateTime.Now;
                    var Yesterday = DateTime.Now.AddDays(-1);
                    string x = Today.ToString("yyyy-MM-dd");
                    string y = Yesterday.ToString("yyyy-MM-dd");
                if (line == null)
                {
                    a = index;
                    strbuffer.Append("The url of Robot= " + linesOfMyRobots[a-1] + " ====is not Available in server \n");
                }
                
                try
                {
                    string v = string.Format("http://checkmate.ukwest.cloudapp.azure.com:9100/documentServer/documentRepository/documents?query=SourceUrl:" + "\"" + "{0}" + "\"" + " AND PublishDate:" + "[" + y + " TO " + x + "]&sortByField=PublishDate&sortDescending=true", line.Trim());  //Adding Date


                    var response2 = new WebClient().DownloadString(v);

               

                
                    JObject jObj1 = JObject.Parse(response2);// C# object of JSONJObject jObj1 = JObject.Parse(response2);// C# object of JSON

                    if (jObj1["Documents"] != null && jObj1["Documents"].HasValues == true )
                    {
                        string id = (string)jObj1["Documents"][0]["_id"];

                        if (response2 != null && string.IsNullOrEmpty(id) == false)
                        {

                          

                            strbuffer.Append("The url of Robot= " + line+ "=====is  working\n");
                      
                           
                        }
                        else
                        {
                            strbuffer.Append("The url of Robot= " + line + "=====is not working due to no Robot_ID values \n");
                        }
                    }
                    else
                    {
                        strbuffer.Append("The url of Robot= " + line + "=====is not working due to no Document values \n");
                    }


                }
                catch (Exception e)
                {
                   
                }

           
            }
           


            File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + filename, strbuffer.ToString());
            }
            

            public String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmss");
        }

    }
    }

