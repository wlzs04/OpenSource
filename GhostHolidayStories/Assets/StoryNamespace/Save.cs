using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.StoryNamespace
{
    class Save
    {
        int index = 0;
        string savePath;
        DateTime createTime;
        DateTime lastSaveTime;
        int chapterIndex = 0;
        int sectionIndex = 0;
        string sceneName;
        Vector2 position;

        public Save(string savePath, int index)
        {
            this.savePath = savePath;
            this.index = index;
        }

        public void LoadContent()
        {
            XDocument doc = XDocument.Load(savePath);
            XElement root = doc.Root;

            foreach (var attribute in root.Attributes())
            {
                switch (attribute.Name.ToString())
                {
                    case "createTime":
                        createTime = Convert.ToDateTime(attribute.Value);
                        break;
                    case "lastSaveTime":
                        lastSaveTime = Convert.ToDateTime(attribute.Value);
                        break;
                    case "chapterIndex":
                        chapterIndex = Convert.ToInt32(attribute.Value);
                        break;
                    case "sectionIndex":
                        sectionIndex = Convert.ToInt32(attribute.Value);
                        break;
                    case "sceneName":
                        sceneName = attribute.Value;
                        break;
                    case "position":
                        int tempIndex = attribute.Value.IndexOf(',');
                        float x = (float)Convert.ToDouble(attribute.Value.Substring(0, tempIndex));
                        float y = (float)Convert.ToDouble(attribute.Value.Substring(tempIndex+1));
                        position = new Vector2(x, y);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
