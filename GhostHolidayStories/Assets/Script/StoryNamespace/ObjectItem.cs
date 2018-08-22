using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Assets.Script.StoryNamespace
{
    struct ObjectInfo
    {
        public int itemId;//物品Id
        public string name;//名称
        public string description;//描述
    }

    class ObjectItem
    {
        int itemId;//物品Id
        int number = 1;//数量

        static Dictionary<int, ObjectInfo> objectInfoMap = new Dictionary<int, ObjectInfo>();

        public static void LoadConfig()
        {
            objectInfoMap.Clear();
            XDocument doc = XDocument.Load(GameManager.GetCurrentStory().GetStoryPath()+"/Config/ObjectInfo.xml");

            foreach (var item in doc.Root.Elements())
            {
                if(item.Name.ToString()== "ObjectInfo")
                {
                    ObjectInfo objectInfo = new ObjectInfo();
                    objectInfo.itemId = Convert.ToInt32(item.Attribute("itemId").Value);
                    objectInfo.name = item.Attribute("name").Value;
                    objectInfo.description = item.Attribute("description").Value;
                    objectInfoMap.Add(objectInfo.itemId, objectInfo);
                }
            }
        }

        /// <summary>
        /// 加载物品
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static ObjectItem LoadObject(XElement node)
        {
            ObjectItem objectItem = new ObjectItem();
            objectItem.LoadContent(node);
            return objectItem;
        }

        /// <summary>
        /// 加载内容
        /// </summary>
        /// <param name="node"></param>
        private void LoadContent(XElement node)
        {
            foreach (var attribute in node.Attributes())
            {
                switch (attribute.Name.ToString())
                {
                    case "itemId":
                        itemId = Convert.ToInt32(attribute.Value);
                        break;
                    case "number":
                        number = Convert.ToInt32(attribute.Value);
                        break;
                    default:
                        break;
                }
            }
        }

        public int GetItemId()
        {
            return itemId;
        }

        public int GetNumber()
        {
            return number;
        }

        public string GetName()
        {
            return objectInfoMap[itemId].name;
        }

        public string GetDescription()
        {
            return objectInfoMap[itemId].description;
        }
    }
}
