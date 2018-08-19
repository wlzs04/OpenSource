using Assets.StoryNamespace;
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
        Story currentStory = null;

        string storiesPath = "/Data/Stories/";

        private GameManager()
        {
            LoadAllStoryName();
        }

        public static GameManager GetInstance()
        {
            return gameManager;
        }

        public string GetStoriesPath()
        {
            return Application.dataPath + storiesPath;
        }

        public List<string> GetAllStoryName()
        {
            return storyNameList;
        }

        private void LoadAllStoryName()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetStoriesPath());

            foreach (var item in directoryInfo.GetDirectories())
            {
                storyNameList.Add(item.Name);
            }
        }

        public void ChooseStory(string storyName)
        {
            currentStory = new Story(storyName);
        }

        private void StartStory()
        {
            //currentStory.Start();
        }
    }
}
