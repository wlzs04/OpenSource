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

        List<ActionBase> actionList = new List<ActionBase>();//当选择此项后进行的下一项指令

        public OptionAction():base("Option")
        {
        }

        public override void Execute(ActorBase executor)
        {
            //actionList[0].Execute();
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

        public string GetContent()
        {
            return content;
        }
    }
}
