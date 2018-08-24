using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Assets.Script.StoryNamespace.ActionNamespace
{
    /// <summary>
    /// 摄像机指令：控制摄像机行动
    /// </summary>
    class CameraAction : ActionBase
    {
        string followActorName = "";
        bool isAsync = false;
        float needTime = 0;
        MoveState moveState;

        public CameraAction() : base("Camera")
        {
        }

        protected override ActionBase CreateAction(XElement node)
        {
            CameraAction action = new CameraAction();
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
                    case "followActor":
                        followActorName = attribute.Value;
                        break;
                    case "isAsync":
                        isAsync = Convert.ToBoolean(attribute.Value);
                        break;
                    case "needTime":
                        needTime = (float)Convert.ToDouble(attribute.Value);
                        break;
                    case "moveState":
                        moveState = (MoveState)Enum.Parse(typeof(MoveState), attribute.Value);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
