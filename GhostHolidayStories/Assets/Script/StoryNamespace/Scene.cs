using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Script.StoryNamespace
{
    class Scene
    {
        string scenePath;
        string name;
        Vector2 position;



        private Scene(string scenePath, string name)
        {
            this.scenePath = scenePath;
            this.name = name;
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="scenePath"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Scene LoadScene(string scenePath,string name)
        {
            Scene scene = new Scene(scenePath, name);
            scene.LoadContent();
            return scene;
        }

        /// <summary>
        /// 加载内容
        /// </summary>
        private void LoadContent()
        {
            XDocument doc = XDocument.Load(scenePath);
            XElement root = doc.Root;

            foreach (var attribute in root.Attributes())
            {
                switch (attribute.Name.ToString())
                {
                    case "position":
                        int tempIndex = attribute.Value.IndexOf(',');
                        float x = (float)Convert.ToDouble(attribute.Value.Substring(0, tempIndex));
                        float y = (float)Convert.ToDouble(attribute.Value.Substring(tempIndex + 1));
                        position = new Vector2(x, y);
                        break;
                    default:
                        break;
                }
            }

            foreach (var item in root.Elements())
            {
                switch (item.Name.ToString())
                {
                    case "Static":
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
