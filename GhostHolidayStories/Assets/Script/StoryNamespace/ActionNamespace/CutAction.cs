using Assets.Script.StoryNamespace.SceneNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Assets.Script.StoryNamespace.ActionNamespace
{
    /// <summary>
    /// 结束指令：结束剧本上的内容,让演员可以自由移动
    /// </summary>
    class CutAction : ActionBase
    {
        public CutAction():base("Cut")
        {

        }

        public override void Execute(ActorBase executor)
        {
            if (executor is DirectorActor)
            {
                this.executor = executor;
            }
            else
            {
                GameManager.ShowErrorMessage("只有导演类可以使用Cut指令！");
            }
            Complete();
        }

        protected override ActionBase CreateAction(XElement node)
        {
            CutAction action = new CutAction();
            action.LoadContent(node);
            return action;
        }

        protected override void Complete()
        {
            
        }
    }
}
