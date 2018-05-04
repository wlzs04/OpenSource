using LLGameServer.Data;
using LLGameServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LLGameServer.TestGame.TableHockeyGame
{
    class CStartGameProtocol : LLGameClientProtocol
    {
        public CStartGameProtocol():base("CStartGameProtocol")
        {

        }

        public override LLGameClientProtocol GetInstance()
        {
            return new CStartGameProtocol();
        }

        public override void Process()
        {
            UserDataManager userDataManager = UserDataManager.GetInstance();
            string userKey = (socket.RemoteEndPoint as IPEndPoint).ToString();
            TableHockeyGameUserData userData = (TableHockeyGameUserData)userDataManager.GetUserData(userKey);
            TableHockeyGameUserData opponentData = (TableHockeyGameUserData)userDataManager.GetUserData(userData.GetContent("opponentKey"));
            
            if (opponentData.GetContent("state")== "准备中")
            {
                opponentData.SetContent("state", "游戏中");
                userData.SetContent("state", "游戏中");
                SStartGameProtocol sp1 = new SStartGameProtocol();
                sp1.SetSocket(socket);
                sp1.SendContentBySocket();
                SStartGameProtocol sp2 = new SStartGameProtocol();
                sp2.SetSocket(opponentData.mySocket);
                sp2.SendContentBySocket();
            }
            else
            {
                userData.SetContent("state", "准备中");
            }
        }
    }

    class SStartGameProtocol: LLGameServerProtocol
    {
        public SStartGameProtocol() : base("SStartGameProtocol")
        {

        }
    }
}
