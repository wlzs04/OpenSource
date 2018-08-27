using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Assets.Script.StoryNamespace.ActionNamespace
{
    /// <summary>
    /// 结束指令：剧本上的内容暂停
    /// </summary>
    class CutAction : ActionBase
    {
        public CutAction():base("Cut")
        {

        }

        public override void Execute()
        {
            
        }

        protected override ActionBase CreateAction(XElement node)
        {
            CutAction action = new CutAction();
            action.LoadContent(node);
            return action;
        }
    }
}
