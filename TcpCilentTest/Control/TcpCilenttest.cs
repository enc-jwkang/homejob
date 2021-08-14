using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TcpCilentTest
{
    class TcpCilenttest
    {
        public ConcurrentQueue<string> ReciveMainQueue = new ConcurrentQueue<string>();

        public bool IsConnect { get; protected set; }
        public string sCommend { get; set; }

        public TcpClient _tcpClient;
        Thread _ClientThread;

        public TcpCilenttest()
        {
            IsConnect = false;
            _ClientThread = new Thread(ClientReciveData) { IsBackground = true };

            Task.Factory.StartNew(ClientConnect);

        }

        ~TcpCilenttest()
        {
            _ClientThread.Abort();
        }

        public bool SendCommend(string commend)
        {
            if (IsConnect == false)
            {
                return false;
            }

            NetworkStream ns;
            int timeout = _tcpClient.SendTimeout;
            try
            {
                ns = _tcpClient.GetStream();
                StreamWriter sw = new StreamWriter(ns);
                sw.WriteLine(commend);
                Thread.Sleep(100);
                sw.FlushAsync();
                return true;
            }
            catch
            {
                try
                {
                    _tcpClient.Dispose();
                }
                catch
                {

                }
                IsConnect = false;
                return false;
            }
        }

        async Task ClientConnect()
        {
            var ConnectTask = Task.Run(() =>
            {
                try
                {
                    _tcpClient = new TcpClient("127.0.0.1", 15000);
                    _tcpClient.SendTimeout = 1000;
                }
                catch
                {

                }
            });

            var TimeoutTask = Task.Delay(10 * 1000);
            var doneTask = await Task.WhenAny(ConnectTask, TimeoutTask).ConfigureAwait(false);

            if (doneTask == TimeoutTask)
            {
                try
                {
                    ConnectTask.Dispose();
                }
                catch
                {

                }
                IsConnect = false;
            }

            if (_tcpClient.Connected)
            {
                IsConnect = true;
                _ClientThread.Start();
            }//else
        }

        void ClientReciveData()
        {
            NetworkStream ns = _tcpClient.GetStream();
            StreamReader sr = new StreamReader(ns);

            string commend = "";

            while (_tcpClient.Connected)
            {
                try
                {
                    commend = sr.ReadLine();
                }
                catch
                {
                    break;
                }
                if (String.IsNullOrEmpty(commend) != false)
                {
                    ReciveMainQueue.Enqueue(commend);
                }
            }
        }
    }
}
