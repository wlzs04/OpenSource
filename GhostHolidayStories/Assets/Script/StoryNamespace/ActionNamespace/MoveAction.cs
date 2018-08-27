using Assets.Script.StoryNamespace.SceneNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Script.StoryNamespace.ActionNamespace
{
    class MoveAction : ActionBase
    {
        Vector2 position;
        MoveState moveState;
        float needTime = 0;

        public MoveAction():base("Move")
        {

        }

        public override void Execute()
        {
            ActorBase actor = GameManager.GetCurrentStory().GetWorld().GetActor(actorName);
            actor.MoveToPosition(position, needTime, moveState);
        }

        protected override ActionBase CreateAction(XElement node)
        {
            MoveAction action = new MoveAction();
            action.LoadContent(node);
            return action;
        }

        protected override void LoadContent(XElement node)
        {
            base.LoadContent(node);
            foreach (var attribute in node.Attributes())
            {
                switch (attribute.Name.ToString())
                {
                    case "position":
                        int tempIndex = attribute.Value.IndexOf(',');
                        float x = (float)Convert.ToDouble(attribute.Value.Substring(0, tempIndex));
                        float y = (float)Convert.ToDouble(attribute.Value.Substring(tempIndex + 1));
                        position = new Vector2(x, y);
                        break;
                    case "moveState":
                        moveState = (MoveState)Enum.Parse(typeof(MoveState), attribute.Value);
                        break;
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
