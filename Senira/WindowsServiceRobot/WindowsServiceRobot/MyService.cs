using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using System.Configuration;

namespace WindowsServiceRobot
{
    class MyService
    {
        
        private static Logger logger = LogManager.GetCurrentClassLogger();


        readonly CancellationTokenSource _cancellationTokenSource;
        readonly CancellationToken _cancellationToken;
        readonly Task _task;

        public MyService()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;

            _task = new Task(DoWork, _cancellationToken);
        }



        public void Start()
        {
            _task.Start();
            ///Start service                      

        }


        public void Stop()
        {
            _cancellationTokenSource.Cancel();

            _task.Wait();   ///Stop service    
        }

        private void DoWork()
        {


            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                Service1 check = new Service1();
                check.OnDebug();
                System.Threading.Thread.Sleep(12 * 60 * 60 * 1000);
            }
        }
    }
}
