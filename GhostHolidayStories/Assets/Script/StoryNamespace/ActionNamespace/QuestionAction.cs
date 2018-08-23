using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

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
    }
}
