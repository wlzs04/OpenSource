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

        ActionBase nextAction = null;//当选择此项后进行的下一项指令

        public OptionAction():base("Option")
        {
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
                if(nextAction==null)
                {
                    nextAction = LoadAction(item);
                }
                else
                {
                    ActionBase newNextAction = LoadAction(item);
                    GameManager.ShowErrorMessage("选项指令的子指令："+nextAction.GetSimpleActionClassName()+"已存在，将会被新指令："+ newNextAction.GetSimpleActionClassName()+ "覆盖。");
                    nextAction = newNextAction;
                }
            }
        }
    }
}
