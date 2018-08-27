using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Script.StoryNamespace.SceneNamespace
{
    /// <summary>
    /// 摄像机
    /// </summary>
    class CameraActor : ActorBase
    {
        static CameraActor cameraActor = new CameraActor();

        GameObject gameObject = null;
        ActorBase actor = null;

        private CameraActor():base("Camera")
        {
            gameObject = GameObject.Find("Main Camera");
        }

        public static CameraActor GetInstance()
        {
            return cameraActor;
        }

        public override void Update()
        {
            if(actor != null)
            {
                gameObject.transform.localPosition = new Vector3(actor.GetPosition().x, actor.GetPosition().y, gameObject.transform.localPosition.z);
            }
        }

        protected override ActorBase CreateActor(XElement node)
        {
            GameManager.ShowErrorMessage("CameraActor演员无法被创建！");
            return null;
        }

        /// <summary>
        /// 设置跟随的演员
        /// </summary>
        /// <param name="actorName"></param>
        public void SetFollowActor(string actorName)
        {
            if(actorName=="")
            {
                actor = null;
            }
            else
            {
                actor = GameManager.GetCurrentStory().GetWorld().GetActor(actorName);
            }
        }

        /// <summary>
        /// 获得跟随的演员
        /// </summary>
        /// <returns></returns>
        public ActorBase GetFollowActor()
        {
            return actor;
        }
    }
}
