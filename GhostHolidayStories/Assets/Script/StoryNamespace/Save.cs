using Assets.Script.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Script.StoryNamespace
{
    /// <summary>
    /// 存档类：存档文件命名规则：以“1”开头（“01”也可以），以此递增的xml文件。
    /// </summary>
    class Save
    {
        int index = 0;
        string savePath;
        DateTime createTime;
        DateTime lastSaveTime;
        TimeSpan playTime;
        int chapterIndex = 0;
        int sectionIndex = 0;
        int actionIndex = 0;
        string sceneName="";
        Vector2 position;

        Sprite image = null;

        private Save(string savePath, int index)
        {
            this.savePath = savePath;
            this.index = index;
        }

        /// <summary>
        /// 从文件中加载存档
        /// </summary>
        /// <param name="savePath"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Save LoadSave(string savePath, int index)
        {
            Save save = new Save(savePath, index);
            save.LoadContent();
            return save;
        }

        /// <summary>
        /// 创建存档
        /// </summary>
        /// <param name="savePath"></param>
        /// <returns></returns>
        public static Save CreateSave()
        {
            Story story = GameManager.GetCurrentStory();
            if(story==null)
            {
                GameManager.ShowErrorMessage("故事未加载，无法创建存档!");
                return null;
            }
            if(story.GetSaveNumber()<story.GetMaxSaveNumber())
            {
                int index = story.GetSaveNumber() + 1;
                string savePath = story.GetStoryPath() + "/Save/" + index+".xml";
                Save save = new Save(savePath, index);
                save.createTime = DateTime.Now;
                save.lastSaveTime = DateTime.Now;
                return save;
            }
            else
            {
                GameManager.ShowErrorMessage("已经达到存档数量上限：" + story.GetMaxSaveNumber());
                return null;
            }
        }

        /// <summary>
        /// 加载内容
        /// </summary>
        private void LoadContent()
        {
            XDocument doc = XDocument.Load(savePath);
            if(doc==null)
            {
                GameManager.ShowErrorMessage("存档"+ savePath + "不存在！");
                return;
            }

            XElement root = doc.Root;

            foreach (var attribute in root.Attributes())
            {
                switch (attribute.Name.ToString())
                {
                    case "createTime":
                        createTime = Convert.ToDateTime(attribute.Value);
                        break;
                    case "lastSaveTime":
                        lastSaveTime = Convert.ToDateTime(attribute.Value);
                        break;
                    case "chapterIndex":
                        chapterIndex = Convert.ToInt32(attribute.Value);
                        break;
                    case "sectionIndex":
                        sectionIndex = Convert.ToInt32(attribute.Value);
                        break;
                    case "actionIndex":
                        actionIndex = Convert.ToInt32(attribute.Value);
                        break;
                    case "sceneName":
                        sceneName = attribute.Value;
                        break;
                    case "position":
                        int tempIndex = attribute.Value.IndexOf(',');
                        float x = (float)Convert.ToDouble(attribute.Value.Substring(0, tempIndex));
                        float y = (float)Convert.ToDouble(attribute.Value.Substring(tempIndex+1));
                        position = new Vector2(x, y);
                        break;
                    default:
                        GameManager.ShowErrorMessage("未知属性：" + attribute.Name.ToString());
                        break;
                }
            }

            playTime = lastSaveTime - createTime;

            image = ImageHelper.LoadSprite(savePath.Substring(0, savePath.LastIndexOf(".")) + ".jpg");
        }

        /// <summary>
        /// 保存存档
        /// </summary>
        public void SaveGameData()
        {
            lastSaveTime = DateTime.Now;
            playTime = lastSaveTime - createTime;

            XDocument doc = new XDocument(
                new XElement("Save",
                    new XAttribute("createTime", createTime),
                    new XAttribute("lastSaveTime", lastSaveTime),
                    new XAttribute("chapterIndex", chapterIndex),
                    new XAttribute("sectionIndex", sectionIndex),
                    new XAttribute("sceneName", sceneName),
                    new XAttribute("position", position.x + "," + position.y)
                ));

            doc.Save(savePath);
        }

        public int GetChapterIndex()
        {
            return chapterIndex;
        }

        public int GetSectionIndex()
        {
            return sectionIndex;
        }
        
        public int GetActionIndex()
        {
            return actionIndex;
        }

        public string GetSceneName()
        {
            return sceneName;
        }

        public Sprite GetImage()
        {
            return image;
        }

        public TimeSpan GetPlayTime()
        {
            return playTime;
        }
    }
}
