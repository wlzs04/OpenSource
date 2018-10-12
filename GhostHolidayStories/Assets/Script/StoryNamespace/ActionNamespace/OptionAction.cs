using Assets.Script.StoryNamespace.SceneNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Assets.Script.StoryNamespace.ActionNamespace
{
    /// <summary>
    /// 选项指令：一般用于对话中,由玩家在给定选项中进行选择
    /// </summary>
    class OptionAction : ActionBase
    {
        string content = "";
        bool changeStateAfterSelect = false;
        bool endLoop = false;

        QuestionAction questionAction = null;//本选项所存在的问题指令
        int index = 0;//当前选项为问题的第几个选项
        bool haveSelected = false;//是否被选中过

        List<ActionBase> actionList = new List<ActionBase>();//当选择此项后进行的下一项指令
        int currentActionIndex = 0;//当前执行到了那个指令
        List<ActionBase> actionCacheList = new List<ActionBase>();

        public OptionAction():base("Option")
        {
        }

        public override void Update()
        {
            base.Update();
            if(actionList[actionList.Count - 1].IsCompleted())
            {
                Complete();
            }
            for (int i = actionCacheList.Count-1; i >=0; i--)
            {
                actionCacheList[i].Update();
                if(actionCacheList[i].IsCompleted())
                {
                    actionCacheList.RemoveAt(i);
                }
            }
            if (actionList[currentActionIndex].IsCompleted()|| actionList[currentActionIndex].IsAsync())
            {
                for (int i = currentActionIndex + 1; i < actionList.Count; i++)
                {
                    currentActionIndex++;
                    ActionBase action = actionList[i];
                    action.Execute();
                    actionCacheList.Add(action);
                    if (!action.IsAsync())
                    {
                        break;
                    }
                }
            }
        }

        public override void Execute(ActorBase executor)
        {
            GameManager.ShowDebugMessage(executor.GetName()+"选择了："+content);

            questionAction.ChooseOption(index);
            haveSelected = true;

            if(actionList.Count == 0)
            {
                Complete();
            }
            else
            {
                ActionBase action = actionList[currentActionIndex];
                action.Execute();
                actionCacheList.Add(action);
            }
        }

        public override void Init()
        {
            base.Init();
            foreach (var item in actionList)
            {
                item.Init();
            }
            currentActionIndex = 0;
        }

        protected override ActionBase CreateAction(XElement node)
        {
            OptionAction action = new OptionAction();
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
                    case "changeStateAfterSelect":
                        changeStateAfterSelect = Convert.ToBoolean(attribute.Value);
                        break;
                    case "endLoop":
                        endLoop = Convert.ToBoolean(attribute.Value);
                        break;
                    default:
                        break;
                }
            }
            foreach (var item in node.Elements())
            {
                actionList.Add(LoadAction(item));
            }
        }

        protected override void Complete()
        {
            base.Complete();
        }

        public string GetContent()
        {
            return content;
        }

        public void SetQuestionActionAndIndex(QuestionAction questionAction,int index)
        {
            if(this.questionAction==null)
            {
                this.questionAction = questionAction;
                this.index = index;
            }
            else
            {
                GameManager.ShowErrorMessage("与选项关联的问题已经存在，有可能出现逻辑错误。");
            }
        }

        public bool GetEndLoop()
        {
            return endLoop;
        }

        /// <summary>
        /// 获得此选项是否有被选过的痕迹
        /// </summary>
        /// <returns></returns>
        public bool GetSelectedMark()
        {
            return changeStateAfterSelect && haveSelected;
        }
    }
}
