using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Assets.Script.StoryNamespace.ActionNamespace
{
    /// <summary>
    /// 随机指令：在指令列表中随机选择一项
    /// </summary>
    class RandomAction : ActionBase
    {
        List<ActionBase> actionList = new List<ActionBase>();

        public RandomAction() : base("Random")
        {
        }

        protected override ActionBase CreateAction(XElement node)
        {
            RandomAction action = new RandomAction();
            action.LoadContent(node);
            return action;
        }

        protected override void LoadContent(XElement node)
        {
            base.LoadContent(node);
            foreach (var item in node.Elements())
            {
                ActionBase action = LoadAction(item);
                if (action != null)
                {
                    actionList.Add(action);
                }
                else
                {
                    GameManager.ShowErrorMessage("在添加随机指令时指令加载失败！");
                }
            }
        }
    }
}
