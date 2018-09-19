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
        ActorBase enterActor = null;

        public EnterAction():base("Enter")
        {

        }

        public override void Execute(ActorBase executor)
        {
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
            foreach (var item in node.Elements())
            {
                if(enterActor == null)
                {
                    enterActor = ActorBase.LoadActor(item);
                }
                else
                {
                    ActorBase newActor= ActorBase.LoadActor(item); ;
                    GameManager.ShowErrorMessage("入场的演员:"+ newActor .GetName()+ "已存在，将会被新演员："+ newActor.GetName()+ "代替！");
                    enterActor = newActor;
                }
            }
        }
    }
}
