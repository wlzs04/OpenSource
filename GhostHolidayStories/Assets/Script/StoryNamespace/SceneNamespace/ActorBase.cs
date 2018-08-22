using Assets.Script.StoryNamespace.ActionNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
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
        string simpleActorClassName = "Actor";

        string name="";//名称
        string imagePath = "";//图片路径
        float width = 0;//宽度
        float height = 0;//高度
        bool isBlock = false;//是否可以阻挡角色移动
        int layer = 1;//图片所在层级,0代表永远在最下层，1表示与按演员所在位置进行排序，2表示永远在上层
        Vector2 position;//位置
        bool canShow = true;//是否显示

        static Dictionary<string, ActorBase> legalActorMap = new Dictionary<string, ActorBase>();

        protected ActorBase(string simpleActorClassName)
        {
            this.simpleActorClassName = simpleActorClassName;
        }

        protected static void AddLegalActor(ActorBase actorBase)
        {
            legalActorMap.Add(actorBase.simpleActorClassName, actorBase);
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

        protected virtual void LoadContent(XElement node)
        {
            foreach (var attribute in node.Attributes())
            {
                switch (attribute.Name.ToString())
                {
                    case "name":
                        name = attribute.Value;
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
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="action"></param>
        public void Action(Action action)
        {

        }

        /// <summary>
        /// 设置位置
        /// </summary>
        /// <param name="newPosition"></param>
        private void SetPosition(Vector2 newPosition)
        {

        }

        /// <summary>
        /// 角色移动到指定位置
        /// </summary>
        /// <param name="newPosition"></param>
        private void MoveToPosition(Vector2 newPosition,float needTime,MoveState moveState)
        {

        }
    }
}
