using Assets.Script.StoryNamespace.SceneNamespace;
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

        public QuestionAction() : base("Question")
        {
        }

        public override void Execute()
        {
            if(showContent)
            {
                DirectorActor.SetUI(StoryUIState.Talk);
                TalkUIScript talkUIScript = GameObject.Find("TalkUIRootPrefab").GetComponent<TalkUIScript>();
                talkUIScript.SetActorName(actorName);
                talkUIScript.SetContent(content);

                talkUIScript.SetCompleteCallBack(ShowOption);
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
                    optionList.Add(action);
                }
                else
                {
                    GameManager.ShowErrorMessage("在添加选项指令时指令加载失败！");
                }
            }
        }

        protected override void Complete()
        {
            //base.Complete();
        }

        /// <summary>
        /// 显示选项
        /// </summary>
        protected void ShowOption()
        {
            DirectorActor.SetUI(StoryUIState.Question); 
            QuestionUIScript script = GameObject.Find("QuestionUIRootPrefab").GetComponent<QuestionUIScript>();
            foreach (var item in optionList)
            {
                script.AddOption(item.GetContent(), () => { item.Execute(); });
            }
        }
    }
}
