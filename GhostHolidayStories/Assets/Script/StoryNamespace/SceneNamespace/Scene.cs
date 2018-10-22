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
        float width = 0;
        float height = 0;

        Dictionary<string, ActorBase> actorMap = new Dictionary<string, ActorBase>();

        World world = null;
        GameObject gameObject = null;
        Rect rect;

        private Scene(string scenePath, string name)
        {
            this.scenePath = scenePath;
            this.name = name;
        }

        public string GetName()
        {
            return name;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        /// <summary>
        /// 获得当前场景中所有演员
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, ActorBase> GetAllActor()
        {
            return actorMap;
        }

        /// <summary>
        /// 加载场景，未做重复检查
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
        /// 初始化场景本身
        /// </summary>
        void InitScene()
        {
            gameObject = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Actor/ActorPrefab"));
            gameObject.name = name;

            rect = new Rect(position, new Vector2(width, height));
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
                    case "name":
                        name = attribute.Value;
                        break;
                    case "width":
                        width = (float)Convert.ToDouble(attribute.Value);
                        break;
                    case "height":
                        height = (float)Convert.ToDouble(attribute.Value);
                        break;
                    default:
                        break;
                }
            }

            InitScene();

            foreach (var item in root.Elements())
            {
                ActorBase actor = ActorBase.LoadActor(item);
                if (actor!=null)
                {
                    actor.SetScene(this);
                }
                else
                {
                    GameManager.ShowErrorMessage("从场景中读取Actor:" + item.Name.ToString() + "失败！");
                }
            }
        }

        public void Update()
        {
            foreach (var item in actorMap)
            {
                item.Value.Update();
            }
        }

        /// <summary>
        /// 判断传入点是否在场景中
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool InScecneByPosition(Vector2 position)
        {
            return rect.Contains(position);
        }

        public void SetWorld(World world)
        {
            this.world = world;
            gameObject.transform.parent = world.GetGameObject().transform;
            gameObject.transform.localPosition = position;
        }

        /// <summary>
        /// 通过名称获得演员
        /// </summary>
        /// <param name="actorName"></param>
        /// <returns></returns>
        public ActorBase GetActor(string actorName)
        {
            if(actorMap.ContainsKey(actorName))
            {
                return actorMap[actorName];
            }
            return null;
        }

        /// <summary>
        /// 添加演员
        /// </summary>
        /// <param name="actor"></param>
        public void AddActor(ActorBase actor)
        {
            actorMap.Add(actor.GetName(), actor);
        }

        /// <summary>
        /// 移除演员
        /// </summary>
        public void RemoveActor(ActorBase actor)
        {
            actorMap.Remove(actor.GetName());
        }

        /// <summary>
        /// 清空场景
        /// </summary>
        public void Clear()
        {
            foreach (var item in actorMap)
            {
                GameObject.DestroyImmediate(item.Value.GetGameObject());
            }
            actorMap.Clear();
            GameObject.DestroyImmediate(gameObject);
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public float GetWidth()
        {
            return width;
        }

        public float GetHeight()
        {
            return height;
        }
    }
}
