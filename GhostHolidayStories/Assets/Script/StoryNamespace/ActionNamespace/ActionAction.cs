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

        public override void Execute()
        {
            GameManager.GetCurrentStory().AddAction(this);
            GameManager.GetCurrentStory().Action();
            actionCompleteCallBack.Invoke();
        }
    }
}
