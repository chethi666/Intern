using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldSpe
{
    public class MyService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public void Start()
        {
            
            CheckStatus check = new CheckStatus();
            check.Check();
        }
        public void Stop()
        {
            
            logger.Error("Service is Stopped");
        }
    }
}
