using Assets.Script.StoryNamespace.SceneNamespace;
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
        Random random = new Random();
        int selectedActionIndex = -1;

        public RandomAction() : base("Random")
        {
        }

        public override void Update()
        {
            base.Update();
            actionList[selectedActionIndex].Update();
            if (selectedActionIndex!=-1&& actionList[selectedActionIndex].IsCompleted())
            {
                Complete();
                actionList[selectedActionIndex].Init();
            }
        }

        public override void Execute(ActorBase executor)
        {
            if(actionList.Count<=0)
            {
                Complete();
                return;
            }
            selectedActionIndex = random.Next(actionList.Count);
            actionList[selectedActionIndex].Execute();
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

        protected override void Complete()
        {
            base.Complete();
        }

        public override void Init()
        {
            base.Init();
            selectedActionIndex = -1;
        }
    }
}
