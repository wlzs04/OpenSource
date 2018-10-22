using Assets.Script.Helper;
using Assets.Script.StoryNamespace.SceneNamespace;
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

        //需要保存的内容
        DateTime createTime;//创建时间
        DateTime lastSaveTime;//最后一次保存时间
        int chapterIndex = 0;//当前章
        int sectionIndex = 0;//当前节
        string sceneName = "";//所在场景
        XElement starringActorElement = null;//主演信息，仅以节点形式保存
        //已经完成的章节
        Dictionary<int, List<int>> haveFinishChapterAndSectionMap = new Dictionary<int, List<int>>();
        //需要在存档加载时执行的指令
        List<XElement> needExecuteOnLoadActionList = new List<XElement>();
        Dictionary<int, int> objectItemMap = new Dictionary<int, int>();
        Sprite image = null;
        TimeSpan playTime = new TimeSpan();

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
            if (story == null)
            {
                GameManager.ShowErrorMessage("故事未加载，无法创建存档!");
                return null;
            }
            if (story.GetSaveNumber() < story.GetMaxSaveNumber())
            {
                int index = story.GetSaveNumber();
                string savePath = story.GetStoryPath() + "/Save/" + index + ".xml";
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
            haveFinishChapterAndSectionMap.Clear();
            needExecuteOnLoadActionList.Clear();
            objectItemMap.Clear();
            XDocument doc = XDocument.Load(savePath);
            if (doc == null)
            {
                GameManager.ShowErrorMessage("存档" + savePath + "不存在！");
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
                    case "sceneName":
                        sceneName = attribute.Value;
                        break;
                    default:
                        GameManager.ShowErrorMessage("未知属性：" + attribute.Name.ToString());
                        break;
                }
            }

            foreach (var item in root.Elements())
            {
                if (item.Name == "Starring")
                {
                    starringActorElement = item.Elements().First();
                }
                if (item.Name == "HaveFinishChapterAndSection")
                {
                    foreach (var chapter in item.Elements())
                    {
                        int chapterIndex = Convert.ToInt32(chapter.Attribute("index").Value);
                        haveFinishChapterAndSectionMap[chapterIndex] = new List<int>();
                        foreach (var section in chapter.Elements())
                        {
                            int sectionIndex = Convert.ToInt32(section.Attribute("index").Value);
                            haveFinishChapterAndSectionMap[chapterIndex].Add(sectionIndex);
                        }
                    }
                }
                if (item.Name == "NeedExecuteOnLoadAction")
                {
                    foreach (var action in item.Elements())
                    {
                        needExecuteOnLoadActionList.Add(action);
                    }
                }
                if (item.Name == "ObjectItemMap")
                {
                    foreach (var objectItem in item.Elements())
                    {
                        int id = Convert.ToInt32(objectItem.Attribute("id").Value);
                        int count = Convert.ToInt32(objectItem.Attribute("count").Value);
                        objectItemMap[id] = count;
                    }
                }
            }
            image = ImageHelper.LoadSprite(savePath.Substring(0, savePath.LastIndexOf(".")) + ".jpg");
        }

        /// <summary>
        /// 保存存档
        /// </summary>
        public void SaveGameData()
        {
            lastSaveTime = DateTime.Now;
            XDocument doc = new XDocument(
                new XElement("Save",
                    new XAttribute("createTime", createTime.ToString("yyyy-MM-dd HH:mm:ss")),
                    new XAttribute("lastSaveTime", lastSaveTime.ToString("yyyy-MM-dd HH:mm:ss")),
                    new XAttribute("chapterIndex", chapterIndex),
                    new XAttribute("sectionIndex", sectionIndex),
                    new XAttribute("sceneName", DirectorActor.GetCurrentScene().GetName())
                ));

            //暂时将主演信息以节点形式导出
            starringActorElement = DirectorActor.GetInstance().GetStarringActor().ExportContent();

            XElement chapterAndSectionElement = new XElement("HaveFinishChapterAndSection");
            foreach (var chapter in haveFinishChapterAndSectionMap)
            {
                XElement chapterElement = new XElement("Chapter",
                    new XAttribute("index", chapter.Key));
                foreach (var section in chapter.Value)
                {
                    XElement sectionElement = new XElement("Section",
                        new XAttribute("index", section));
                    chapterElement.Add(sectionElement);
                }
                chapterAndSectionElement.Add(chapterElement);
            }

            XElement needExecuteOnLoadActionElement = new XElement("NeedExecuteOnLoadAction");
            foreach (var action in needExecuteOnLoadActionList)
            {
                needExecuteOnLoadActionElement.Add(action);
            }

            XElement objectItemMapElement = new XElement("ObjectItemMap");
            foreach (var objectItem in objectItemMap)
            {
                objectItemMapElement.Add(new XElement("ObjectItem",
                        new XAttribute("id", objectItem.Key),
                        new XAttribute("count", objectItem.Value)));
            }

            doc.Root.Add(new XElement("Starring", starringActorElement));
            doc.Root.Add(chapterAndSectionElement);
            doc.Root.Add(needExecuteOnLoadActionElement);
            doc.Root.Add(objectItemMapElement);
            doc.Save(savePath);
        }

        /// <summary>
        /// 获得存档图片路径
        /// </summary>
        public string GetImagePath()
        {
            return savePath.Substring(0, savePath.LastIndexOf(".")) + ".jpg";
        }

        public TimeSpan GetPlayTime()
        {
            return playTime;
        }

        public int GetChapterIndex()
        {
            return chapterIndex;
        }

        public int GetSectionIndex()
        {
            return sectionIndex;
        }

        public string GetSceneName()
        {
            return sceneName;
        }

        public Sprite GetImage()
        {
            return image;
        }

        public int GetIndex()
        {
            return index;
        }

        public XElement GetStarringActorElement()
        {
            return starringActorElement;
        }

        /// <summary>
        /// 判断当前存档是否已经完成了指定章节
        /// </summary>
        /// <param name="chapterIndex"></param>
        /// <param name="sectionIndex"></param>
        /// <returns></returns>
        public bool HaveFinishChapterAndSection(int chapterIndex, int sectionIndex)
        {
            foreach (var chapter in haveFinishChapterAndSectionMap)
            {
                if (chapter.Key == chapterIndex)
                {
                    foreach (var section in chapter.Value)
                    {
                        if (section == sectionIndex)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 添加完成指定章节
        /// </summary>
        /// <param name="chapterIndex"></param>
        /// <param name="sectionIndex"></param>
        public void AddFinishChapterAndSection(int chapterIndex, int sectionIndex)
        {
            this.chapterIndex = chapterIndex;
            this.sectionIndex = sectionIndex;
            if (haveFinishChapterAndSectionMap.ContainsKey(chapterIndex))
            {
                if (haveFinishChapterAndSectionMap[chapterIndex].Contains(sectionIndex))
                {
                    GameManager.ShowErrorMessage("当前存档已经完成指定章:" + chapterIndex + ",节：" + sectionIndex + "。请检查是否存在主线剧情逻辑错误！");
                }
                else
                {
                    haveFinishChapterAndSectionMap[chapterIndex].Add(sectionIndex);
                }
            }
            else
            {
                haveFinishChapterAndSectionMap[chapterIndex] = new List<int>() { sectionIndex };
            }
        }

        /// <summary>
        /// 添加需要在存档加载时执行的指令
        /// </summary>
        public void AddActionToSave(XElement node)
        {
            needExecuteOnLoadActionList.Add(node);
        }

        public List<XElement> GetNeedExecuteOnLoadActionList()
        {
            return needExecuteOnLoadActionList;
        }

        public Dictionary<int, int> GetObjectItemMap()
        {
            return objectItemMap;
        }

        public void AddObejct(int itemId, int number)
        {
            if (objectItemMap.ContainsKey(itemId))
            {
                objectItemMap[itemId] = objectItemMap[itemId] + number;
            }
            else
            {
                objectItemMap[itemId] = number;
            }
        }
        
        public void RemoveObejct(int itemId, int number)
        {
            if (objectItemMap.ContainsKey(itemId))
            {
                objectItemMap[itemId] = objectItemMap[itemId] - number > 0 ? objectItemMap[itemId] - number : 0;
            }
        }

        /// <summary>
        /// 获得指定物品的数量
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public int GetObjectNumberById(int itemId)
        {
            return objectItemMap.ContainsKey(itemId) ? objectItemMap[itemId] : 0;
        }
    }
}
