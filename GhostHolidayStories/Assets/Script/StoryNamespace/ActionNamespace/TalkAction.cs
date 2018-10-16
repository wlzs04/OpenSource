using Assets.Script.StoryNamespace.SceneNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.StoryNamespace.ActionNamespace
{
    /// <summary>
    /// 谈话指令
    /// </summary>
    class TalkAction:ActionBase
    {
        string content = "";//谈话内容
        int onlyTalkByTime = 0;//只有在规定次数的谈话才会显示
        string audio = "";//谈话的音频路径
        bool showContent = true;

        int talkTime = 0;

        public TalkAction():base("Talk")
        {
        }

        protected override void Execute(ActorBase executor)
        {
            talkTime++;
            if (onlyTalkByTime != 0 && talkTime != onlyTalkByTime)
            {
                Complete();
                return;
            }

            DirectorActor.UITalk(executor.GetName(), content, audio, Complete);
        }

        protected override ActionBase CreateAction(XElement node)
        {
            TalkAction action = new TalkAction();
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
                    case "content":
                        content = attribute.Value;
                        break;
                    case "onlyTalkByTime":
                        onlyTalkByTime = Convert.ToInt32(attribute.Value);
                        break;
                    case "audio":
                        audio = attribute.Value;
                        break;
                    case "showContent":
                        showContent = Convert.ToBoolean(attribute.Value);
                        break;
                    default:
                        break;
                }
            }
        }

        protected override void CompleteAction()
        {
            DirectorActor.UIHide();
        }
    }
}
