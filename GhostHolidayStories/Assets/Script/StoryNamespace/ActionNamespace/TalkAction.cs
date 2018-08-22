using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Assets.Script.StoryNamespace.ActionNamespace
{
    class TalkAction:ActionBase
    {
        string content = "";//谈话内容
        int onlyTalkByTime = 0;//只有在规定次数的谈话才会显示
        string audio = "";//谈话的音频路径

        public TalkAction():base("TalkAction")
        {

        }

        protected override ActionBase CreateAction(XElement node)
        {
            TalkAction talkAction = new TalkAction();
            talkAction.LoadContent(node);
            return talkAction;
        }

        protected override void LoadContent(XElement node)
        {
            base.LoadContent(node);
            foreach (var attribute in node.Attributes())
            {
                switch (attribute.Name.ToString())
                {
                    case "content":
                        content = attribute.Value;
                        break;
                    case "onlyTalkByTime":
                        onlyTalkByTime = Convert.ToInt32(attribute.Value);
                        break;
                    case "audio":
                        audio = attribute.Value;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
