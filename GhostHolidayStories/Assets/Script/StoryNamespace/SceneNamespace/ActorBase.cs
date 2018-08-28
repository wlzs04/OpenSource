using Assets.Script.Helper;
using Assets.Script.StoryNamespace.ActionNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Script.StoryNamespace.SceneNamespace
{
    /// <summary>
    /// 演员基类：场景中的一切都是演员
    /// </summary>
    abstract class ActorBase
    {
        string simpleActorClassName = "ActorBase";

        protected string name ="";//名称
        protected string imagePath = "";//图片路径
        protected float width = 0;//宽度
        protected float height = 0;//高度
        protected bool isBlock = false;//是否可以阻挡角色移动
        protected int layer = 1;//图片所在层级,0代表永远在最下层，1表示与按演员所在位置进行排序，2表示永远在上层
        protected Vector2 position;//位置
        protected bool canShow = true;//是否显示

        protected Texture2D texture = null;
        //protected Sprite image = null;
        protected GameObject gameObject = null;
        protected Scene scene =null;
        
        static Dictionary<string, ActorBase> legalActorMap = new Dictionary<string, ActorBase>();

        protected ActorBase(string simpleActorClassName)
        {
            this.simpleActorClassName = simpleActorClassName;
        }

        /// <summary>
        /// 更新
        /// </summary>
        public virtual void Update()
        {
        }

        /// <summary>
        /// 添加合法演员
        /// </summary>
        /// <param name="actorBase"></param>
        protected static void AddLegalActor(ActorBase actorBase)
        {
            legalActorMap.Add(actorBase.simpleActorClassName, actorBase);
        }

        /// <summary>
        /// 加载所有合法演员
        /// </summary>
        public static void LoadAllLegalActor()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(ActorBase));
            foreach (var item in assembly.GetTypes())
            {
                if(item.Namespace== typeof(ActorBase).Namespace)
                {
                    if (item.Name != "Scene" && item.Name != "ActorBase" && item.Name != "GameActor" & item.Name != "CameraActor")
                    {
                        AddLegalActor((ActorBase)assembly.CreateInstance(item.FullName));
                    }
                }
            }
        }

        /// <summary>
        /// 设置所在场景
        /// </summary>
        /// <param name="scene"></param>
        public void SetScene(Scene scene)
        {
            scene.AddActor(this);
            gameObject.transform.parent = scene.GetGameObject().transform;
            gameObject.transform.localPosition = position;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = layer;
            gameObject.GetComponent<SpriteRenderer>().size = new Vector2(width, height);
        }

        /// <summary>
        /// 让子类创建演员
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected abstract ActorBase CreateActor(XElement node);

        /// <summary>
        /// 加载演员
        /// </summary>
        /// <param name="node"></param>
        public static ActorBase LoadActor(XElement node)
        {
            string actorClassName = node.Name.ToString();
            ActorBase actor = null;

            if(legalActorMap.ContainsKey(actorClassName))
            {
                actor = legalActorMap[actorClassName].CreateActor(node);
            }
            else
            {
                GameManager.ShowErrorMessage("读取到未知类型的Actor:"+ node.Name.ToString()+ "！");
            }
            return actor;
        }

        /// <summary>
        /// 加载内容
        /// </summary>
        /// <param name="node"></param>
        protected virtual void LoadContent(XElement node)
        {
            gameObject = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Actor/ActorPrefab"));
            foreach (var attribute in node.Attributes())
            {
                switch (attribute.Name.ToString())
                {
                    case "name":
                        name = attribute.Value;
                        gameObject.name = name;
                        break;
                    case "image":
                        imagePath = attribute.Value;
                        break;
                    case "width":
                        width = (float)Convert.ToDouble(attribute.Value);
                        break;
                    case "height":
                        height = (float)Convert.ToDouble(attribute.Value);
                        break;
                    case "isBlock":
                        isBlock = Convert.ToBoolean(attribute.Value);
                        break;
                    case "layer":
                        layer = Convert.ToInt32(attribute.Value);
                        break;
                    case "position":
                        int tempIndex = attribute.Value.IndexOf(',');
                        float x = (float)Convert.ToDouble(attribute.Value.Substring(0, tempIndex));
                        float y = (float)Convert.ToDouble(attribute.Value.Substring(tempIndex + 1));
                        position = new Vector2(x, y);
                        break;
                    case "canShow":
                        canShow = Convert.ToBoolean(attribute.Value);
                        break;
                    default:
                        break;
                }
            }
            if(imagePath!="")
            {
                texture = ImageHelper.LoadTexture(GameManager.GetCurrentStory().GetStoryPath() + "/Texture/" + imagePath);
            }
        }
        
        /// <summary>
        /// 设置位置
        /// </summary>
        /// <param name="newPosition"></param>
        public void SetPosition(Vector2 newPosition)
        {
            position = newPosition;
            gameObject.transform.localPosition = position;
        }
        
        public string GetName()
        {
            return name;
        }

        public int GetLayer()
        {
            return layer;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}
