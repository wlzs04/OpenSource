using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.StoryNamespace
{
    /// <summary>
    /// 摄像机
    /// </summary>
    class CameraActor : Actor
    {
        static CameraActor cameraActor = null;
        private CameraActor():base("Camera")
        {
        }
    }
}
