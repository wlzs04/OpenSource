using Assets.Script;
using Assets.Script.StoryNamespace.ActionNamespace;
using Assets.Script.StoryNamespace.SceneNamespace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Script.StoryNamespace
{
    /// <summary>
    /// 故事UI状态
    /// </summary>
    enum StoryUIState
    {
        Hide,//隐藏
        Interlude,//过场
        Talk,//谈话
        Question,//问题：带选项
        Menu,//主菜单
    }

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
        List<ActionBase> actionList = new List<ActionBase>();

        World world = null;

        Save currentSave = null;

        DirectorActor directorActor = null;
        CameraActor cameraActor = null;

        Scene currentScene = null;

        bool actionStart = false;
        bool haveRemoveAction = false;

        public Story(string name)
        {
            this.name = name;
            storyPath = GameManager.GetStoriesPath() + name;
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            directorActor.Update();
            world.Update();
            cameraActor.Update();
        }

        public void Action()
        {
            actionStart = true;
            DirectorActor.UIHide();
        }

        public void Cut()
        {
            actionStart = false;
            DirectorActor.UIHide();
        }

        /// <summary>
        /// 加载故事信息
        /// </summary>
        public void LoadInfo()
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
                    Save save = Save.LoadSave(item.FullName, Convert.ToInt32(item.Name.Substring(0,item.Name.IndexOf('.'))));
                    saveList.Add(save);
                }
            }
        }

        /// <summary>
        /// 获得存档数量
        /// </summary>
        /// <returns></returns>
        public int GetSaveNumber()
        {
            return saveList.Count;
        }

        /// <summary>
        /// 获得最大存档数量
        /// </summary>
        /// <returns></returns>
        public int GetMaxSaveNumber()
        {
            return maxSaveNumber;
        }

        /// <summary>
        /// 获得描述
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            return description;
        }

        /// <summary>
        /// 获得存档列表
        /// </summary>
        /// <returns></returns>
        public List<Save> GetSaveList()
        {
            return saveList;
        }

        /// <summary>
        /// 获得指定章节
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Chapter GetChapterByIndex(int index)
        {
            return chapterList[index];
        }

        /// <summary>
        /// 获得故事路径
        /// </summary>
        /// <returns></returns>
        public string GetStoryPath()
        {
            return storyPath;
        }

        public World GetWorld()
        {
            return world;
        }

        public Scene GetCurrentScene()
        {
            return currentScene;
        }

        /// <summary>
        /// 通过名称获得场景
        /// </summary>
        /// <param name="sceneName"></param>
        public void SetCurrentSceneByName(string sceneName)
        {
            currentScene = world.GetScene(sceneName);
        }

        /// <summary>
        /// 加载故事完整内容
        /// </summary>
        private void LoadContent()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(storyPath + "/Chapter/");

            foreach (var item in directoryInfo.GetFiles())
            {
                if (item.Extension == ".xml")
                {
                    Chapter chapter = new Chapter(item.FullName, Convert.ToInt32(item.Name.Substring(0, item.Name.IndexOf('.'))));
                    chapterList.Add(chapter);
                }
            }

            LoadObject();
            LoadChapter(currentSave.GetChapterIndex());
            if(currentSave.GetSceneName()!="")
            {
                LoadScene(currentSave.GetSceneName());
                SetCurrentSceneByName(currentSave.GetSceneName());
            }
            directorActor = DirectorActor.GetInstance();
            cameraActor = CameraActor.GetInstance();

            //如果主演存在加载主演
            if(currentSave.GetStarringActorElement()!=null)
            {
                ActorBase actor = ActorBase.LoadActor(currentSave.GetStarringActorElement());
                actor.SetScene(currentScene);
                directorActor.SetStarringActor(actor);
                cameraActor.SetFollowActor(actor);
            }

            //执行存档中需要在进入游戏时执行的指令
            foreach (var item in currentSave.GetNeedExecuteOnLoadActionList())
            {
                ActionBase action = ActionBase.LoadAction(item);
                directorActor.AddActionToQueue(action);
            }
        }

        /// <summary>
        /// 开始
        /// </summary>
        public void Start()
        {
            currentSave = Save.CreateSave();
            if (currentSave == null)
            {
                GameManager.ShowDebugMessage("存档创建失败！");
                return;
            }
            saveList.Add(currentSave);
            Continue(currentSave.GetIndex()-1);
            StartContent();
        }

        /// <summary>
        /// 继续
        /// </summary>
        /// <param name="index"></param>
        public void Continue(int index)
        {
            world = World.GetInstance();
            currentSave = saveList[index];
            DirectorActor.GetInstance().Init();
            LoadContent();
            DirectorActor.UIHide();
        }

        /// <summary>
        /// 开始内容
        /// </summary>
        private void StartContent()
        {
            Section currentSection = chapterList[currentSave.GetChapterIndex()].GetSection(currentSave.GetSectionIndex());
            SetCurrentSceneByName(currentSection.GetSceneName());
            directorActor.AddStoryAction(currentSection.GetActionList());
            directorActor.StartStoryAction();
        }

        /// <summary>
        /// 开始新章节
        /// </summary>
        public void StartNewChapterSection(int chapterIndex,int sectionIndex)
        {
            Section currentSection = chapterList[chapterIndex].GetSection(sectionIndex);
            SetCurrentSceneByName(currentSection.GetSceneName());
            currentSave.AddFinishChapterAndSection(chapterIndex, sectionIndex);
            directorActor.AddStoryAction(currentSection.GetActionList());
            directorActor.StartStoryAction();
        }

        /// <summary>
        /// 加载章节
        /// </summary>
        /// <param name="index"></param>
        private void LoadChapter(int index)
        {
            if(chapterList.Count>index)
            {
                chapterList[index].LoadContent();
            }
            else
            {
                GameManager.ShowErrorMessage("故事"+name+"缺少章节："+index);
            }
        }

        /// <summary>
        /// 加载物品
        /// </summary>
        private void LoadObject()
        {
            ObjectItem.LoadConfig();
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        private void LoadScene(string sceneName)
        {
            Scene scene = Scene.LoadScene(storyPath+"/Scene/"+ sceneName + ".xml", sceneName);
            if(scene!=null)
            {
                world.AddScene(scene);
            }
            else
            {
                GameManager.ShowErrorMessage("加载场景："+ sceneName+"失败！");
            }
        }

        /// <summary>
        /// 获得当前章
        /// </summary>
        /// <returns></returns>
        public Chapter GetCurrentChapter()
        {
            return chapterList[currentSave.GetChapterIndex()];
        }

        /// <summary>
        /// 获得当前节
        /// </summary>
        /// <returns></returns>
        public Section GetCurrentSection()
        {
            return chapterList[currentSave.GetChapterIndex()].GetSection(currentSave.GetSectionIndex());
        }

        public Save GetCurrentSave()
        {
            return currentSave;
        }
    }
}
