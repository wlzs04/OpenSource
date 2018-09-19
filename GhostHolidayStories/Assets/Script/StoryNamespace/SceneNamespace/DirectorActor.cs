﻿using Assets.Script.Helper;
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

        Scene currentScene = null;

        bool isPlaying = false;//表演中,大范围，作为暂停的判断
        bool isAction = false;//表演中,小范围，判断演员是否在执行指令，一般用在剧情中

        //主演属性
        float influenceRange = 200;//影响的范围
        float speed = 200;//速度

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
            GetUserInput();
        }

        /// <summary>
        /// 获得玩家输入
        /// </summary>
        void GetUserInput()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Pause();
                return;
            }

            if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Space))
            {
                ActorBase actor = null;
                actor = CheckInteractive();
                if (actor != null)
                {
                    (actor as InteractiveActor).Interactive(starringActor);
                }
            }
            else
            {
                if (starringActor!=null)
                {
                    float move = speed * Time.deltaTime;
                    float mx = 0;
                    float my = 0;
                    if (Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.UpArrow))
                    {
                        my += move;
                    }
                    if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                    {
                        my -= move;
                    }
                    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                    {
                        mx -= move;
                    }
                    if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                    {
                        mx += move;
                    }
                    if (mx != 0 || my != 0)
                    {
                        Vector2 position = starringActor.GetPosition();
                        starringActor.SetPosition(new Vector2(position.x + mx, position.y + my));
                    }
                }
            }
        }

        protected override ActorBase CreateActor(XElement node)
        {
            GameManager.ShowErrorMessage("DirectorActor演员无法被创建！");
            return null;
        }

        /// <summary>
        /// 设置UI
        /// </summary>
        /// <param name="ui"></param>
        public static void SetUI(StoryUIState ui)
        {

        }

        public static void UITalk(ActorBase actor,string content,ActionCompleteCallBack callBack)
        {
            TalkUIScript talkUIScript = GameObject.Find("TalkUIRootPrefab").GetComponent<TalkUIScript>();
            talkUIScript.SetActorName(actor.GetName());
            talkUIScript.SetContent(content);

            talkUIScript.SetCompleteCallBack(callBack);
        }

        public static void UIQuestion(List<OptionAction> optionList)
        {
            QuestionUIScript script = GameObject.Find("QuestionUIRootPrefab").GetComponent<QuestionUIScript>();
            foreach (var item in optionList)
            {
                script.AddOption(item.GetContent(), () => { item.Execute(directorActor.GetStarringActor()); });
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        void Pause()
        {
            isPlaying = false;
        }

        /// <summary>
        /// 开始
        /// </summary>
        void Start()
        {
            isPlaying = true;
        }

        /// <summary>
        /// 设置主演
        /// </summary>
        /// <param name="actorName"></param>
        public void SetStarringActor(string actorName)
        {
            ActorBase actor = currentScene.GetActor(actorName);

            if (actor == null)
            {
                GameManager.ShowErrorMessage("在设置主演时没有找到指定演员："+actorName);
                return;
            }
            if (starringActor != null && actor != null)
            {
                GameManager.ShowErrorMessage("主演:" + starringActor.GetName() + "被替换为：" + actorName);
            }
            starringActor = actor;
        }

        /// <summary>
        /// 设置主演
        /// </summary>
        /// <param name="actor"></param>
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
        /// 检测主演周围是否有可以进行交互的演员
        /// </summary>
        /// <returns></returns>
        public ActorBase CheckInteractive()
        {
            foreach (var item in starringActor.GetScene().GetAllActor())
            {
                if (item.Value.CanInteractive() && GameHelper.CheckActorInArea(item.Value, starringActor.GetPosition(), influenceRange))
                {
                    return item.Value;
                }
            }
            return null;
        }
    }
}
