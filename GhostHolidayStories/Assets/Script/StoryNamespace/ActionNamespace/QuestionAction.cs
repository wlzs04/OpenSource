﻿using Assets.Script.StoryNamespace.SceneNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Script.StoryNamespace.ActionNamespace
{
    class QuestionAction:ActionBase
    {
        string content = "";
        bool showContent = true;
        bool isLoop = false;

        List<OptionAction> optionList = new List<OptionAction>();

        OptionAction currentOptionAction = null;

        public QuestionAction() : base("Question")
        {
        }

        public override void Update()
        {
            base.Update();
            if(currentOptionAction!=null)
            {
                if(!currentOptionAction.IsCompleted())
                {
                    currentOptionAction.Update();
                }
                else
                {
                    currentOptionAction.Init();
                    if(isLoop&& !currentOptionAction.GetEndLoop())
                    {
                        currentOptionAction = null;
                        ShowOption();
                    }
                    else
                    {
                        currentOptionAction = null;
                        Complete();
                    }
                }
            }
        }

        protected override void Execute(ActorBase executor)
        {
            if(showContent)
            {
                DirectorActor.UITalk(executor,content,"", ShowOption);
            }
            else
            {
                ShowOption();
            }
        }

        protected override ActionBase CreateAction(XElement node)
        {
            QuestionAction action = new QuestionAction();
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
                    case "content":
                        content = attribute.Value;
                        break;
                    case "showContent":
                        showContent = Convert.ToBoolean(attribute.Value);
                        break;
                    case "isLoop":
                        isLoop = Convert.ToBoolean(attribute.Value);
                        break;
                    default:
                        break;
                }
            }

            foreach (var item in node.Elements())
            {
                OptionAction action = (OptionAction)LoadAction(item);
                if (action != null)
                {
                    AddOption(action);
                }
                else
                {
                    GameManager.ShowErrorMessage("在添加选项指令时指令加载失败！");
                }
            }
        }

        /// <summary>
        /// 向问题指令中添加选项指令
        /// </summary>
        /// <param name="optionAction"></param>
        void AddOption(OptionAction optionAction)
        {
            optionAction.SetQuestionActionAndIndex(this, optionList.Count);
            optionList.Add(optionAction);
        }

        /// <summary>
        /// 显示选项
        /// </summary>
        protected void ShowOption()
        {
            DirectorActor.UIQuestion(optionList);
        }

        /// <summary>
        /// 选择指定选项
        /// </summary>
        /// <param name="index"></param>
        public void ChooseOption(int index)
        {
            currentOptionAction = optionList[index];
        }
    }
}
