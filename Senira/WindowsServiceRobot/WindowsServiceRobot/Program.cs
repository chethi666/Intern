using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using NLog;
using Topshelf;

namespace WindowsServiceRobot
{
    class Program
    {
        

        static void Main(string[] args)
        {
            Console.Out.WriteLine("Greetings, some loggings is about to take minutes.");
            Console.Out.WriteLine("");
            // ConfigureService.Configure();

            Service1 myService = new Service1();
                    myService.OnDebug();
                   System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);

        }

       
        
    }
}
