using Assets.Script.StoryNamespace.ActionNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Script.StoryNamespace.SceneNamespace
{
    /// <summary>
    /// 可以进行交互的演员：例如路上的行人，具体如何交互由Action指令类控制
    /// </summary>
    class InteractiveActor : ActorBase
    {
        bool playDefaultAnimation = true;//是否播放动画

        int interactiveNumber = 0;//交互次数
        
        public InteractiveActor() : base("Interactive")
        {
            canInteractive = true;
            actionExecuteCondition = ActionExecuteCondition.Interactive;
        }

        public override void Update()
        {
            base.Update();
        }

        protected override ActorBase CreateActor(XElement node)
        {
            InteractiveActor actor = new InteractiveActor();
            actor.LoadContent(node);
            return actor;
        }

        /// <summary>
        /// 加载内容
        /// </summary>
        /// <param name="node"></param>
        protected override void LoadContent(XElement node)
        {
            base.LoadContent(node);

            foreach (var attribute in node.Attributes())
            {
                switch (attribute.Name.ToString())
                {
                    case "playDefaultAnimation":
                        playDefaultAnimation = Convert.ToBoolean(attribute.Value);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 开始交互
        /// </summary>
        public override void Interactive(ActorBase actor)
        {
            base.Interactive(actor);
            interactiveNumber++;
        }
    }
}
