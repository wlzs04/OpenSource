using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Assets.Script.StoryNamespace.SceneNamespace;

namespace Assets.Script.StoryNamespace.ActionNamespace
{
    /// <summary>
    /// 离场指令：指定演员离开场景
    /// </summary>
    class QuitAction : ActionBase
    {
        public QuitAction() : base("Quit")
        {

        }

        public override void Execute(ActorBase executor)
        {
            if(executor is DirectorActor)
            {
                World.GetInstance().GetActor(actorName).RemoveFromScene();
                Complete();
            }
            else
            {
                actorName = executor.GetName();
                DirectorActor.GetInstance().AddActionToQueue(this);
            }
        }

        protected override ActionBase CreateAction(XElement node)
        {
            QuitAction action = new QuitAction();
            action.LoadContent(node);
            return action;
        }

        public override ActorBase GetExecutor()
        {
            return DirectorActor.GetInstance();
        }
    }
}
