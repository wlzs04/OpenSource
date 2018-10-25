using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Assets.Script.StoryNamespace.ActionNamespace
{
    /// <summary>
    /// 计时器跳过或结束指令：仅存在于计时器指令中，当玩家自动跳过计时或计时结束触发
    /// </summary>
    class PassAction : MultiplyAction
    {
        public PassAction() : base("Pass")
        {
        }

        protected override ActionBase CreateAction(XElement node)
        {
            PassAction action = new PassAction();
            action.LoadContent(node);
            return action;
        }
    }
}
