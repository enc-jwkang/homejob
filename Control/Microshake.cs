using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace TcpipTest
{
    public class Microshake : MainWindow
    {
        public static ConcurrentQueue<string> MicroshakeQueue = new ConcurrentQueue<string>();

        private Thread _processThread;
        private bool _threadExit = false;

        private void ExcuteThread()
        {
            string command = "";
            while (_threadExit == false)
            {
                if (MicroshakeQueue.IsEmpty) continue;

                command = "";
                if (MicroshakeQueue.TryDequeue(out command))
                {
                    //Todo
                    mess.addText(command);
                }
            }
        }
    }
}
