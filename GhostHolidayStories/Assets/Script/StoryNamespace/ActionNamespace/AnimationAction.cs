using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Assets.Script.StoryNamespace.ActionNamespace
{
    /// <summary>
    /// 动画指令
    /// </summary>
    class AnimationAction : ActionBase
    {
        bool isAsync = false;//是否此指令执行的同时执行下一条指令
        int loopTime = 1;//动画循环次数，-1代表永远执行
        float onceTime = 1;//动画播放一次所用的时间
        int row = 1;//行数
        int column = 1;//列数
        string animationName = "";//动画名称

        public AnimationAction():base("Animation")
        {

        }

        protected override ActionBase CreateAction(XElement node)
        {
            AnimationAction action = new AnimationAction();
            action.LoadContent(node);
            return action;
        }

        protected override void LoadContent(XElement node)
        {

            base.LoadContent(node);
            foreach (var attribute in node.Attributes())
            {
                switch (attribute.Name.ToString())
                {
                    case "isAsync":
                        isAsync = Convert.ToBoolean(attribute.Value);
                        break;
                    case "loopTime":
                        loopTime = Convert.ToInt32(attribute.Value);
                        break;
                    case "onceTime":
                        onceTime = (float)Convert.ToDouble(attribute.Value);
                        break;
                    case "row":
                        row = Convert.ToInt32(attribute.Value);
                        break;
                    case "column":
                        column = Convert.ToInt32(attribute.Value);
                        break;
                    case "animationName":
                        animationName = attribute.Value;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
