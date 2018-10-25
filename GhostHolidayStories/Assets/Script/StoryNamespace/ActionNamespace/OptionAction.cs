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
    class OptionAction : MultiplyAction
    {
        string content = "";
        bool changeStateAfterSelect = false;
        bool endLoop = false;

        QuestionAction questionAction = null;//本选项所存在的问题指令
        int index = 0;//当前选项为问题的第几个选项
        bool haveSelected = false;//是否被选中过

        public OptionAction():base("Option")
        {
        }

        protected override void Execute(ActorBase executor)
        {
            GameManager.ShowDebugMessage(executor.GetName()+"选择了："+content);

            questionAction.ChooseOption(index);
            haveSelected = true;

            base.Execute(executor);
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
        }

        public override ActorBase GetExecutor()
        {
            return DirectorActor.GetInstance().GetStarringActor();
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
