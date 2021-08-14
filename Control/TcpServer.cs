using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TcpipTest
{
    class TcpServer
    {
        //Todo Logger 추가 App으로 추가해서 연동할것

        private static TcpListener _tcpServer;
        private static TcpClient _tcpClient;
        
        public TcpServer()
        {
            _tcpServer = new TcpListener(IPAddress.Parse("127.0.0.1"), 15000);
            Initialize();
        }

        ~TcpServer()
        {
            //
        }

        public bool SendCommend(string commend, object o = null)
        {
            if (_tcpClient == null)
            {
                return false;
            }

            if (commend.IndexOf("<") > 0)
            {
                commend = commend.Substring(1);
            }

            if (o == null)
            {
                NetworkStream ns;

                try
                {
                    ns = _tcpClient.GetStream();
                    StreamWriter sw = new StreamWriter(ns);
                    sw.WriteLine(commend);
                    sw.Flush();
                }
                catch
                {
                    try
                    {
                        _tcpClient.Dispose();
                    }
                    catch
                    {
                        //
                    }
                }
            }
            else if (o != null)
            {
                TcpClient tc = (TcpClient)o;
                NetworkStream ns;
                try
                {
                    ns = _tcpClient.GetStream();
                    StreamWriter sw = new StreamWriter(ns);
                    sw.WriteLine(commend);
                    sw.Flush();
                }
                catch
                {
                    try
                    {
                        _tcpClient.Dispose();
                    }
                    catch
                    {
                        //
                    }
                }
            }

            return true;
        }

        public virtual bool Initialize()
        {
            _tcpServer.Start();
            Task.Factory.StartNew(AcceptClient);

            return true;
        }

        async Task AcceptClient()
        {
            while(true)
            {
                _tcpClient = await _tcpServer.AcceptTcpClientAsync().ConfigureAwait(false);
                new Thread(() => { BroadcastData(); }) { IsBackground = true }.Start();
            }
        }

        void BroadcastData()
        {
            NetworkStream ns = _tcpClient.GetStream();
            StreamReader sr = new StreamReader(ns);
            string commend;

            while (true)
            {
                commend = string.Empty;
                try
                {
                    commend = sr.ReadLine();
                    sr.DiscardBufferedData();
                }
                catch
                {
                    break;
                }

                if (String.IsNullOrEmpty(commend) == false)
                {
                    SendCommend(commend, _tcpClient);
                    
                    ProcessMain.ReciveMainQueue.Enqueue(commend);
                }
            }
        }
    }
}
