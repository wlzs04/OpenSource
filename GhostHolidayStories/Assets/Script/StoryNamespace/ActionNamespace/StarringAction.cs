using Assets.Script.StoryNamespace.SceneNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Assets.Script.StoryNamespace.ActionNamespace
{
    /// <summary>
    /// 主演指令：设置指定演员为主演，主演可以被玩家控制
    /// </summary>
    class StarringAction : ActionBase
    {
        public StarringAction():base("Starring")
        {
        }

        public override void Execute(ActorBase executor)
        {
            ActorBase actor = GameManager.GetCurrentStory().GetWorld().GetActor(actorName);
            DirectorActor.GetInstance().SetStarringActor(actor);
            Complete();
        }

        protected override ActionBase CreateAction(XElement node)
        {
            StarringAction action = new StarringAction();
            action.LoadContent(node);
            return action;
        }

        public override ActorBase GetExecutor()
        {
            return CameraActor.GetInstance();
        }
    }
}
