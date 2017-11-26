using Newtonsoft.Json.Linq;
using System;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Net;


namespace Gold_without_log_
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        public void OnDebug() {
            OnStart(null);

        }


        StringBuilder strbuffer = new StringBuilder();

        protected override void OnStart(string[] args)
        {


            RetrieveRobots R1 = new RetrieveRobots();
            R1.GetRobots();
            CheckStatus check = new CheckStatus();
            check.Check();

           

        }

        protected override void OnStop()
        {
            String timeStamp = GetTimestamp(DateTime.Now);
            String filename = "Result_On_" + timeStamp + ".txt";


            strbuffer.Append(" Service is stoped\n");

            File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + filename, strbuffer.ToString());
        }

        public String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmss");
        }
       
    }
}
