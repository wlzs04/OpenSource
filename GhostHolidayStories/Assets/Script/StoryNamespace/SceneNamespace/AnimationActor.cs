using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Assets.Script.StoryNamespace.SceneNamespace
{
    /// <summary>
    /// 动画演员：展示动态物品，如被风吹动的草等
    /// </summary>
    class AnimationActor : ActorBase
    {
        int row = 1;//行数
        int column = 1;//列数
        float playTime = 1;//播放时间

        public AnimationActor() : base("Animation")
        {

        }

        protected override ActorBase CreateActor(XElement node)
        {
            AnimationActor animationActor = new AnimationActor();

            return animationActor;
        }

        protected override void LoadContent(XElement node)
        {
            base.LoadContent(node);
            foreach (var attribute in node.Attributes())
            {
                switch (attribute.Name.ToString())
                {
                    case "row":
                        row = Convert.ToInt32(attribute.Value);
                        break;
                    case "column":
                        column = Convert.ToInt32(attribute.Value);
                        break;
                    case "playTime":
                        playTime = (float)Convert.ToDouble(attribute.Value);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
