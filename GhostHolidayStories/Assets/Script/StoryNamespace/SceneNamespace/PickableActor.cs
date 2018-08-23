﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Assets.Script.StoryNamespace.SceneNamespace
{
    /// <summary>
    /// 可以拾起的演员：例如掉在地上的金钱等
    /// </summary>
    class PickableActor : ActorBase
    {
        bool playAnimation = true;
        bool canPick = true;

        List<ObjectItem> objectItemList = new List<ObjectItem>();

        public PickableActor() : base("Pickable")
        {

        }

        protected override ActorBase CreateActor(XElement node)
        {
            PickableActor pickableActor = new PickableActor();
            pickableActor.LoadContent(node);
            return pickableActor;
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
                    case "canPick":
                        canPick = Convert.ToBoolean(attribute.Value);
                        break;
                    default:
                        break;
                }
            }
            foreach (var item in node.Elements())
            {
                switch (item.Name.ToString())
                {
                    case "ObjectItem":
                        ObjectItem objectItem = ObjectItem.LoadObject(item);
                        objectItemList.Add(objectItem);
                        break;
                    default:
                        GameManager.ShowErrorMessage("PickableActor中出现未知子节点："+ item.Name.ToString());
                        break;
                }
            }
        }
    }
}