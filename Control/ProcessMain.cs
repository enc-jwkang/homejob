using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TcpipTest
{
    public class ProcessMain
    {
        public static ConcurrentQueue<string> ReciveMainQueue = new ConcurrentQueue<string>();
        
        public enum eState
        {
            Done,
            Start,
            Doing,
            Error,
        }
        public enum eCOMCommend
        {
            None,
            LMK6,
            Microshake,
        }
        
        public eCOMCommend _PatternPcCommend;

        private Thread _processThread;
        private bool _threadExit = false;
        
        public ProcessMain()
        {
            _processThread = new Thread(ExcuteThread) { IsBackground = true };
            _processThread.Start();
        }

        ~ProcessMain()
        {
            _processThread.Join();
            _threadExit = true;
        }
        
        private void SplitCommend(string Commend)
        {
            string[] sSplitedcommend = Commend.Split('.');

            try
            {
                _PatternPcCommend = (eCOMCommend)Enum.Parse(typeof(eCOMCommend), sSplitedcommend[0]);
            }
            catch (Exception)
            {
                return;
            }

            switch (_PatternPcCommend)
            {
                case eCOMCommend.None:
                    break;
                case eCOMCommend.LMK6:
                    LMK6.LMK6Queue.Enqueue(Commend);
                    break;
                case eCOMCommend.Microshake:
                    Microshake.MicroshakeQueue.Enqueue(Commend);
                    break;
                default:
                    break;
            }
            
        }

        private void ExcuteThread()
        {
            string command = "";
            while (_threadExit == false)
            {
                if (ReciveMainQueue.IsEmpty) continue;

                command = "";
                if (ReciveMainQueue.TryDequeue(out command))
                {
                    int temp = ReciveMainQueue.Count;
                    SplitCommend(command);
                }
                Thread.Sleep(1);
            }
        }
    }
}
