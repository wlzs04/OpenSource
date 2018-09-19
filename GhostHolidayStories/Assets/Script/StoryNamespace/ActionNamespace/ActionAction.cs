using Assets.Script.StoryNamespace.SceneNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Assets.Script.StoryNamespace.ActionNamespace
{
    /// <summary>
    /// 开始指令
    /// </summary>
    class ActionAction : ActionBase
    {
        public ActionAction() : base("Action")
        {

        }

        protected override ActionBase CreateAction(XElement node)
        {
            ActionAction action = new ActionAction();
            action.LoadContent(node);
            return action;
        }

        public override void Execute(ActorBase executor)
        {
            if(executor is DirectorActor)
            {
                this.executor = executor;
            }
            else
            {
                GameManager.ShowErrorMessage("只有导演类可以使用Action指令！");
            }
            Complete();
        }
    }
}
