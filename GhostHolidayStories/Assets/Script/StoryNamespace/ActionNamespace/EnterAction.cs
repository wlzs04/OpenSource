using Assets.Script.StoryNamespace.SceneNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Assets.Script.StoryNamespace.ActionNamespace
{
    /// <summary>
    /// 入场指令：添加一名演员到场景中
    /// </summary>
    class EnterAction : ActionBase
    {
        List<XElement> enterActorElementList = new List<XElement>();

        public EnterAction():base("Enter")
        {

        }

        protected override void Execute(ActorBase executor)
        {
            foreach (var item in enterActorElementList)
            {
                ActorBase enterActor = ActorBase.LoadActor(item);
                enterActor.SetScene(DirectorActor.GetCurrentScene());
            }
            Complete();
        }

        protected override ActionBase CreateAction(XElement node)
        {
            EnterAction action = new EnterAction();
            action.LoadContent(node);
            return action;
        }

        protected override void LoadContent(XElement node)
        {
            enterActorElementList.Clear();
            base.LoadContent(node);
            if(node.HasElements)
            {
                foreach (var item in node.Elements())
                {
                    enterActorElementList.Add(item);
                }
            }
            else
            {
                GameManager.ShowErrorMessage("入场指令中不存在演员！");
            }
        }

        public override ActorBase GetExecutor()
        {
            return DirectorActor.GetInstance();
        }
    }
}
