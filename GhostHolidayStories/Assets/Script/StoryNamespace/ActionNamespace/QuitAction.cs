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
        bool inDirector = false;
        public QuitAction() : base("Quit")
        {

        }

        public override void Update()
        {
            base.Update();

            if(World.GetInstance().GetActor(actorName)==null)
            {
                Complete();
            }
            else if (inDirector)
            {
                World.GetInstance().GetActor(actorName).RemoveFromScene();
                Complete();
            }
        }

        protected override void Execute(ActorBase executor)
        {
            if (World.GetInstance().GetActor(actorName) == null)
            {
                Complete();
            }
            else if (inDirector)
            {
                World.GetInstance().GetActor(actorName).RemoveFromScene();
                Complete();
            }
            else
            {
                actorName = executor.GetName();
                DirectorActor.GetInstance().AddActionToQueue(this);
                inDirector = true;
            }
        }

        protected override ActionBase CreateAction(XElement node)
        {
            QuitAction action = new QuitAction();
            action.LoadContent(node);
            return action;
        }

        protected override XElement ExportContent()
        {
            return new XElement("Quit",new XAttribute("actor",actorName));
        }
    }
}
