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
    /// 触发演员：当主演与其发生碰撞时触发事件
    /// </summary>
    class TriggerActor : ActorBase
    {
        Rect rect;
        TrueAction action;
        bool haveTriggered = false;

        public TriggerActor() : base("Trigger")
        {

        }

        public override void Update()
        {
            base.Update();
            if(haveTriggered)
            {
                return;
            }
            if(DirectorActor.GetInstance().GetStarringActor()==null)
            {
                return;
            }
            if (rect.Contains(DirectorActor.GetInstance().GetStarringActor().GetPosition()))
            {
                foreach (var item in actionList)
                {
                    item.Init();
                    DirectorActor.GetInstance().AddActionToQueue(item);
                }
                haveTriggered = true;
            }
        }

        protected override ActorBase CreateActor(XElement node)
        {
            TriggerActor actor = new TriggerActor();
            actor.LoadContent(node);
            return actor;
        }

        protected override void LoadContent(XElement node)
        {
            base.LoadContent(node);
            rect = new Rect(position, new Vector2(width, height));
        }
    }
}
