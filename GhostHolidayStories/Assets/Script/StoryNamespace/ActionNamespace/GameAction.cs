using Assets.Script.StoryNamespace.SceneNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Assets.Script.StoryNamespace.ActionNamespace
{
    /// <summary>
    /// 游戏指令：用来控制游戏主体
    /// </summary>
    class GameAction : ActionBase
    {
        GameState gameState;

        public GameAction() : base("Game")
        {

        }

        public override void Execute()
        {
            GameManager.GetCurrentStory().AddAction(this);
            GameActor.GetInstance().SetGameState(gameState);
            actionCompleteCallBack.Invoke();
        }

        protected override ActionBase CreateAction(XElement node)
        {
            GameAction action = new GameAction();
            action.LoadContent(node);
            return action;
        }

        protected override void LoadContent(XElement node)
        {
            base.LoadContent(node);
            foreach (var attribute in node.Attributes())
            {
                switch (attribute.Name.ToString())
                {
                    case "gameState":
                        gameState = (GameState)Enum.Parse(typeof(GameState), attribute.Value);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
