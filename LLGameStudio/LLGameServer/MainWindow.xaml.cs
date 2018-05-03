using LLGameServer.Server;
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
        ServerManager serverManager;
        static int port = 1234;

        public MainWindow()
        {
            InitializeComponent();
            serverManager = new ServerManager();
            serverManager.AcceptNewSocketEvent += AcceptNewSocket;
            serverManager.ProcessProtocolEvent += ProcessProtocol;
            if (serverManager.StartListen(port))
            {
                textBoxShowContent.AppendText($"开始监听端口：{port}。");
            }
            else
            {
                textBoxShowContent.AppendText($"端口：{port}无法监听，可能被占用！");
            }
        }

        void AcceptNewSocket(Socket socket)
        {
            Dispatcher.Invoke(new Action(()=> { textBoxShowContent.AppendText((socket.RemoteEndPoint as IPEndPoint).ToString()); }));
        }

        void ProcessProtocol(LLGameProtocol protocol)
        {
            protocol.Process();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            serverManager.StopListen();
        }
    }
}
