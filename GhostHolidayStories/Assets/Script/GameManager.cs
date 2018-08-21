using Assets.Script.StoryNamespace;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script
{
    class GameManager
    {
        static GameManager gameManager = new GameManager();
        List<string> storyNameList = new List<string>();
        static Story currentStory = null;

        string storiesPath = "/Data/Stories/";

        private GameManager()
        {
            LoadAllStoryName();
        }

        public static GameManager GetInstance()
        {
            return gameManager;
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
    }
}
