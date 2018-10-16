using Assets.Script.StoryNamespace.SceneNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Assets.Script.StoryNamespace.ActionNamespace
{
    /// <summary>
    /// 停止指令：让指定角色停止行动，未指定角色将停止所有角色
    /// </summary>
    class StopAction : ActionBase
    {
        public StopAction():base("Stop")
        {

        }

        protected override void Execute(ActorBase executor)
        {
            
        }

        protected override ActionBase CreateAction(XElement node)
        {
            StopAction action = new StopAction();
            action.LoadContent(node);
            return action;
        }

        protected override void LoadContent(XElement node)
        {
            base.LoadContent(node);
        }
    }
}
