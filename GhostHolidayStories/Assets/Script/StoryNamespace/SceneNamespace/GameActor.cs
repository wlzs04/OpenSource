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
                    if(starringActor is MoveableActor)
                    {
                        float speed = (starringActor as MoveableActor).GetSpeed();
                        float mx = 0;
                        float my = 0;
                        if (Input.GetKey(KeyCode.W))
                        {
                            my += speed;
                        }
                        if (Input.GetKey(KeyCode.S))
                        {
                            my -= speed;
                        }
                        if (Input.GetKey(KeyCode.A))
                        {
                            mx -= speed;
                        }
                        if (Input.GetKey(KeyCode.D))
                        {
                            mx += speed;
                        }
                        if (mx != 0 || my != 0)
                        {
                            Vector2 position = starringActor.GetPosition();
                            starringActor.SetPosition(new Vector2(position.x + mx*Time.deltaTime, position.y + my * Time.deltaTime));
                        }
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
