using Assets.Script.Helper;
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
    /// 导演
    /// </summary>
    class DirectorActor : ActorBase
    {
        static DirectorActor directorActor = new DirectorActor();

        ActorBase starringActor = null;

        static StoryUIState currentStoryUIState;

        GameState gameState= GameState.Free;

        Scene currentScene = null;

        bool isPlaying = false;//表演中,大范围，作为暂停的判断
        bool isAction = false;//表演中,小范围，判断演员是否在执行指令，一般用在剧情中

        //主演属性
        float influenceRange = 200;//影响的范围
        float speed = 500;//速度

        List<ActionBase> storyActionList = new List<ActionBase>();
        bool needExecuteStoryAction = false;//判断是否需要让演员执行指令

        int currentActionIndex = 0;//当前执行的指令位置

        Transform canvasTransform = null;

        //UI根节点列表
        static Dictionary<StoryUIState, GameObject> uiRootMap;

        //Dictionary<int, int> objectItemMap = new Dictionary<int, int>();

        Story story = null;
        Save currentSave = null;

        DateTime playFromTime;//游戏开始时间

        private DirectorActor() : base("Director")
        {
            
        }

        public static DirectorActor GetInstance()
        {
            return directorActor;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            InitUI();
            story = GameManager.GetCurrentStory();
            currentSave = story.GetCurrentSave();

            playFromTime = DateTime.Now;
        }

        public override void Update()
        {
            if(needExecuteStoryAction)
            {
                if(storyActionList[currentActionIndex].IsCompleted())
                {
                    for (int i = currentActionIndex+1; i < storyActionList.Count; i++)
                    {
                        currentActionIndex++;
                        ActionBase action = storyActionList[i];
                        action.GetExecutor().AddActionToQueue(action);
                        if (!action.IsAsync())
                        {
                            break;
                        }
                    }
                }
            }
            else if(gameState==GameState.Free) 
            {
                GetUserInput();
            }
            base.Update();
        }

        /// <summary>
        /// 获得玩家输入
        /// </summary>
        void GetUserInput()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if (gameState == GameState.Free)
                {
                    UIMenu();
                }
            }

            if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Space))
            {
                ActorBase actor = null;
                actor = CheckInteractive();
                if (actor != null)
                {
                    actor.Interactive(starringActor);
                }
            }
            else
            {
                if (starringActor != null)
                {
                    float move = speed * Time.deltaTime;
                    float mx = 0;
                    float my = 0;
                    if (Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.UpArrow))
                    {
                        my += move;
                    }
                    if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                    {
                        my -= move;
                    }
                    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                    {
                        mx -= move;
                    }
                    if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                    {
                        mx += move;
                    }
                    if (mx != 0 || my != 0)
                    {
                        Vector2 position = starringActor.GetWorldPosition();
                        starringActor.MoveToPosition(new Vector2(position.x + mx, position.y + my));
                        CheckCurrentScene();
                    }
                }
            }
        }

        protected override ActorBase CreateActor(XElement node)
        {
            GameManager.ShowErrorMessage("DirectorActor演员无法被创建！");
            return null;
        }

        /// <summary>
        /// 检测主演所在场景，主要用于主演主动移动后
        /// </summary>
        void CheckCurrentScene()
        {
            Scene newScene = World.GetInstance().GetSceneByPosition(starringActor.GetWorldPosition());
            if (newScene!=null)
            {
                if(newScene != currentScene)
                {
                    GameManager.ShowDebugMessage("当前主要从场景："+ currentScene + "移动到场景："+newScene+"中。");
                    currentScene = newScene;
                }
            }
            else
            {
                GameManager.ShowErrorMessage("当前主演已移动到地图外。");
            }
        }

        void InitUI()
        {
            uiRootMap = new Dictionary<StoryUIState, GameObject>();
            canvasTransform = GameObject.Find("Canvas").transform;

            foreach (StoryUIState item in Enum.GetValues(typeof(StoryUIState)))
            {
                if (item == StoryUIState.Hide)
                {
                    continue;
                }
                GameObject uiPrefab = Resources.Load<GameObject>("UI/" + item.ToString() + "UIRootPrefab");
                if (uiPrefab == null)
                {
                    GameManager.ShowErrorMessage("资源目录下缺少" + item.ToString() + "预设文件！");
                    continue;
                }
                GameObject uiRootObject = GameObject.Instantiate(uiPrefab, canvasTransform);
                uiRootObject.SetActive(false);
                uiRootObject.name = item.ToString() + "UIRootPrefab";
                uiRootMap.Add(item, uiRootObject);
            }
        }

        /// <summary>
        /// 清空UI
        /// </summary>
        void CleanUI()
        {
            foreach (var item in uiRootMap)
            {
                GameObject.DestroyImmediate(item.Value);
            }
            uiRootMap.Clear();
        }

        public static void UIHide()
        {
            currentStoryUIState = StoryUIState.Hide;
            foreach (var item in uiRootMap)
            {
                item.Value.SetActive(false);
            }
        }

        public static void UITalk(ActorBase actor,string content, string audio, ActionCompleteCallBack callBack)
        {
            UITalk(actor!=null?actor.GetName():"", content, audio, callBack);
        }

        public static void UITalk(string actorName, string content,string audio, ActionCompleteCallBack callBack)
        {
            UIHide();
            currentStoryUIState = StoryUIState.Talk;
            uiRootMap[currentStoryUIState].SetActive(true);
            TalkUIScript talkUIScript = uiRootMap[currentStoryUIState].GetComponent<TalkUIScript>();
            talkUIScript.Init();
            talkUIScript.SetActorName(actorName);
            talkUIScript.SetContent(content);
            talkUIScript.SetAudio(audio);
            talkUIScript.SetCompleteCallBack(callBack);
        }

        public static void UIQuestion(List<OptionAction> optionList)
        {
            UIHide();
            currentStoryUIState = StoryUIState.Question;
            uiRootMap[currentStoryUIState].SetActive(true);
            QuestionUIScript script = uiRootMap[currentStoryUIState].GetComponent<QuestionUIScript>();
            script.ClearAllOption();
            foreach (var item in optionList)
            {
                script.AddOption(item.GetContent(), () => { item.Execute(); }, item.GetSelectedMark());
            }
            script.SetFocusIndex(0);
        }

        public static void UIMenu()
        {
            UIHide();
            currentStoryUIState = StoryUIState.Menu;
            uiRootMap[currentStoryUIState].SetActive(true);
            uiRootMap[currentStoryUIState].GetComponent<MenuUIScript>().RefreshObjectList();
            directorActor.gameState = GameState.Controlled;
        }

        /// <summary>
        /// UI界面从菜单返回
        /// </summary>
        public static void UIReturnFromMenu()
        {
            UIHide();
            directorActor.gameState = GameState.Free;
        }

        /// <summary>
        /// 暂停
        /// </summary>
        void Pause()
        {
            isPlaying = false;
        }

        /// <summary>
        /// 开始
        /// </summary>
        void Start()
        {
            isPlaying = true;
        }

        /// <summary>
        /// 退出故事
        /// </summary>
        public static void ExitStory()
        {
            GameManager.ShowDebugMessage("退出当前故事,回到游戏主界面！");
            directorActor.starringActor = null;
            directorActor.storyActionList.Clear();
            //directorActor.objectItemMap.Clear();
            directorActor.SetGameState(GameState.Free);
            directorActor.CleanUI();
            World.GetInstance().Clear();
            directorActor.story = null;
            GameManager.ExitStory();
        }

        /// <summary>
        /// 设置主演
        /// </summary>
        /// <param name="actorName"></param>
        public void SetStarringActor(string actorName)
        {
            ActorBase actor = currentScene.GetActor(actorName);
            if (actor == null)
            {
                GameManager.ShowErrorMessage("在设置主演时没有找到指定演员：" + actorName);
                return;
            }
            SetStarringActor(actor);
        }

        /// <summary>
        /// 设置主演
        /// </summary>
        /// <param name="actor"></param>
        public void SetStarringActor(ActorBase actor)
        {
            if (actor == null)
            {
                GameManager.ShowErrorMessage("在设置主演时指定演员不存在！");
                return;
            }
            if (starringActor != null)
            {
                starringActor.SetCanMove(false);
                GameManager.ShowErrorMessage("主演:" + starringActor.GetName() + "被替换为：" + actor.GetName());
            }
            starringActor = actor;
            starringActor.SetCanMove(true);
        }

        /// <summary>
        /// 获得主演
        /// </summary>
        /// <returns></returns>
        public ActorBase GetStarringActor()
        {
            return starringActor;
        }

        public void SetGameState(GameState gameState)
        {
            this.gameState = gameState;
        }

        public GameState GetGameState()
        {
            return gameState;
        }

        /// <summary>
        /// 检测主演周围是否有可以进行交互的演员
        /// </summary>
        /// <returns></returns>
        public ActorBase CheckInteractive()
        {
            foreach (var item in currentScene.GetAllActor())
            {
                if (item.Value.CanInteractive() && GameHelper.CheckActorInArea(item.Value, starringActor.GetPosition(), influenceRange))
                {
                    return item.Value;
                }
            }
            return null;
        }

        /// <summary>
        /// 开始执行指令
        /// </summary>
        public void StartStoryAction()
        {
            needExecuteStoryAction = true;
            currentActionIndex = 0;
            storyActionList[currentActionIndex].Execute();
        }

        /// <summary>
        /// 添加接下来所有的指令，
        /// 一般用于主线、支线和角色之间对话的信息，
        /// 为章节文件传入主线指令或角色自带指令
        /// </summary>
        /// <param name="actionList"></param>
        public void AddStoryAction(List<ActionBase> actionList)
        {
            if(actionList.Count>0)
            {
                foreach (var item in actionList)
                {
                    item.Init();
                    storyActionList.Add(item);
                }
                needExecuteStoryAction = true;
            }
        }

        /// <summary>
        /// 停止正在执行的指令
        /// </summary>
        public void StopExecuteStoryAction()
        {
            storyActionList.Clear();
            needExecuteStoryAction = false;
            currentActionIndex = 0;
        }

        /// <summary>
        /// 获得物品
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="number"></param>
        public void AddObejct(int itemId,int number)
        {
            currentSave.AddObejct(itemId,number);
        }

        /// <summary>
        /// 移除物品
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="number"></param>
        public void RemoveObejct(int itemId, int number)
        {
            currentSave.RemoveObejct(itemId, number);
        }

        /// <summary>
        /// 获得指定物品的数量
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public int GetObjectNumberById(int itemId)
        {
            return currentSave.GetObjectNumberById(itemId); 
        }

        public Dictionary<int,int> GetObjectItemMap()
        {
            return currentSave.GetObjectItemMap();
        }

        /// <summary>
        /// 获得金钱数量
        /// </summary>
        /// <returns></returns>
        public int GetMoney()
        {
            if(currentSave.GetObjectItemMap().ContainsKey(10001))
            {
                return currentSave.GetObjectItemMap()[10001];
            }
            return 0;

        }

        /// <summary>
        /// 判断当前存档是否已经完成了指定章节
        /// </summary>
        /// <param name="chapterIndex"></param>
        /// <param name="sectionIndex"></param>
        /// <returns></returns>
        public bool HaveFinishChapterAndSection(int chapterIndex,int sectionIndex)
        {
            return story.GetCurrentSave().HaveFinishChapterAndSection(chapterIndex, sectionIndex);
        }

        /// <summary>
        /// 保存当前存档到文件中
        /// </summary>
        public static void SaveGameData()
        {
            directorActor.story.GetCurrentSave().SaveGameData();
            Texture2D screenPng = ShotScreen();

            // 最后将这些纹理数据，成一个jpg图片文件  
            byte[] bytes = screenPng.EncodeToJPG();
            System.IO.File.WriteAllBytes(directorActor.story.GetCurrentSave().GetImagePath(), bytes);
            directorActor.story.GetCurrentSave().SaveGameData();
        }

        /// <summary>
        /// 获得当前所在场景
        /// </summary>
        /// <returns></returns>
        public static Scene GetCurrentScene()
        {
            return directorActor.currentScene;
        }

        /// <summary>
        /// 添加指令到存档中
        /// </summary>
        public void AddActionToSave(XElement action)
        {
            directorActor.story.GetCurrentSave().AddActionToSave(action);
        }

        /// <summary>
        /// 截屏
        /// </summary>
        /// <returns></returns>
        static Texture2D ShotScreen()
        {
            Camera camera = CameraActor.GetInstance().GetCamera();
            // 创建一个RenderTexture对象  
            RenderTexture rt = new RenderTexture(camera.pixelWidth, camera.pixelHeight, 0);
            // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机  
            camera.targetTexture = rt;
            camera.Render();
            //ps: --- 如果这样加上第二个相机，可以实现只截图某几个指定的相机一起看到的图像。  
            //ps: camera2.targetTexture = rt;  
            //ps: camera2.Render();  
            //ps: -------------------------------------------------------------------  

            // 激活这个rt, 并从中中读取像素。  
            RenderTexture.active = rt;
            Texture2D screenShot = new Texture2D(camera.pixelWidth, camera.pixelHeight, TextureFormat.RGB24, false);
            screenShot.ReadPixels(new Rect(0,0,camera.pixelWidth, camera.pixelHeight), 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素  
            screenShot.Apply();

            // 重置相关参数，以使用camera继续在屏幕上显示  
            camera.targetTexture = null;
            //ps: camera2.targetTexture = null;  
            RenderTexture.active = null; // JC: added to avoid errors  
            GameObject.Destroy(rt);
            
            return screenShot;
        }

        /// <summary>
        /// 获得本次游戏时间
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetThisTimePlayTimeSpan()
        {
            return DateTime.Now - playFromTime;
        }
    }
}
