using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Assets.Script.StoryNamespace.SceneNamespace;

namespace Assets.Script.StoryNamespace.ActionNamespace
{
    /// <summary>
    /// 指令：获得物品
    /// </summary>
    class GetObjectAction : ActionBase
    {
        List<ObjectItem> objectItemList = new List<ObjectItem>();

        public GetObjectAction() : base("GetObject")
        {
        }

        public override void Execute(ActorBase executor)
        {
            if (executor != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("获得物品：");

                foreach (var item in objectItemList)
                {
                    if (item == objectItemList[objectItemList.Count - 1])
                    {
                        stringBuilder.Append(item.GetName() + ":" + item.GetNumber());
                    }
                    else
                    {
                        stringBuilder.AppendLine(item.GetName() + ":" + item.GetNumber());
                    }
                }

                executor.RemoveFromScene();
                GameManager.ShowTip(stringBuilder.ToString());
            }
        }

        protected override ActionBase CreateAction(XElement node)
        {
            GetObjectAction action = new GetObjectAction();
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
                        GameManager.ShowErrorMessage("GetObjectAction中出现未知子节点：" + item.Name.ToString());
                        break;
                }
            }
        }
    }
}
