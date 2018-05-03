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
        Queue<LLGameProtocol> protocolQueue = new Queue<LLGameProtocol>();
        List<string> legalProtocolNameList = new List<string>();

        public delegate void SocketEvent(Socket socket);
        public delegate void ProtocolEvent(LLGameProtocol protocol);
        public SocketEvent AcceptNewSocketEvent;
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
            serverSocket.Listen(2);

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
                    int byteNumber = socket.Receive(buf);
                    string s = Encoding.UTF8.GetString(buf, 0, byteNumber);
                    string[] ss = s.Split();
                    LLGameProtocol protocol;
                    foreach (var item in legalProtocolNameList)
                    {
                        if (ss[0] == item)
                        {
                            Type t = Type.GetType(item);
                            if (t != null && typeof(LLGameProtocol).IsAssignableFrom(t))
                            {
                                object[] parameters = new object[1];
                                parameters[0] = socket;
                                protocol = t.Assembly.CreateInstance(s, true, System.Reflection.BindingFlags.Default, null, parameters, null, null) as LLGameProtocol;
                                protocolQueue.Enqueue(protocol);
                                processProtocolEvent.Set();
                            }
                        }
                    }
                }
            }
            catch (SocketException e)
            {
            }
            finally
            {
                string ip = (socket.RemoteEndPoint as IPEndPoint).Address.ToString();
                Console.WriteLine($"{ip}：断开连接！");
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
                    ProcessProtocolEvent(protocolQueue.Dequeue());
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
        public void SendBroadcastProtocol(LLGameProtocol protocol)
        {
            byte[] msg = Encoding.UTF8.GetBytes(protocol.GetContent());
            foreach (var item in ipSocketMap)
            {
                item.Value.Send(msg);
            }
        }
    }
}
