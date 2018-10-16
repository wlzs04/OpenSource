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
    /// 指令：移动
    /// </summary>
    class MoveAction : ActionBase
    {
        Vector2 position;
        MoveState moveState;
        float needTime = 0;

        /// <summary>
        /// 与移动相关
        /// </summary>
        bool isMoving = false;
        float lastTime = 0;
        Vector2 lastPosition;
        Vector2 newPosition;

        ActorBase actor = null;

        public MoveAction():base("Move")
        {
        }

        public override void Update()
        {
            base.Update();
            if (isMoving)
            {
                if (Time.time - lastTime <= needTime)
                {
                    float rate = (Time.time - lastTime) / needTime;
                    float moveX = rate * (newPosition.x - lastPosition.x);
                    float moveY = rate * (newPosition.y - lastPosition.y);
                    actor.SetPosition(new Vector2(lastPosition.x + moveX, lastPosition.y + moveY));
                }
                else
                {
                    actor.SetPosition(newPosition);
                    isMoving = false;
                    Complete();
                }
            }
        }

        protected override void Execute(ActorBase executor)
        {
            lastTime = Time.time;
            lastPosition = executor.GetPosition();
            newPosition = position;
            actor = executor;
            switch (moveState)
            {
                case MoveState.Set:
                    executor.SetPosition(newPosition);
                    Complete();
                    break;
                case MoveState.AI:
                    isMoving = true;
                    break;
                case MoveState.Line:
                    isMoving = true;
                    break;
                case MoveState.Jump:
                    isMoving = true;
                    break;
                default:
                    break;
            }
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

        protected override XElement ExportContent()
        {
            return new XElement("Move",
                new XAttribute("actor",actorName),
                new XAttribute("position", position.x+","+position.y));
        }
    }
}
