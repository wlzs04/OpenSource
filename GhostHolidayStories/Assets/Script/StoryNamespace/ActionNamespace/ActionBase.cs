using Assets.Script.StoryNamespace.SceneNamespace;
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
        Controlled,//角色受控制，玩家无法操作角色，一般用在剧情中
        Interactive,//角色交互中，玩家无法移动角色，但可以进行选择确定等操作，一般用在交互中
        Free,//角色由玩家控制
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
    /// 指令完成的回调
    /// </summary>
    public delegate void ActionCompleteCallBack();

    /// <summary>
    /// 指令基类：演员收到指令后进行相应行动
    /// </summary>
    abstract class ActionBase
    {
        string simpleActionClassName = "Action";//指令类型简称
        //所有合法指令类型
        static Dictionary<string, ActionBase> legalActionMap = new Dictionary<string, ActionBase>();

        //由文件配置的属性
        protected string actorName = "";//执行指令的演员
        protected ActionResult actionResult;//指令执行后的反馈
        protected bool isAsync = false;//是否此指令执行的同时执行下一条指令
        protected bool endAllAction = false;//判断此指令执行后结束以下所有指令
        protected bool executeByStarringActor = false;//是否由主角进行执行
        //是否需要保存到存档中，用于物品获取或NPC移动，保证读取存档后场景内容与保存时相同
        protected bool isNeedSaveToData = false;

        protected ActorBase executor = null;//执行者
        protected bool isCompleted = false;//是否完成

        protected ActionBase(string simpleActionClassName)
        {
            this.simpleActionClassName = simpleActionClassName;
        }

        public virtual ActorBase GetExecutor()
        {
            if(executeByStarringActor)
            {
                return DirectorActor.GetInstance().GetStarringActor();
            }
            if(actorName!="")
            {
                return World.GetInstance().GetActor(actorName);
            }
            return null;
        }

        /// <summary>
        /// 初始化指令，清空已完成等状态
        /// </summary>
        public virtual void Init()
        {
            isCompleted = false;
        }

        public virtual void Update()
        {

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
                    if (typeof(ActionBase).IsAssignableFrom(item) && !item.IsAbstract)
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
                    case "isAsync":
                        isAsync = Convert.ToBoolean(item.Value);
                        break;
                    case "executeByStarringActor":
                        executeByStarringActor = Convert.ToBoolean(item.Value);
                        break;
                    case "isNeedSaveToData":
                        isNeedSaveToData = Convert.ToBoolean(item.Value);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 导出内容
        /// </summary>
        /// <returns></returns>
        protected virtual XElement ExportContent()
        {
            GameManager.ShowErrorMessage(simpleActionClassName+"指令没有重写导出方法，无法导出！");
            return null;
        }

        /// <summary>
        /// 获得指令类型的名称
        /// </summary>
        /// <returns></returns>
        public string GetSimpleActionClassName()
        {
            return simpleActionClassName;
        }

        /// <summary>
        /// 获得执行者名称
        /// </summary>
        /// <returns></returns>
        public string GetActorName()
        {
            return actorName;
        }

        /// <summary>
        /// 执行指令：由子类来重写
        /// </summary>
        /// <param name="executor"></param>
        protected abstract void Execute(ActorBase executor);

        /// <summary>
        /// 执行指令的真正执行方法
        /// </summary>
        public void Execute()
        {
            GameManager.ShowDebugMessage("正在执行"+ simpleActionClassName + "指令！");
            Execute(GetExecutor());
        }

        /// <summary>
        /// 指令执行完成的处理方法
        /// </summary>
        protected void Complete()
        {
            if(!isCompleted)
            {
                isCompleted = true;
                CompleteAction();
                if (isNeedSaveToData)
                {
                    GameManager.ShowDebugMessage("将" + simpleActionClassName + "指令添加到当前存档中！");
                    DirectorActor.GetInstance().AddActionToSave(ExportContent());
                }
            }
        }

        /// <summary>
        /// 子类重写指令执行完成的处理方法
        /// </summary>
        protected virtual void CompleteAction()
        {
            
        }

        public bool IsAsync()
        {
            return isAsync;
        }

        public bool IsCompleted()
        {
            return isCompleted;
        }
    }
}
