using Assets.Script.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Script.StoryNamespace.SceneNamespace
{
    /// <summary>
    /// 动态演员：展示动态物品，如被风吹动的草等,可以执行指令，
    /// 但不一定可以移动，此处的Dynamic与Unity所指含义不同
    /// </summary>
    class DynamicActor : ActorBase
    {
        public DynamicActor() : base("Dynamic")
        {

        }

        public override void Update()
        {
            base.Update();
        }

        protected override ActorBase CreateActor(XElement node)
        {
            DynamicActor actor = new DynamicActor();
            actor.LoadContent(node);
            return actor;
        }
    }
}
