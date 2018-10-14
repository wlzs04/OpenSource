using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Assets.Script.StoryNamespace.SceneNamespace;

namespace Assets.Script.StoryNamespace.ActionNamespace
{
    /// <summary>
    /// 假指令：仅存在于条件指令中，当不满足条件是触发执行
    /// </summary>
    class FalseAction : MultiplyAction
    {
        public FalseAction() : base("False")
        {
        }

        protected override ActionBase CreateAction(XElement node)
        {
            FalseAction action = new FalseAction();
            action.LoadContent(node);
            return action;
        }
    }
}
