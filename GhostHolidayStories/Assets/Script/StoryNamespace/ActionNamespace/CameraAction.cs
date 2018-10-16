using Assets.Script.StoryNamespace.SceneNamespace;
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
        float needTime = 0;
        MoveState moveState;

        public CameraAction() : base("Camera")
        {
        }

        protected override void Execute(ActorBase executor)
        {
            if (executor is CameraActor)
            {
                this.executor = executor;
                (executor as CameraActor).SetFollowActor(followActorName);

                switch (moveState)
                {
                    case MoveState.Set:
                        break;
                    case MoveState.AI:
                        break;
                    case MoveState.Line:
                        break;
                    case MoveState.Jump:
                        break;
                    default:
                        break;
                }
            }
            else
            {
                GameManager.ShowErrorMessage("只有摄像类可以使用Camera指令！");
            }
            Complete();
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

        public override ActorBase GetExecutor()
        {
            return CameraActor.GetInstance();
        }
    }
}
