using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Script.StoryNamespace.SceneNamespace
{
    /// <summary>
    /// 摄像师
    /// </summary>
    class CameraActor : ActorBase
    {
        static CameraActor cameraActor = new CameraActor();
        
        ActorBase focusActor = null;//被拍摄的演员

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
            if(focusActor != null)
            {
                Vector3 focusActorPosition = focusActor.GetPosition();
                gameObject.transform.localPosition = new Vector3(focusActorPosition.x, focusActorPosition.y, gameObject.transform.localPosition.z);
            }
            base.Update();
        }

        protected override ActorBase CreateActor(XElement node)
        {
            GameManager.ShowErrorMessage("CameraActor演员无法被创建！");
            return null;
        }

        protected override void LoadContent(XElement node)
        {
            GameManager.ShowErrorMessage("CameraActor演员无法从文件中加载！");
        }

        /// <summary>
        /// 设置跟随的演员
        /// </summary>
        /// <param name="actorName"></param>
        public void SetFollowActor(string actorName)
        {
            if(actorName=="")
            {
                focusActor = null;
            }
            else
            {
                focusActor = GameManager.GetCurrentStory().GetWorld().GetActor(actorName);
            }
        }

        /// <summary>
        /// 设置跟随的演员
        /// </summary>
        /// <param name="focusActor"></param>
        public void SetFollowActor(ActorBase focusActor)
        {
            this.focusActor = focusActor;
        }

        /// <summary>
        /// 获得跟随的演员
        /// </summary>
        /// <returns></returns>
        public ActorBase GetFocusActor()
        {
            return focusActor;
        }

        /// <summary>
        /// 获得摄像机
        /// </summary>
        /// <returns></returns>
        public Camera GetCamera()
        {
            return gameObject.GetComponent<Camera>();
        }
    }
}
