using Assets.Script.Helper;
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
    /// 导演
    /// </summary>
    class DirectorActor : ActorBase
    {
        static DirectorActor directorActor = new DirectorActor();

        ActorBase starringActor = null;

        GameState gameState;

        float pickRange = 200;//主角可捡起物品的范围

        private DirectorActor() : base("Director")
        {

        }

        public static DirectorActor GetInstance()
        {
            return directorActor;
        }

        public override void Update()
        {
            base.Update();
            if (gameState == GameState.Free)
            {
                if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Space))
                {
                    ActorBase actor = null;
                    actor = CheckPickable();

                    if (actor != null)
                    {
                        StringBuilder stringBuffer = new StringBuilder();
                        stringBuffer.AppendLine("获得物品：");
                        stringBuffer.Append(actor.GetInfo());
                        actor.RemoveFromScene();
                        GameManager.ShowTip(stringBuffer.ToString());
                        return;
                    }
                    actor = CheckInteractive();
                    if (actor != null)
                    {
                        (actor as InteractiveActor).Interactive();
                    }
                }
                else
                {
                    if (starringActor is MoveableActor)
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
                            starringActor.SetPosition(new Vector2(position.x + mx * Time.deltaTime, position.y + my * Time.deltaTime));
                        }
                    }
                }
            }
        }

        protected override ActorBase CreateActor(XElement node)
        {
            GameManager.ShowErrorMessage("DirectorActor演员无法被创建！");
            return null;
        }

        public static void SetUI(StoryUIState ui)
        {

        }

        /// <summary>
        /// 设置主演
        /// </summary>
        /// <returns></returns>
        public void SetStarringActor(ActorBase actor)
        {
            if (starringActor != null)
            {
                GameManager.ShowErrorMessage("主演:" + starringActor.GetName() + "被替换为：" + actor.GetName());
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
        /// 检测主演周围是否有可以捡起的物品
        /// </summary>
        /// <returns></returns>
        public ActorBase CheckPickable()
        {
            foreach (var item in starringActor.GetScene().GetAllActor())
            {
                if (item.Value is PickableActor && GameHelper.CheckActorInArea(item.Value, starringActor.GetPosition(), pickRange))
                {
                    return item.Value;
                }
            }
            return null;
        }

        /// <summary>
        /// 检测主演周围是否有可以进行交互的演员
        /// </summary>
        /// <returns></returns>
        public ActorBase CheckInteractive()
        {
            foreach (var item in starringActor.GetScene().GetAllActor())
            {
                if (item.Value is InteractiveActor && GameHelper.CheckActorInArea(item.Value, starringActor.GetPosition(), pickRange))
                {
                    return item.Value;
                }
            }
            return null;
        }
    }
}
