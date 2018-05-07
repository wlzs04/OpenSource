using LLGameServer.Data;
using LLGameServer.Encrypt;
using LLGameServer.Server;
using LLGameServer.TestGame.TableHockeyGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LLGameServer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        EncryptNumber encryptNumber = null;
        ServerManager serverManager;
        UserDataManager userDataManager;
        static int port = 1234;
        int maxUserNumber = 2;

        string firstUserKey;
        string secondUserKey;

        string encryptKey = "4201357";

        public MainWindow()
        {
            InitializeComponent();

            InitServer();
        }

        void InitServer()
        {
            serverManager = ServerManager.GetInstance();

            encryptNumber = new EncryptNumber();
            encryptNumber.SetKey(encryptKey);

            serverManager.SetEncryptClass(encryptNumber);

            serverManager.AddLegalProtocol(new CStartGameProtocol());
            serverManager.AddLegalProtocol(new CRestartGameProtocol());
            serverManager.AddLegalProtocol(new CSendMyHandBallInfoProtocol());
            serverManager.AcceptNewSocketEvent += AcceptNewSocket;
            serverManager.SocketDisconnectEvent += SocketDisconnect;
            serverManager.ProcessProtocolEvent += ProcessProtocol;
            if (serverManager.StartListen(port))
            {
                AppendNewLine($"开始监听端口：{port}。");
            }
            else
            {
                AppendNewLine($"端口：{port}无法监听，可能被占用！");
            }
            userDataManager = UserDataManager.GetInstance();
            userDataManager.SetMaxUserNumber(maxUserNumber);
        }

        void AcceptNewSocket(Socket socket)
        {
            string userString = (socket.RemoteEndPoint as IPEndPoint).ToString();
            AppendNewLine(userString + "连接到服务器。");
            
            int userCount = userDataManager.GetUserCount();
            if(userCount < maxUserNumber)
            {
                TableHockeyGameUserData userData = new TableHockeyGameUserData();
                userData.SetContent("myKey", userString);
                userData.mySocket = socket;
                if (userCount==0)
                {
                    firstUserKey = userString;
                }
                else if(userCount==1)
                {
                    secondUserKey = userString;
                    TableHockeyGameUserData opponentUserData = (TableHockeyGameUserData)userDataManager.GetUserData(firstUserKey);
                    opponentUserData.SetContent("opponentKey", userString);
                    userData.SetContent("opponentKey", opponentUserData.GetContent("myKey"));
                    opponentUserData.opponentSocket = socket;
                    userData.opponentSocket = opponentUserData.mySocket;
                }
                userDataManager.AddUserData(userString, userData);
                AppendNewLine($"连接人数：{userCount+1}。");
            }
            else
            {
                AppendNewLine($"连接人数过多。");
            }
        }

        void SocketDisconnect(Socket socket)
        {
            AppendNewLine((socket.RemoteEndPoint as IPEndPoint).ToString() + "断开连接。");
        }

        void ProcessProtocol(LLGameClientProtocol protocol)
        {
            protocol.Process();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(serverManager!=null)
            {
                serverManager.StopListen();
            }
        }

        private void AppendNewLine(string content)
        {
            Dispatcher.Invoke(new Action(() => { textBoxShowContent.AppendText(content + Environment.NewLine); }));
        }
    }
}
