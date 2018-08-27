using Assets.Script.StoryNamespace.ActionNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Assets.Script.StoryNamespace.SceneNamespace
{
    /// <summary>
    /// 游戏主体
    /// </summary>
    class GameActor : ActorBase
    {
        static GameActor gameActor = new GameActor();
        GameState gameState;

        private GameActor():base("Game")
        {

        }

        public static GameActor GetInstance()
        {
            return gameActor;
        }

        protected override ActorBase CreateActor(XElement node)
        {
            GameManager.ShowErrorMessage("GameActor演员无法被创建！");
            return null;
        }

        public void SetGameState(GameState gameState)
        {
            this.gameState = gameState;
        }

        public GameState GetGameState()
        {
            return gameState;
        }
    }
}
