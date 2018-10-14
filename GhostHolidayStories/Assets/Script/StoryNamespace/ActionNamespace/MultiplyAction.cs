using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Assets.Script.StoryNamespace.SceneNamespace;

namespace Assets.Script.StoryNamespace.ActionNamespace
{
    /// <summary>
    /// 多重指令：多重指令的基类，此指令中包含多条指令，执行时可以将包含的指令按次序执行
    /// </summary>
    abstract class MultiplyAction:ActionBase
    {
        List<ActionBase> actionList = new List<ActionBase>();//当选择此项后进行的下一项指令
        int currentActionIndex = 0;//当前执行到了那个指令
        List<ActionBase> actionCacheList = new List<ActionBase>();

        public MultiplyAction(string simpleActionClassName) : base(simpleActionClassName)
        {
        }

        public override void Update()
        {
            base.Update();
            if (actionList[actionList.Count - 1].IsCompleted())
            {
                Complete();
            }
            for (int i = actionCacheList.Count - 1; i >= 0; i--)
            {
                actionCacheList[i].Update();
                if (actionCacheList[i].IsCompleted())
                {
                    actionCacheList.RemoveAt(i);
                }
            }
            if (actionList[currentActionIndex].IsCompleted() || actionList[currentActionIndex].IsAsync())
            {
                for (int i = currentActionIndex + 1; i < actionList.Count; i++)
                {
                    currentActionIndex++;
                    ActionBase action = actionList[i];
                    action.Execute();
                    actionCacheList.Add(action);
                    if (!action.IsAsync())
                    {
                        break;
                    }
                }
            }
        }

        public override void Execute(ActorBase executor)
        {
            if (actionList.Count == 0)
            {
                Complete();
            }
            else
            {
                ActionBase action = actionList[currentActionIndex];
                action.Execute();
                actionCacheList.Add(action);
            }
        }

        public override void Init()
        {
            base.Init();
            foreach (var item in actionList)
            {
                item.Init();
            }
            actionCacheList.Clear();
            currentActionIndex = 0;
        }

        protected override void LoadContent(XElement node)
        {
            base.LoadContent(node);
            foreach (var item in node.Elements())
            {
                actionList.Add(LoadAction(item));
            }
        }
    }
}
