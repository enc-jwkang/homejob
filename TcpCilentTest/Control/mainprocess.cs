using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TcpCilentTest
{
    class mainprocess
    {
        public static ConcurrentQueue<string> MainQueue = new ConcurrentQueue<string>();

        static TcpCilenttest _tcpClienttest;

        public Thread mMaincommend = new Thread(SendCommand) { IsBackground = true };

        public mainprocess()
        {
            _tcpClienttest = new TcpCilenttest();

            mMaincommend.Start();
        }
       
        private static void SendCommand()
        {
            string command = "";
            bool rst = false;
            while (true)
            {
                if (MainQueue.IsEmpty) continue;

                command = "";
                rst = false;
                while (!MainQueue.TryDequeue(out command))
                {
                    Thread.Sleep(1);
                }
                rst = _tcpClienttest.SendCommend(command);
            }
        }
    }
}
