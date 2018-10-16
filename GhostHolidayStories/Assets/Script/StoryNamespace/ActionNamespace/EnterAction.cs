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
        XElement enterActorElement = null;
        //ActorBase enterActor = null;

        public EnterAction():base("Enter")
        {

        }

        protected override void Execute(ActorBase executor)
        {
            ActorBase enterActor= ActorBase.LoadActor(enterActorElement);
            enterActor.SetScene(GameManager.GetCurrentStory().GetCurrentScene());
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
            base.LoadContent(node);
            if(node.HasElements)
            {
                enterActorElement = node.Elements().First();
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
