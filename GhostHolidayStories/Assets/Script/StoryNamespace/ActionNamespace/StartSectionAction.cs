using Assets.Script.StoryNamespace.SceneNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Assets.Script.StoryNamespace.ActionNamespace
{
    /// <summary>
    /// 章节指令：开始播放指定章节
    /// </summary>
    class StartSectionAction : ActionBase
    {
        int chapterIndex = 0;//章
        int sectionIndex = 0;//节

        public StartSectionAction() : base("StartSection")
        {

        }

        protected override ActionBase CreateAction(XElement node)
        {
            StartSectionAction action = new StartSectionAction();
            action.LoadContent(node);
            return action;
        }

        public override ActorBase GetExecutor()
        {
            return DirectorActor.GetInstance();
        }

        protected override void Execute(ActorBase executor)
        {
            GameManager.GetCurrentStory().StartNewChapterSection(chapterIndex, sectionIndex);            
            Complete();
        }

        protected override void LoadContent(XElement node)
        {
            base.LoadContent(node);
            foreach (var attribute in node.Attributes())
            {
                switch (attribute.Name.ToString())
                {
                    case "chapterIndex":
                        chapterIndex = Convert.ToInt32(attribute.Value);
                        break;
                    case "sectionIndex":
                        sectionIndex = Convert.ToInt32(attribute.Value);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
