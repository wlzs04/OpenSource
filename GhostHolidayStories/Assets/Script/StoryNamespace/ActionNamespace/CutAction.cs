﻿using System;
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

        public override void Execute()
        {
            GameManager.GetCurrentStory().AddAction(this);
            GameManager.GetCurrentStory().Cut();
            actionCompleteCallBack.Invoke();
        }

        protected override ActionBase CreateAction(XElement node)
        {
            CutAction action = new CutAction();
            action.LoadContent(node);
            return action;
        }

        protected override void Complete()
        {
            GameManager.GetCurrentStory().RemoveAction(this);
        }
    }
}
