using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Assets.Script.StoryNamespace.SceneNamespace;

namespace Assets.Script.StoryNamespace.ActionNamespace
{
    /// <summary>
    /// 真指令：仅存在于条件指令中，当满足条件是触发执行
    /// </summary>
    class TrueAction : MultiplyAction
    {
        public TrueAction() : base("True")
        {
        }

        protected override ActionBase CreateAction(XElement node)
        {
            TrueAction action = new TrueAction();
            action.LoadContent(node);
            return action;
        }
    }
}
