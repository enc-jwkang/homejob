using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TcpipTest
{
    public class LMK6 : MainWindow
    {
        public static ConcurrentQueue<string> LMK6Queue = new ConcurrentQueue<string>();

        private Thread _processThread;
        private bool _threadExit = false;

        private void ExcuteThread()
        {
            string command = "";
            while (_threadExit == false)
            {
                if (LMK6Queue.IsEmpty) continue;

                command = "";
                if (LMK6Queue.TryDequeue(out command))
                {
                    //Todo
                    mess.addText(command);
                }
            }
        }
    }
}
