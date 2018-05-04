using LLGameServer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LLGameServer.TestGame.TableHockeyGame
{
    class TableHockeyGameUserData : UserData
    {
        string myKey;//用来标记自己
        string opponentKey;//用来标记对手
        public Socket mySocket = null;
        public Socket opponentSocket = null;
        string state;//用来标记当前状态，
        //分为：未准备；准备中；游戏中

        public TableHockeyGameUserData()
        {
            SetContent("myKey", myKey);
            SetContent("opponentKey", opponentKey);
            SetContent("state", state);
        }
    }
}
