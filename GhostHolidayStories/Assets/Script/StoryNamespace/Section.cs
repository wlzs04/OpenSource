using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Script.StoryNamespace
{
    class Section
    {
        List<Action> actionList = new List<Action>();
        int index = 0;
        string sceneName;

        public void LoadContent(XElement node)
        {
            foreach (var item in node.Attributes())
            {
                switch (item.Name.ToString())
                {
                    case "index":
                        index = Convert.ToInt32(item.Value);
                        break;
                    case "sceneName":
                        sceneName = item.Value;
                        break;
                    default:
                        break;
                }
            }

            foreach (var item in node.Elements())
            {
                switch (item.Name.ToString())
                {
                    case "Action":
                        index = Convert.ToInt32(item.Value);
                        break;
                    default:
                        Debug.LogError("未知指令："+ item.Name.ToString());
                        break;
                }
            }
        }
    }
}
