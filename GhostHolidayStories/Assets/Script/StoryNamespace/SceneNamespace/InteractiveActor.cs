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
    /// 可以进行交互的演员：例如路上的行人，具体如何交互由Action指令类控制
    /// </summary>
    class InteractiveActor : ActorBase
    {
        protected Sprite image = null;
        bool playDefaultAnimation = true;//是否播放动画
        bool canTalk = true;//是否可以说话

        int interactiveNumber = 0;//交互次数

        List<ActionBase> actionList = new List<ActionBase>();

        public InteractiveActor() : base("Interactive")
        {

        }

        protected override ActorBase CreateActor(XElement node)
        {
            InteractiveActor actor = new InteractiveActor();
            actor.LoadContent(node);
            return actor;
        }

        /// <summary>
        /// 加载内容
        /// </summary>
        /// <param name="node"></param>
        protected override void LoadContent(XElement node)
        {
            base.LoadContent(node);
            foreach (var attribute in node.Attributes())
            {
                switch (attribute.Name.ToString())
                {
                    case "playDefaultAnimation":
                        playDefaultAnimation = Convert.ToBoolean(attribute.Value);
                        break;
                    case "canTalk":
                        canTalk = Convert.ToBoolean(attribute.Value);
                        break;
                    default:
                        break;
                }
            }
            foreach (var item in node.Elements())
            {
                ActionBase action = ActionBase.LoadAction(item);
                if (action != null)
                {
                    actionList.Add(action);
                }
                else
                {
                    GameManager.ShowErrorMessage("从InteractiveActor中读取Action:" + item.Name.ToString() + "失败！");
                }
            }

            image = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            gameObject.GetComponent<SpriteRenderer>().sprite = image;
        }
    }
}
