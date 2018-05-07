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
            string opponentKey = userData.GetContent("opponentKey");
            if (opponentKey != null)
            {
                TableHockeyGameUserData opponentData = (TableHockeyGameUserData)userDataManager.GetUserData(opponentKey);
                if (opponentData.GetContent("state") == "准备中")
                {
                    opponentData.SetContent("state", "游戏中");
                    userData.SetContent("state", "游戏中");
                    SStartGameProtocol sp1 = new SStartGameProtocol();
                    sp1.SetSocket(socket);
                    ServerManager.GetInstance().SendProtocol(sp1);
                    SStartGameProtocol sp2 = new SStartGameProtocol();
                    sp2.SetSocket(opponentData.mySocket);
                    ServerManager.GetInstance().SendProtocol(sp2);
                }
                else
                {
                    userData.SetContent("state", "准备中");
                }
            }
        }
    }

    class SStartGameProtocol : LLGameServerProtocol
    {
        public SStartGameProtocol() : base("SStartGameProtocol")
        {

        }
    }

    class CRestartGameProtocol : LLGameClientProtocol
    {
        public CRestartGameProtocol() : base("CRestartGameProtocol")
        {

        }

        public override LLGameClientProtocol GetInstance()
        {
            return new CRestartGameProtocol();
        }

        public override void Process()
        {
            UserDataManager userDataManager = UserDataManager.GetInstance();
            string userKey = (socket.RemoteEndPoint as IPEndPoint).ToString();
            TableHockeyGameUserData userData = (TableHockeyGameUserData)userDataManager.GetUserData(userKey);
            string opponentKey = userData.GetContent("opponentKey");
            if(opponentKey != null)
            {
                TableHockeyGameUserData opponentData = (TableHockeyGameUserData)userDataManager.GetUserData(opponentKey);
                if (opponentData.GetContent("state") == "准备中")
                {
                    opponentData.SetContent("state", "游戏中");
                    userData.SetContent("state", "游戏中");
                    SRestartGameProtocol sp1 = new SRestartGameProtocol();
                    sp1.SetSocket(socket);
                    ServerManager.GetInstance().SendProtocol(sp1);
                    SRestartGameProtocol sp2 = new SRestartGameProtocol();
                    sp2.SetSocket(opponentData.mySocket);
                    ServerManager.GetInstance().SendProtocol(sp2);
                }
                else
                {
                    userData.SetContent("state", "准备中");
                }
            }
        }
    }

    class SRestartGameProtocol : LLGameServerProtocol
    {
        public SRestartGameProtocol() : base("SRestartGameProtocol")
        {

        }
    }

    class CSendMyHandBallInfoProtocol : LLGameClientProtocol
    {
        public CSendMyHandBallInfoProtocol() : base("CSendMyHandBallInfoProtocol")
        {

        }

        public override LLGameClientProtocol GetInstance()
        {
            return new CSendMyHandBallInfoProtocol();
        }

        public override void Process()
        {
            UserDataManager userDataManager = UserDataManager.GetInstance();
            string userKey = (socket.RemoteEndPoint as IPEndPoint).ToString();
            TableHockeyGameUserData userData = (TableHockeyGameUserData)userDataManager.GetUserData(userKey);
            string opponentKey = userData.GetContent("opponentKey");
            if (opponentKey != null)
            {
                TableHockeyGameUserData opponentData = (TableHockeyGameUserData)userDataManager.GetUserData(opponentKey);
                SGetOpponentBallInfoProtocol sp = new SGetOpponentBallInfoProtocol();
                sp.AddContent("px", GetContent("px"));
                sp.AddContent("py", GetContent("py"));
                sp.SetSocket(opponentData.mySocket);
                ServerManager.GetInstance().SendProtocol(sp);
            }
        }
    }

    class SGetOpponentBallInfoProtocol : LLGameServerProtocol
    {
        public SGetOpponentBallInfoProtocol() : base("SGetOpponentBallInfoProtocol")
        {

        }
    }
}
