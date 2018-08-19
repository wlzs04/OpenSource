using Assets.Script;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Assets.StoryNamespace
{
    class Story
    {
        string name;
        string storyPath;

        //Save:与存档相关配置
        int maxSaveNumber = 10;
        int lastSaveIndex = 0;
        //Preview:与预览相关配置
        string description = "";
        string image = "";

        List<Save> saveList = new List<Save>();
        List<Chapter> chapterList = new List<Chapter>();

        public Story(string name)
        {
            this.name = name;
            storyPath = GameManager.GetInstance().GetStoriesPath() + name;
            LoadInfo();
        }

        /// <summary>
        /// 加载故事信息
        /// </summary>
        private void LoadInfo()
        {
            LoadConfig();
            LoadSave();
        }

        /// <summary>
        /// 加载故事配置
        /// </summary>
        private void LoadConfig()
        {
            XDocument doc = XDocument.Load(storyPath + "/Story.xml");
            XElement root = doc.Root;
            foreach (var item in root.Elements())
            {
                switch (item.Name.ToString())
                {
                    case "Save":
                        foreach (var attribute in item.Attributes())
                        {
                            switch (attribute.Name.ToString())
                            {
                                case "maxSaveNumber":
                                    maxSaveNumber = Convert.ToInt32(attribute.Value);
                                    break;
                                case "lastSaveIndex":
                                    lastSaveIndex = Convert.ToInt32(attribute.Value);
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case "Preview":
                        foreach (var attribute in item.Attributes())
                        {
                            switch (attribute.Name.ToString())
                            {
                                case "description":
                                    description = attribute.Value;
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 加载故事存档
        /// </summary>
        private void LoadSave()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(storyPath + "/Save/");

            foreach (var item in directoryInfo.GetFiles())
            {
                if(item.Extension==".xml")
                {
                    Save save = new Save(item.FullName, Convert.ToInt32(item.Name.Substring(0,item.Name.IndexOf('.'))));
                    save.LoadContent();
                    saveList.Add(save);
                }
            }
        }

        /// <summary>
        /// 加载故事完整内容
        /// </summary>
        private void LoadContent()
        {

        }

        public void Start()
        {

        }

        public void Continue(int index)
        {

        }

        private void LoadChapter(int index)
        {

        }
    }
}
