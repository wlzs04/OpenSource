using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Script.StoryNamespace.ActionNamespace
{
    enum GameState
    {
        Controlled,//角色受控制，玩家无法控制角色，一般用在剧情中。
        Free,//角色由玩家控制。
    }

    enum MoveState
    {
        AI,
        Line,
        Jump
    }

    /// <summary>
    /// 指令基类：演员收到指令后进行相应行动
    /// </summary>
    abstract class ActionBase
    {
        string simpleActionClassName = "Action";

        string sourceActorName = "";
        string targetActorName = "";
        bool backToLastAction = false;

        static Dictionary<string, ActionBase> legalActionMap = new Dictionary<string, ActionBase>();

        protected ActionBase(string simpleActionClassName)
        {
            this.simpleActionClassName = simpleActionClassName;
        }

        protected static void AddLegalAction(ActionBase actionBase)
        {
            legalActionMap.Add(actionBase.simpleActionClassName, actionBase);
        }

        public static ActionBase LoadAction(XElement node)
        {
            string actionClassName = node.Name.ToString();
            ActionBase action = null;

            if (legalActionMap.ContainsKey(actionClassName))
            {
                action = legalActionMap[actionClassName].CreateAction(node);
            }
            else
            {
                GameManager.ShowErrorMessage("读取到未知类型的Action:" + node.Name.ToString() + "！");
            }
            return action;
        }

        /// <summary>
        /// 让子类创建指令
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected abstract ActionBase CreateAction(XElement node);

        protected virtual void LoadContent(XElement node)
        {
            foreach (var item in node.Attributes())
            {

                switch (item.Name.ToString())
                {
                    case "sourceActor":
                        sourceActorName = item.Value;
                        break;
                    case "targetActorName":
                        targetActorName = item.Value;
                        break;
                    case "backToLastAction":
                        backToLastAction = Convert.ToBoolean(item.Value);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
