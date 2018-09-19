using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Script.StoryNamespace.SceneNamespace
{
    /// <summary>
    /// 可以拾起的演员：例如掉在地上的金钱等
    /// </summary>
    class PickableActor : ActorBase
    {
        bool playAnimation = true;
        
        public PickableActor() : base("Pickable")
        {
            canInteractive = true;
            actionExecuteCondition = ActionExecuteCondition.Interactive;
        }

        protected override ActorBase CreateActor(XElement node)
        {
            PickableActor actor = new PickableActor();
            actor.LoadContent(node);
            return actor;
        }

        protected override void LoadContent(XElement node)
        {
            base.LoadContent(node);
            foreach (var attribute in node.Attributes())
            {
                switch (attribute.Name.ToString())
                {
                    case "playAnimation":
                        playAnimation = Convert.ToBoolean(attribute.Value);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
