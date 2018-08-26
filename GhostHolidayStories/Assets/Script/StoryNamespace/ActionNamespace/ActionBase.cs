using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Script.StoryNamespace.ActionNamespace
{
    /// <summary>
    /// 游戏状态
    /// </summary>
    enum GameState
    {
        Controlled,//角色受控制，玩家无法控制角色，一般用在剧情中。
        Free,//角色由玩家控制。
    }

    /// <summary>
    /// 移动状态
    /// </summary>
    enum MoveState
    {
        Set,//直接设置位置
        AI,//由计算机控制角色移动
        Line,//直线移动
        Jump//跳跃移动
    }

    /// <summary>
    /// 执行指令后的反馈
    /// </summary>
    enum ActionResult
    {
        Normal,//正常情况
        EndLoop,//结束循环
        EndAllAction,//结束之后的所有行为
    }

    /// <summary>
    /// 指令基类：演员收到指令后进行相应行动
    /// </summary>
    abstract class ActionBase
    {
        string simpleActionClassName = "Action";

        protected string actorName = "";
        protected bool endAllAction = false;

        static Dictionary<string, ActionBase> legalActionMap = new Dictionary<string, ActionBase>();

        protected ActionBase(string simpleActionClassName)
        {
            this.simpleActionClassName = simpleActionClassName;
        }

        /// <summary>
        /// 添加合法指令
        /// </summary>
        /// <param name="actionBase"></param>
        protected static void AddLegalAction(ActionBase actionBase)
        {
            legalActionMap.Add(actionBase.simpleActionClassName, actionBase);
        }

        /// <summary>
        /// 加载所有合法指令
        /// </summary>
        public static void LoadAllLegalAction()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(ActionBase));
            foreach (var item in assembly.GetTypes())
            {
                if (item.Namespace == typeof(ActionBase).Namespace)
                {
                    if (item.IsClass&& item.Name != "ActionBase")
                    {
                        AddLegalAction((ActionBase)assembly.CreateInstance(item.FullName));
                    }
                }
            }
        }

        /// <summary>
        /// 从节点中加载指令
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 加载内容
        /// </summary>
        /// <param name="node"></param>
        protected virtual void LoadContent(XElement node)
        {
            foreach (var item in node.Attributes())
            {
                switch (item.Name.ToString())
                {
                    case "actor":
                        actorName = item.Value;
                        break;
                    case "endAllAction":
                        endAllAction = Convert.ToBoolean(item.Value);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 获得指令类型的名称
        /// </summary>
        /// <returns></returns>
        public string GetSimpleActionClassName()
        {
            return simpleActionClassName;
        }
    }
}
