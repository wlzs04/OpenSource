using Assets.Script.StoryNamespace.SceneNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Script.StoryNamespace.ActionNamespace
{
    /// <summary>
    /// 等待指令
    /// </summary>
    class WaitAction:ActionBase
    {
        float needTime = 0;
        float startTime = 0;

        public WaitAction() : base("Wait")
        {

        }

        public override void Update()
        {
            base.Update();
            if(Time.time-startTime>= needTime)
            {
                Complete();
            }
        }

        protected override ActionBase CreateAction(XElement node)
        {
            WaitAction action = new WaitAction();
            action.LoadContent(node);
            return action;
        }

        public override ActorBase GetExecutor()
        {
            return DirectorActor.GetInstance();
        }

        protected override void Execute(ActorBase executor)
        {
            if (executor is DirectorActor)
            {
                startTime = Time.time;
            }
            else
            {
                GameManager.ShowErrorMessage("只有导演类可以使用Wait指令！");
                Complete();
            }
        }

        protected override void LoadContent(XElement node)
        {
            base.LoadContent(node);
            foreach (var attribute in node.Attributes())
            {
                switch (attribute.Name.ToString())
                {
                    case "needTime":
                        needTime = (float)Convert.ToDouble(attribute.Value);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
