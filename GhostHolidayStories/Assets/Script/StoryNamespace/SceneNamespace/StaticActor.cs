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
    /// 静态演员：用来展示静态物品，如房屋等
    /// </summary>
    class StaticActor : ActorBase
    {
        public StaticActor():base("Static")
        {

        }

        protected override ActorBase CreateActor(XElement node)
        {
            StaticActor actor = new StaticActor();
            actor.LoadContent(node);
            return actor;
        }

        protected override void LoadContent(XElement node)
        {
            base.LoadContent(node);

        }

    }
}
