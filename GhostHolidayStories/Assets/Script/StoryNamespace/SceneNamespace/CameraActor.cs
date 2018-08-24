using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Assets.Script.StoryNamespace.SceneNamespace
{
    /// <summary>
    /// 摄像机
    /// </summary>
    class CameraActor : ActorBase
    {
        static CameraActor cameraActor = null;

        private CameraActor():base("Camera")
        {
        }

        protected override ActorBase CreateActor(XElement node)
        {
            throw new NotImplementedException();
        }
    }
}
