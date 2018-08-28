using Assets.Script.StoryNamespace.ActionNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Script.StoryNamespace.SceneNamespace
{
    /// <summary>
    /// 游戏主体
    /// </summary>
    class GameActor : ActorBase
    {
        static GameActor gameActor = new GameActor();

        ActorBase starringActor = null;

        GameState gameState;

        private GameActor():base("Game")
        {

        }

        public static GameActor GetInstance()
        {
            return gameActor;
        }

        public override void Update()
        {
            base.Update();
            if(gameState==GameState.Free)
            {
                if (Input.GetKeyDown(KeyCode.J))
                {
                    if (CheckInteractive())
                    {

                    }
                }
                else
                {
                    Vector2 position = starringActor.GetPosition();
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        starringActor.SetPosition(new Vector2(position.x, position.y + 10));
                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        starringActor.SetPosition(new Vector2(position.x, position.y - 10));
                    }
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        starringActor.SetPosition(new Vector2(position.x-10, position.y));
                    }
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        starringActor.SetPosition(new Vector2(position.x+10, position.y));
                    }
                }
            }
        }

        protected override ActorBase CreateActor(XElement node)
        {
            GameManager.ShowErrorMessage("GameActor演员无法被创建！");
            return null;
        }

        /// <summary>
        /// 设置主演
        /// </summary>
        /// <param name="actor"></param>
        public void SetStarringActor(ActorBase actor)
        {
            if(starringActor!=null)
            {
                GameManager.ShowDebugMessage("主演由：" + starringActor.GetName() + "替换为：" + actor.GetName());
            }
            starringActor = actor;
        }

        /// <summary>
        /// 获得主演
        /// </summary>
        /// <returns></returns>
        public ActorBase GetStarringActor()
        {
            return starringActor;
        }

        public void SetGameState(GameState gameState)
        {
            this.gameState = gameState;
        }

        public GameState GetGameState()
        {
            return gameState;
        }

        /// <summary>
        /// 检测主演周围是否有可以进行交互的演员
        /// </summary>
        /// <returns></returns>
        public bool CheckInteractive()
        {
            return false;
        }
    }
}
