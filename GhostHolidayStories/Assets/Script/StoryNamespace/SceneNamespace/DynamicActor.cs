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
    /// 动态演员：展示动态物品，如被风吹动的草等
    /// </summary>
    class DynamicActor : ActorBase
    {
        public DynamicActor() : base("Dynamic")
        {

        }

        protected override ActorBase CreateActor(XElement node)
        {
            DynamicActor actor = new DynamicActor();
            actor.LoadContent(node);
            return actor;
        }

        protected override void LoadContent(XElement node)
        {
            base.LoadContent(node);
        }
    }
}
