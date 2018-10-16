using Assets.Script.StoryNamespace;
using Assets.Script.StoryNamespace.ActionNamespace;
using Assets.Script.StoryNamespace.SceneNamespace;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
namespace Assets.Script
{
    /// <summary>
    /// UI状态
    /// </summary>
    enum UIState
    {
        Hide,//隐藏
        Init,//初始
        StoryList,//故事列表
        StoryContent,//故事内容
    }

    class GameManager
    {
        static GameManager gameManager = new GameManager();
        static Story currentStory;
        static string commonPath;
        static string storiesPath;
        static Transform canvasTransform;

        static GameObject tipUIPrefab;
        static GameObject storyItemPrefab;
        static GameObject saveItemPrefab;
        static UIState uiState;
        static bool storyPlay;
        static bool uiHaveInit = false;//UI是否已经被初始化

        static List<string> storyNameList;

        static AudioPlayer audioPlayer;

        //UI根节点列表
        static Dictionary<UIState, GameObject> uiRootMap;

        private GameManager()
        {
            commonPath = "/Data/Common/";
            storiesPath = "/Data/Stories/";
            uiRootMap = new Dictionary<UIState, GameObject>();
            storyNameList = new List<string>();
            storyPlay = false;

            InitPrefab();
            InitUI();

            LoadAllStoryName();

            ActorBase.LoadAllLegalActor();
            ActionBase.LoadAllLegalAction();

            audioPlayer = GameObject.Find("AudioPlayer").GetComponent<AudioPlayer>();

            audioPlayer.PlayAudio(GetCommonPath()+"Audio/back.ogg");
        }

        public static GameManager GetInstance()
        {
            return gameManager;
        }

        /// <summary>
        /// 初始化预设
        /// </summary>
        void InitPrefab()
        {
            tipUIPrefab = Resources.Load<GameObject>("UI/TipUIPrefab");
            storyItemPrefab = Resources.Load<GameObject>("UI/StoryItemPrefab");
            saveItemPrefab = Resources.Load<GameObject>("UI/SaveItemPrefab");
        }

        /// <summary>
        /// 初始化UI
        /// </summary>
        void InitUI()
        {
            if(uiHaveInit)
            {
                return;
            }
            canvasTransform = GameObject.Find("Canvas").transform;
            foreach (UIState item in Enum.GetValues(typeof(UIState)))
            {
                if (item == UIState.Hide)
                {
                    continue;
                }
                GameObject uiPrefab = Resources.Load<GameObject>("UI/" + item.ToString() + "UIRootPrefab");
                if (uiPrefab == null)
                {
                    ShowErrorMessage("资源目录下缺少" + item.ToString() + "预设文件！");
                    continue;
                }
                GameObject uiRootObject = GameObject.Instantiate(uiPrefab, canvasTransform);
                uiRootObject.SetActive(false);
                uiRootObject.name = item.ToString() + "UIRootPrefab";
                uiRootMap.Add(item, uiRootObject);
            }

            InitInitUI();
            InitStoryListUI();
            InitStoryContentUI();

            SetUI(UIState.Init);
            uiHaveInit = true;
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
            uiHaveInit = false;
        }

        /// <summary>
        /// 更新：每帧执行
        /// </summary>
        public void Update()
        {
            if (storyPlay)
            {
                currentStory.Update();
            }
        }

        /// <summary>
        /// 显示调试信息
        /// </summary>
        /// <param name="message"></param>
        public static void ShowDebugMessage(object message)
        {
            UnityEngine.Debug.Log(message);
        }

        /// <summary>
        /// 显示错误信息
        /// </summary>
        /// <param name="message"></param>
        public static void ShowErrorMessage(object message)
        {
            UnityEngine.Debug.LogError(message);
        }

        /// <summary>
        /// 弹出提示框
        /// </summary>
        /// <param name="content"></param>
        /// <param name="autoDestroy"></param>
        public static void ShowTip(string content, bool autoDestroy = false, float showTime = 2)
        {
            GameObject.Instantiate(tipUIPrefab, canvasTransform).GetComponent<TipUIScript>().SetContent(content, autoDestroy, showTime);
        }

        /// <summary>
        /// 获得音乐播放器
        /// </summary>
        /// <returns></returns>
        public static AudioPlayer GetAudioPlayer()
        {
            return audioPlayer;
        }

        /// <summary>
        /// 获得故事集路径
        /// </summary>
        /// <returns></returns>
        public static string GetStoriesPath()
        {
            return Application.dataPath + storiesPath;
        }

        /// <summary>
        /// 获得通用路径
        /// </summary>
        /// <returns></returns>
        public static string GetCommonPath()
        {
            return Application.dataPath + commonPath;
        }

        /// <summary>
        /// 加载所有故事的名字
        /// </summary>
        void LoadAllStoryName()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetStoriesPath());

            foreach (var item in directoryInfo.GetDirectories())
            {
                storyNameList.Add(item.Name);
            }
        }

        /// <summary>
        /// 按故事名选择指定故事
        /// </summary>
        /// <param name="storyName"></param>
        public static void ChooseStory(string storyName)
        {
            currentStory = new Story(storyName);
            currentStory.LoadInfo();
        }

        /// <summary>
        /// 获得当前故事
        /// </summary>
        /// <returns></returns>
        public static Story GetCurrentStory()
        {
            return currentStory;
        }

        /// <summary>
        /// 开始新故事
        /// </summary>
        public static void StartStory()
        {
            if (currentStory != null)
            {
                gameManager.CleanUI();
                currentStory.Start();
                storyPlay = true;
            }
            else
            {
                ShowErrorMessage("当前尚未选择故事！");
            }
        }

        /// <summary>
        /// 继续故事
        /// </summary>
        public static void ContinueStory(int index)
        {
            if (currentStory != null)
            {
                gameManager.CleanUI();
                currentStory.Continue(index);
                storyPlay = true;
            }
            else
            {
                ShowErrorMessage("当前尚未选择故事！");
            }
        }

        /// <summary>
        /// 设置UI
        /// </summary>
        public static void SetUI(UIState newUIState)
        {
            if (uiState == newUIState)
            {
                return;
            }
            uiState = newUIState;
            for (int i = 0; i < canvasTransform.childCount; i++)
            {
                canvasTransform.GetChild(i).gameObject.SetActive(false);
            }
            if (uiState == UIState.Hide)
            {
                return;
            }

            if (uiRootMap.ContainsKey(uiState))
            {
                uiRootMap[uiState].SetActive(true);
                //如果有UI需要刷新内容，可以将方法添加到下方
                switch (uiState)
                {
                    case UIState.StoryList:
                        gameManager.RefreshStoryListUI();
                        break;
                    case UIState.StoryContent:
                        gameManager.RefreshStoryContentUI();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                ShowErrorMessage("没有与" + uiState.ToString() + "匹配的UI预设。");
            }
        }

        /// <summary>
        /// 初始化初始UI
        /// </summary>
        void InitInitUI()
        {
            Button startButton = uiRootMap[UIState.Init].transform.Find("OptionListPanel/StartButton").GetComponent<Button>();
            Button configButton = uiRootMap[UIState.Init].transform.Find("OptionListPanel/ConfigButton").GetComponent<Button>();
            Button exitButton = uiRootMap[UIState.Init].transform.Find("OptionListPanel/ExitButton").GetComponent<Button>();

            startButton.onClick.AddListener(() => { SetUI(UIState.StoryList); });
            configButton.onClick.AddListener(() => { });
            exitButton.onClick.AddListener(() => { Exit(); });
        }

        /// <summary>
        /// 初始化故事列表UI
        /// </summary>
        void InitStoryListUI()
        {
            Button returnButton = uiRootMap[UIState.StoryList].transform.Find("ReturnButton").GetComponent<Button>();
            returnButton.onClick.AddListener(() => { SetUI(UIState.Init); });
        }

        /// <summary>
        /// 刷新故事列表UI
        /// </summary>
        void RefreshStoryListUI()
        {
            Transform contentTranfsorm = uiRootMap[UIState.StoryList].transform.Find("StoryListScrollView/Viewport/Content").transform;
            for (int i = 0; i < contentTranfsorm.childCount; i++)
            {
                GameObject.DestroyImmediate(contentTranfsorm.GetChild(i).gameObject);
            }
            foreach (var item in storyNameList)
            {
                GameObject storyItem = GameObject.Instantiate(storyItemPrefab, contentTranfsorm);
                storyItem.transform.Find("Text").GetComponent<Text>().text = item;
                storyItem.GetComponent<Button>().onClick.AddListener(() => { ChooseStory(item); SetUI(UIState.StoryContent); });
            }
        }

        /// <summary>
        /// 初始化故事内容UI
        /// </summary>
        void InitStoryContentUI()
        {
            Button returnButton = uiRootMap[UIState.StoryContent].transform.Find("ReturnButton").GetComponent<Button>();
            returnButton.onClick.AddListener(() => { SetUI(UIState.StoryList); });

            Button startButton = uiRootMap[UIState.StoryContent].transform.Find("StartButton").GetComponent<Button>();
            startButton.onClick.AddListener(() => { StartStory(); });
        }

        /// <summary>
        /// 刷新故事内容UI
        /// </summary>
        void RefreshStoryContentUI()
        {
            Text descriptionText = uiRootMap[UIState.StoryContent].transform.Find("InfoPanel/DescriptionText").GetComponent<Text>();
            descriptionText.text = currentStory.GetDescription();

            Transform contentTranfsorm = uiRootMap[UIState.StoryContent].transform.Find("SaveListScrollView/Viewport/Content").transform;
            for (int i = 0; i < contentTranfsorm.childCount; i++)
            {
                GameObject.DestroyImmediate(contentTranfsorm.GetChild(i).gameObject);
            }
            for (int i = 0; i < currentStory.GetSaveNumber(); i++)
            {
                int saveIndex = i;
                Save save = currentStory.GetSaveList()[i];
                GameObject saveItem = GameObject.Instantiate(saveItemPrefab, contentTranfsorm);
                saveItem.transform.Find("Image").GetComponent<Image>().sprite = save.GetImage();
                saveItem.transform.Find("PlayTimeText").GetComponent<Text>().text = save.GetPlayTime().ToString();
                saveItem.transform.Find("ChapterAndSectionIndexText").GetComponent<Text>().text = "第" + (save.GetChapterIndex() + 1) + "章:第" + (save.GetSectionIndex() + 1) + "节";
                saveItem.AddComponent<Button>().onClick.AddListener(() =>
                {
                    ContinueStory(saveIndex);
                });
            }
        }

        /// <summary>
        /// 退出游戏
        /// </summary>
        public static void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
        }

        /// <summary>
        /// 退出故事
        /// </summary>
        public static void ExitStory()
        {
            storyPlay = false;
            currentStory = null;
            gameManager.InitUI();
        }
    }
}
