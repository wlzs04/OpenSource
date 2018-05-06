using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LLGameServer.Server
{
    class ServerManager
    {
        Socket serverSocket = null;
        IPAddress ip = null;
        int maxLinkNumber = 2;//服务器最大监听数量。
        Dictionary<string, Socket> ipSocketMap = new Dictionary<string, Socket>();
        Queue<LLGameClientProtocol> protocolQueue = new Queue<LLGameClientProtocol>();
        Dictionary<string, LLGameClientProtocol> legalProtocolMap = new Dictionary<string, LLGameClientProtocol>();

        public delegate void SocketEvent(Socket socket);
        public delegate void ProtocolEvent(LLGameClientProtocol protocol);
        public SocketEvent AcceptNewSocketEvent;
        public SocketEvent SocketDisconnectEvent;
        public ProtocolEvent ProcessProtocolEvent;

        //收到协议唤醒线程，解决完让线程挂起。
        AutoResetEvent processProtocolEvent = new AutoResetEvent(false);

        public ServerManager()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ip = GetHostIPV4();
        }

        /// <summary>
        /// 开始监听指定端口
        /// </summary>
        /// <param name="port"></param>
        public bool StartListen(int port)
        {
            if(CheckPortInUsed(port))
            {
                return false;
            }
            IPEndPoint point = new IPEndPoint(ip, port);
            serverSocket.Bind(point);
            serverSocket.Listen(maxLinkNumber);

            Thread threadwatch = new Thread(AcceptClient);
            threadwatch.IsBackground = true;
            threadwatch.Start();

            Thread threadProcess = new Thread(ProcessProtocol);
            threadProcess.IsBackground = true;
            threadProcess.Start();

            return true;
        }

        /// <summary>
        /// 检测某个端口是否被占用
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        bool CheckPortInUsed(int port)
        {
            bool inUsed = false;

            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();
            
            foreach (IPEndPoint endPoint in ipEndPoints)
            {
                if (endPoint.Port == port)
                {
                    inUsed = true;
                    break;
                }
            }
            return inUsed;
        }

        /// <summary>
        /// 获得本机IPV4地址
        /// </summary>
        /// <returns></returns>
        IPAddress GetHostIPV4()
        {
            IPAddress ip = null;
            IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (var item in ips)
            {
                if (item.AddressFamily == AddressFamily.InterNetwork)
                {
                    ip = item;
                    break;
                }
            }
            return ip;
        }

        /// <summary>
        /// 服务器接收客户端
        /// </summary>
        void AcceptClient()
        {
            try
            {
                while (true)
                {
                    Socket clientSocket = serverSocket.Accept();
                    string ipPort = (clientSocket.RemoteEndPoint as IPEndPoint).ToString();
                    ipSocketMap[ipPort] = clientSocket;
                    AcceptNewSocketEvent?.Invoke(clientSocket);
                    Thread t = new Thread(AcceptSocketPeotocol);
                    t.IsBackground = true;
                    t.Start(clientSocket);
                }
            }
            catch (SocketException)
            {

            }
        }

        /// <summary>
        /// 服务器接收客户端发来的协议
        /// </summary>
        /// <param name="obj"></param>
        void AcceptSocketPeotocol(object obj)
        {
            Socket socket = obj as Socket;
            byte[] buf = new byte[1024];
            try
            {
                while (true)
                {
                    socket.Receive(buf, 4,SocketFlags.None);
                    int protocolLength = (buf[0]-'0') * 1000 + (buf[1] - '0') * 100 + (buf[2] - '0') * 10 + (buf[3] - '0') * 1;
                    int byteNumber = socket.Receive(buf, protocolLength, SocketFlags.None);
                    buf = DecodeProtocol(buf);
                    string s = Encoding.UTF8.GetString(buf, 0, byteNumber);
                    string[] ss = s.Split(' ');
                    
                    if(legalProtocolMap.ContainsKey(ss[0]))
                    {
                        LLGameClientProtocol protocol = legalProtocolMap[ss[0]].GetInstance();
                        protocol.LoadContentFromWString(s);
                        protocol.SetSocket(socket);
                        protocolQueue.Enqueue(protocol);
                        processProtocolEvent.Set();
                    }
                }
            }
            catch(SocketException)
            {

            }
            finally
            {
                string ip = (socket.RemoteEndPoint as IPEndPoint).ToString();
                SocketDisconnectEvent?.Invoke(socket);
                ipSocketMap.Remove(ip);
                socket.Close();
            }
        }

        /// <summary>
        /// 处理协议
        /// </summary>
        void ProcessProtocol()
        {
            while (true)
            {
                if (protocolQueue.Count > 0)
                {
                    LLGameClientProtocol protocol = protocolQueue.Dequeue();
                    if(protocol!=null)
                    {
                        ProcessProtocolEvent(protocol);
                    }
                }
                else
                {
                    processProtocolEvent.WaitOne();
                }
            }
        }

        /// <summary>
        /// 服务器停止监听，并断开所有链接
        /// </summary>
        public void StopListen()
        {
            foreach (var item in ipSocketMap)
            {
                item.Value.Close();
            }
            ipSocketMap.Clear();
            serverSocket.Close();
        }

        /// <summary>
        /// 发送广播协议
        /// </summary>
        /// <param name="protocol"></param>
        public void SendBroadcastProtocol(LLGameClientProtocol protocol)
        {
            byte[] msg = Encoding.UTF8.GetBytes(protocol.ExportContentToWString());
            foreach (var item in ipSocketMap)
            {
                item.Value.Send(msg);
            }
        }

        /// <summary>
        /// 添加合法协议
        /// </summary>
        /// <param name="protocol"></param>
        public void AddLegalProtocol(LLGameClientProtocol protocol)
        {
            legalProtocolMap.Add(protocol.GetName(), protocol);
        }

        byte[] DecodeProtocol(byte[] content)
        {
            return content;
        }
    }
}
