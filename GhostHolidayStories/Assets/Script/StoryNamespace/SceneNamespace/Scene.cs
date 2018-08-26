using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Script.StoryNamespace.SceneNamespace
{
    class Scene
    {
        string scenePath;
        string name;
        Vector2 position;

        List<ActorBase> actorList = new List<ActorBase>();

        GameObject world = null;
        GameObject gameObject = null;

        private Scene(string scenePath, string name)
        {
            this.scenePath = scenePath;
            this.name = name;

            gameObject = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Actor/ActorPrefab"));
            gameObject.name = name;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
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
                ActorBase actor = ActorBase.LoadActor(item);
                if (actor!=null)
                {
                    actorList.Add(actor);
                    actor.SetScene(this);
                }
                else
                {
                    GameManager.ShowErrorMessage("从场景中读取Actor:" + item.Name.ToString() + "失败！");
                }
            }

            actorList.Sort(SortRule);
        }

        public void Update()
        {
            
        }

        public void SetWorld(GameObject world)
        {
            this.world = world;
            gameObject.transform.parent = world.transform;
            gameObject.transform.localPosition = position;
        }

        /// <summary>
        /// 场景内的演员的排序规则
        /// </summary>
        private int SortRule(ActorBase actor0, ActorBase actor1)
        {
            if(actor0.GetLayer()> actor1.GetLayer())
            {
                return 1;
            }
            else if (actor0.GetLayer() < actor1.GetLayer())
            {
                return -1;
            }
            else
            {
                if(actor0.GetPosition().y> actor1.GetPosition().y)
                {
                    return 1;
                }
                return -1;
            }
        }
    }
}
