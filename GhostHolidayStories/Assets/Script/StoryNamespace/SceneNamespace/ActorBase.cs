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
    /// 指令执行条件
    /// </summary>
    enum ActionExecuteCondition
    {
        None,//无条件
        Interactive,//交互
        InRange,//在范围中
    }

    /// <summary>
    /// 演员基类：场景中的一切都是演员
    /// </summary>
    abstract class ActorBase
    {
        string simpleActorClassName = "ActorBase";//演员类型简称

        //所有合法演员类型
        static Dictionary<string, ActorBase> legalActorMap = new Dictionary<string, ActorBase>();

        //由文件配置的属性
        protected string name ="";//名称
        protected string imagePath = "";//图片路径
        protected float width = 0;//宽度
        protected float height = 0;//高度
        protected bool isBlock = false;//是否可以阻挡角色移动
        protected int layer = 1;//图片所在层级,0代表永远在最下层，1表示与按演员所在位置进行排序，2表示永远在上层
        protected Vector2 position;//位置
        protected bool canShow = true;//是否显示
        protected ActionExecuteCondition actionExecuteCondition;//包含的指令执行的条件
        protected bool canInteractive = false;//是否可以进行交互

        protected bool inExecution = false;//是否在指令执行中
        protected bool inInteractive = false;//是否在交互中
        protected Sprite image = null;//图片
        protected GameObject gameObject = null;
        protected Rigidbody2D rigidBody = null;
        protected BoxCollider2D boxCollider = null;
        protected Scene scene =null;//所在场景

        //演员本身拥有的指令列表
        List<ActionBase> actionList = new List<ActionBase>();
        //演员需要执行的指令队列
        Queue<ActionBase> actionQueue = new Queue<ActionBase>();
        //演员需要执行的指令队列缓存列表
        List<ActionBase> actionCacheList = new List<ActionBase>();

        protected ActorBase(string simpleActorClassName)
        {
            this.simpleActorClassName = simpleActorClassName;
        }

        /// <summary>
        /// 更新
        /// </summary>
        public virtual void Update()
        {
            if(inExecution)
            {
                ExecuteActionQueue();
            }
            else if(CheckSelfAction())
            {
                inExecution = true;
            }
        }

        /// <summary>
        /// 执行指令队列
        /// </summary>
        void ExecuteActionQueue()
        {
            foreach (var item in actionCacheList)
            {
                item.Update();
            }
            for (int i = actionCacheList.Count - 1; i >= 0; i--)
            {
                if (actionCacheList[i].IsCompleted())
                {
                    actionCacheList.RemoveAt(i);
                }
            }
            if(CheckActionCacheIsAsync())
            {
                while (actionQueue.Count > 0)
                {
                    ActionBase action = actionQueue.Dequeue();
                    actionCacheList.Add(action);
                    action.Execute(this);
                    if (!action.IsAsync())
                    {
                        break;
                    }
                }
            }
            if (actionCacheList.Count == 0&& actionQueue.Count==0)
            {
                inExecution = false;
                if(inInteractive)
                {
                    inInteractive = false;
                }
                DirectorActor.GetInstance().SetGameState(GameState.Free);
            }
        }

        /// <summary>
        /// 检测当前正在执行的指令是不是全部为可同步
        /// </summary>
        /// <returns></returns>
        bool CheckActionCacheIsAsync()
        {
            foreach (var item in actionCacheList)
            {
                if(!item.IsAsync())
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 检查演员自身是否携带有可执行的命令，
        /// 有的话根据条件添加到执行队列中
        /// </summary>
        /// <returns></returns>
        bool CheckSelfAction()
        {
            if (actionExecuteCondition == ActionExecuteCondition.None 
                && actionList.Count>0)
            {
                foreach (var item in actionList)
                {
                    actionQueue.Enqueue(item);
                }

                return true;
            }
            return false;
        }

        /// <summary>
        /// 添加合法演员
        /// </summary>
        /// <param name="actorBase"></param>
        static void AddLegalActor(ActorBase actorBase)
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
                    if (typeof(ActorBase).IsAssignableFrom(item)  && item.Name != "ActorBase"&&item.GetConstructors().Length!=0)
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
            this.scene = scene;
            scene.AddActor(this);
            gameObject.transform.parent = scene.GetGameObject().transform;
            gameObject.transform.localPosition = position;
            GameManager.ShowDebugMessage("演员：" + name + "入场："+ scene.GetName()+ "!");
        }

        /// <summary>
        /// 获得所在场景
        /// </summary>
        /// <returns></returns>
        public Scene GetScene()
        {
            return scene;
        }

        /// <summary>
        /// 从场景中移除
        /// </summary>
        public void RemoveFromScene()
        {
            scene.RemoveActor(this);
            gameObject.transform.parent = null;
            scene = null;
            GameObject.DestroyImmediate(gameObject);
            GameManager.ShowDebugMessage("演员：" + name + "退场！");
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
                    case "actionExecuteCondition":
                        actionExecuteCondition = (ActionExecuteCondition)Enum.Parse(typeof(ActionExecuteCondition), attribute.Value);
                        break;
                    default:
                        break;
                }
            }

            foreach (var item in node.Elements())
            {
                ActionBase action = ActionBase.LoadAction(item);
                actionList.Add(action);
            }

            InitActor();
        }

        /// <summary>
        /// 初始化演员
        /// </summary>
        void InitActor()
        {
            gameObject = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Actor/ActorPrefab"));
            rigidBody = gameObject.AddComponent<Rigidbody2D>();
            rigidBody.bodyType = RigidbodyType2D.Static;
            rigidBody.gravityScale = 0;
            rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            rigidBody.simulated = true;
            boxCollider = gameObject.AddComponent<BoxCollider2D>();

            gameObject.name = name;
            if (imagePath!="")
            {
                image = ImageHelper.LoadSprite(GameManager.GetCurrentStory().GetStoryPath() + "/Texture/" + imagePath);
                gameObject.GetComponent<SpriteRenderer>().sprite = image;
            }
            if (isBlock)
            {
                boxCollider.size = new Vector2(width, height);
                boxCollider.offset = new Vector2(width / 2, height / 2);
            }

            gameObject.GetComponent<SpriteRenderer>().sortingOrder = layer;
            gameObject.GetComponent<SpriteRenderer>().size = new Vector2(width, height);
        }

        public Vector2 GetSize()
        {
            return new Vector2(width, height);
        }

        /// <summary>
        /// 设置位置
        /// </summary>
        /// <param name="newPosition"></param>
        public void SetPosition(Vector2 newPosition)
        {
            gameObject.transform.localPosition = newPosition;
            position = newPosition;
        }

        /// <summary>
        /// 移动到位置
        /// </summary>
        /// <param name="newPosition"></param>
        public void MoveToPosition(Vector2 newPosition)
        {
            rigidBody.MovePosition(newPosition);
            position = gameObject.transform.localPosition;
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

        /// <summary>
        /// 获得演员信息
        /// </summary>
        /// <returns></returns>
        public virtual string GetInfo()
        {
            return "";
        }

        public bool CanInteractive()
        {
            return canInteractive;
        }

        /// <summary>
        /// 演员之间进行交互
        /// </summary>
        public virtual void Interactive(ActorBase actor)
        {
            if(inInteractive)
            {
                GameManager.ShowDebugMessage("演员"+name+"正在交互中！");
                return;
            }
            inInteractive = true;
            if (actionExecuteCondition==ActionExecuteCondition.Interactive)
            {
                foreach (var item in actionList)
                {
                    item.Init();
                    AddActionToQueue(item);
                }
                DirectorActor.GetInstance().SetGameState(GameState.Interactive);
            }
        }

        /// <summary>
        /// 结束当前所有动作
        /// </summary>
        public void Cut()
        {
            inInteractive = false;
            inExecution = false;

            actionQueue.Clear();
            actionCacheList.Clear();
        }

        /// <summary>
        /// 添加指令到执行队列
        /// </summary>
        public void AddActionToQueue(ActionBase action)
        {
            inExecution = true;
            actionQueue.Enqueue(action);
        }

        /// <summary>
        /// 设置演员是否可以移动，一般用于让主演移动
        /// </summary>
        /// <param name="canMove"></param>
        public void SetCanMove(bool canMove)
        {
            rigidBody.bodyType = canMove? RigidbodyType2D.Dynamic:RigidbodyType2D.Static;
        }
    }
}
