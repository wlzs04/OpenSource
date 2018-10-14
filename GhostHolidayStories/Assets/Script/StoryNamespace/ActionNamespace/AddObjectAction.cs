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
    class AddObjectAction : ActionBase
    {
        List<ObjectItem> objectItemList = new List<ObjectItem>();

        public AddObjectAction() : base("AddObject")
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
                    DirectorActor.GetInstance().AddObejct(item.GetItemId(), item.GetNumber());
                    if (item == objectItemList[objectItemList.Count - 1])
                    {
                        stringBuilder.Append(item.GetName() + ":" + item.GetNumber());
                    }
                    else
                    {
                        stringBuilder.AppendLine(item.GetName() + ":" + item.GetNumber());
                    }
                }
                GameManager.ShowTip(stringBuilder.ToString());
            }
            else
            {
                GameManager.ShowErrorMessage("获得物品指令执行时没有发现执行者。");

            }
            Complete();
        }

        protected override ActionBase CreateAction(XElement node)
        {
            AddObjectAction action = new AddObjectAction();
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
                        GameManager.ShowErrorMessage("AddObjectAction中出现未知子节点：" + item.Name.ToString());
                        break;
                }
            }
        }

        public override ActorBase GetExecutor()
        {
            return DirectorActor.GetInstance().GetStarringActor();
        }
    }
}
