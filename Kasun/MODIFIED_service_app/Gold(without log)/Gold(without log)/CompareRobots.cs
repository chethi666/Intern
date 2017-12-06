using Newtonsoft.Json.Linq;
using System;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Net;

namespace Gold_without_log_
{
    class CompareRobots
    {
        public string[] Compare()

        {


            RetrieveRobots rt = new RetrieveRobots();
            string[] lines = rt.GetRobots();
           
            string[] linesOfMyRobots = System.IO.File.ReadAllLines(@"Daily_Sheduled_Robots.txt");
            string[] AvailableRobo;
            AvailableRobo = new String[linesOfMyRobots.Length];


            string[] lines2 = new String[lines.Length];
                         string[] lines3 = new String[linesOfMyRobots.Length];

            for (int i = 0; i < lines.Length; i++)
                        {
                            lines2[i] = lines[i].Trim();
                        }

            for (int r = 0; r < linesOfMyRobots.Length; r++)
            {
                lines3[r] = linesOfMyRobots[r].Trim();
            }

       for(int k=0;k< lines3.Length; k++)
            {

                for(int j = k + 1; j < lines2.Length; j++)
                {
                    if (lines3[k] == lines2[j])
                    {
                        AvailableRobo[k] = lines3[k];
                      
                    }

                }

            }
            
                return AvailableRobo;
            
        }
    }
}
    