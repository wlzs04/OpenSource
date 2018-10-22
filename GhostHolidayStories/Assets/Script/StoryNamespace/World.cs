using Assets.Script.StoryNamespace.SceneNamespace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.StoryNamespace
{
    class World
    {
        Dictionary<string, Scene> sceneMap = new Dictionary<string, Scene>();

        GameObject gameObject = null;

        static World world = new World();

        private World()
        {
            gameObject = GameObject.Find("World");
        }

        public static World GetInstance()
        {
            return world;
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            foreach (var item in sceneMap)
            {
                item.Value.Update();
            }
        }

        /// <summary>
        /// 加载世界中所有场景
        /// </summary>
        public void LoadAllScene()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GameManager.GetCurrentStory().GetStoryPath() + "/Scene");

            foreach (var item in directoryInfo.GetFiles())
            {
                if (item.Extension == ".xml")
                {
                    string sceneName = item.Name.Substring(0, item.Name.IndexOf('.'));
                    Scene scene = Scene.LoadScene(item.FullName, sceneName);
                    if (scene != null)
                    {
                        AddScene(scene);
                    }
                    else
                    {
                        GameManager.ShowErrorMessage("加载场景：" + sceneName + "失败！");
                    }
                }
            }
        }

        /// <summary>
        /// 添加场景
        /// </summary>
        /// <param name="scene"></param>
        void AddScene(Scene scene)
        {
            if(sceneMap.ContainsKey(scene.GetName()))
            {
                GameManager.ShowErrorMessage("场景："+scene.GetName()+"已存在！");
            }
            else
            {
                sceneMap.Add(scene.GetName(), scene);
                scene.SetWorld(world);
            }
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        /// <summary>
        /// 在所有场景中寻找指定演员
        /// </summary>
        /// <param name="actorName"></param>
        /// <returns></returns>
        public ActorBase GetActor(string actorName)
        {
            ActorBase actor = null;
            foreach (var item in sceneMap)
            {
                actor = item.Value.GetActor(actorName);
                if (actor!=null)
                {
                    break;
                }
            }
            return actor;
        }

        /// <summary>
        /// 在指定场景中寻找指定演员
        /// </summary>
        /// <param name="actorName"></param>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public ActorBase GetActor(string actorName,string sceneName)
        {
            return sceneMap[sceneName].GetActor(actorName);
        }

        /// <summary>
        /// 通过名称获得指定场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public Scene GetScene(string sceneName)
        {
            if(sceneMap.ContainsKey(sceneName))
            {
                return sceneMap[sceneName];
            }
            return null;
        }

        /// <summary>
        /// 通过地点获得所在场景
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Scene GetSceneByPosition(Vector2 position)
        {
            foreach (var item in sceneMap)
            {
                if(item.Value.InScecneByPosition(position))
                {
                    return item.Value;
                }
            }
            return null;
        }

        /// <summary>
        /// 清理世界
        /// </summary>
        public void Clear()
        {
            foreach (var item in sceneMap)
            {
                item.Value.Clear();
            }
            sceneMap.Clear();
        }
    }
}
