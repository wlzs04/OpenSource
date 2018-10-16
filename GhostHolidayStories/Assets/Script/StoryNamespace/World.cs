using Assets.Script.StoryNamespace.SceneNamespace;
using System;
using System.Collections.Generic;
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
        /// 添加场景
        /// </summary>
        /// <param name="scene"></param>
        public void AddScene(Scene scene)
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
            else
            {
                Scene scene = Scene.LoadScene(GameManager.GetCurrentStory().GetStoryPath() + "/Scene/" + sceneName + ".xml", sceneName);
                if (scene != null)
                {
                    AddScene(scene);
                }
                else
                {
                    GameManager.ShowErrorMessage("加载场景：" + sceneName + "失败！");
                }
                return scene;
            }
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
