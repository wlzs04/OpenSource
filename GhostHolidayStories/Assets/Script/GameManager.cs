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
        Clean,//清空
        Init,//初始
        StoryList,//故事列表
        StoryContent,//故事内容
        Interlude,//过场
        Talk,//谈话
    }

    class GameManager
    {
        static GameManager gameManager = new GameManager();
        List<string> storyNameList = new List<string>();
        static Story currentStory = null;

        string storiesPath = "/Data/Stories/";

        Transform canvasTransform = null;
        UIState uiState;

        bool storyPlay = false;
        
        private GameManager()
        {
            canvasTransform = GameObject.Find("Canvas").transform;
            SetUI(UIState.Init);
            LoadAllStoryName();

            ActionBase.LoadAllLegalAction();
            ActorBase.LoadAllLegalActor();
        }

        public static GameManager GetInstance()
        {
            return gameManager;
        }

        /// <summary>
        /// 更新：每帧执行
        /// </summary>
        public void Update()
        {
            if(storyPlay)
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
        /// 获得故事集路径
        /// </summary>
        /// <returns></returns>
        public string GetStoriesPath()
        {
            return Application.dataPath + storiesPath;
        }

        /// <summary>
        /// 获得所有故事的名字
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllStoryName()
        {
            return storyNameList;
        }

        /// <summary>
        /// 加载所有故事的名字
        /// </summary>
        private void LoadAllStoryName()
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
        public void ChooseStory(string storyName)
        {
            currentStory = new Story(storyName);
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
        public void StartStory()
        {
            if(currentStory!=null)
            {
                currentStory.Start();
                storyPlay = true;
            }
        }

        /// <summary>
        /// 继续故事
        /// </summary>
        public void ContinueStory(int index)
        {
            if (currentStory != null)
            {
                currentStory.Continue(index);
                storyPlay = true;
            }
        }

        /// <summary>
        /// 设置UI
        /// </summary>
        public void SetUI(UIState uiState)
        {
            if(this.uiState == uiState)
            {
                return;
            }
            this.uiState = uiState;
            if(uiState==UIState.Clean)
            {
                if (canvasTransform.childCount != 0)
                {
                    GameObject.DestroyImmediate(canvasTransform.GetChild(0).gameObject);
                }
                return;
            }

            GameObject uiObject = Resources.Load<GameObject>("UI/"+ uiState.ToString() + "UIRootPrefab");
            if (uiObject != null)
            {
                if(canvasTransform.childCount!=0)
                {
                    GameObject.DestroyImmediate(canvasTransform.GetChild(0).gameObject);
                }
                GameObject.Instantiate(uiObject, canvasTransform).name= uiState.ToString() + "UIRootPrefab";
                switch (uiState)
                {
                    case UIState.Init:
                        SetUIInit();
                        break;
                    case UIState.StoryList:
                        SetUIStoryList();
                        break;
                    case UIState.StoryContent:
                        SetUIStoryContent();
                        break;
                    case UIState.Interlude:
                        SetUIInterlude();
                        break;
                    case UIState.Talk:
                        SetUITalk();
                        break;
                    default:
                        ShowErrorMessage("未知UI状态！");
                        break;
                }
            }
            else
            {
                ShowErrorMessage("没有与"+ uiState.ToString()+"匹配的UI预设。");
            }
        }

        /// <summary>
        /// 设置初始UI
        /// </summary>
        private void SetUIInit()
        {
            Button startButton = GameObject.Find("StartButton").GetComponent<Button>();
            Button configButton = GameObject.Find("ConfigButton").GetComponent<Button>();
            Button exitButton = GameObject.Find("ExitButton").GetComponent<Button>();

            startButton.onClick.AddListener(()=> { SetUI(UIState.StoryList); });
            configButton.onClick.AddListener(() => {  });
            exitButton.onClick.AddListener(() => { Exit(); });
        }

        /// <summary>
        /// 设置故事列表UI
        /// </summary>
        private void SetUIStoryList()
        {
            Button returnButton = GameObject.Find("ReturnButton").GetComponent<Button>();
            returnButton.onClick.AddListener(() => { SetUI(UIState.Init); });

            Transform contentTranfsorm = GameObject.Find("Content").transform;
            GameObject storyItemPrefab = Resources.Load<GameObject>("UI/StoryItemPrefab");
            foreach (var item in storyNameList)
            {
                GameObject storyItem = GameObject.Instantiate(storyItemPrefab, contentTranfsorm);
                storyItem.transform.Find("Text").GetComponent<Text>().text = item;
                storyItem.GetComponent<Button>().onClick.AddListener(() => { ChooseStory(item); SetUI(UIState.StoryContent); });
            }
        }

        /// <summary>
        /// 设置故事内容UI
        /// </summary>
        public void SetUIStoryContent()
        {
            Button returnButton = GameObject.Find("ReturnButton").GetComponent<Button>();
            returnButton.onClick.AddListener(() => { SetUI(UIState.StoryList); });

            Button startButton = GameObject.Find("StartButton").GetComponent<Button>();
            startButton.onClick.AddListener(() => { StartStory(); });

            Text descriptionText = GameObject.Find("DescriptionText").GetComponent<Text>();
            descriptionText.text = currentStory.GetDescription();

            Transform contentTranfsorm = GameObject.Find("Content").transform;
            GameObject saveItemPrefab = Resources.Load<GameObject>("UI/SaveItemPrefab");
            for ( int i =0;i< currentStory.GetSaveNumber();i++)
            {
                int saveIndex = i;
                Save save = currentStory.GetSaveList()[i];
                GameObject saveItem = GameObject.Instantiate(saveItemPrefab, contentTranfsorm);
                saveItem.transform.Find("Image").GetComponent<Image>().sprite = save.GetImage();
                saveItem.transform.Find("PlayTimeText").GetComponent<Text>().text = save.GetPlayTime().ToString();
                saveItem.transform.Find("ChapterAndSectionIndexText").GetComponent<Text>().text = "第" + (save.GetChapterIndex() + 1) + "章:第" + (save.GetSectionIndex() + 1) + "节";
                saveItem.AddComponent<Button>().onClick.AddListener(() => {
                    ContinueStory(saveIndex); });
            }
        }

        /// <summary>
        /// 设置过场UI
        /// </summary>
        public void SetUIInterlude()
        {

        }

        /// <summary>
        /// 设置谈话UI
        /// </summary>
        public void SetUITalk()
        {

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
    }
}
