using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Assets.Script.StoryNamespace.SceneNamespace;

namespace Assets.Script.StoryNamespace.ActionNamespace
{
    enum TimerType
    {
        Countdown,//倒计时
    } 

    /// <summary>
    /// 计时器指令：用于倒计时的效果
    /// </summary>
    class TimerAction : ActionBase
    {
        TimerType timerType;
        bool canPass = true;
        float remainTime = 0;
        PassAction passAction;

        public TimerAction() : base("Timer")
        {

        }

        protected override ActionBase CreateAction(XElement node)
        {
            TimerAction action = new TimerAction();
            action.LoadContent(node);
            return action;
        }

        protected override void Execute(ActorBase executor)
        {
            DirectorActor.GetInstance().SetTimerAction(this);
            Complete();
        }

        public PassAction GetPassAction()
        {
            return passAction;
        }

        public override ActorBase GetExecutor()
        {
            return DirectorActor.GetInstance();
        }

        protected override void LoadContent(XElement node)
        {
            base.LoadContent(node);
            foreach (var attribute in node.Attributes())
            {
                switch (attribute.Name.ToString())
                {
                    case "timerType":
                        timerType = (TimerType)Enum.Parse(typeof(TimerType), attribute.Value);
                        break;
                    case "canPass":
                        canPass = Convert.ToBoolean(attribute.Value);
                        break;
                    case "remainTime":
                        remainTime = (float)Convert.ToDouble(attribute.Value);
                        break;
                    default:
                        break;
                }
            }
            foreach (var item in node.Elements())
            {
                switch (item.Name.ToString())
                {
                    case "Pass":
                        if (passAction != null)
                        {
                            GameManager.ShowErrorMessage("Timer中出现多个计时结束指令节点。");
                        }
                        passAction = (PassAction)ActionBase.LoadAction(item);
                        break;
                    default:
                        GameManager.ShowErrorMessage("Timer中出现未知子节点：" + item.Name.ToString());
                        break;
                }
            }
        }

        public float GetRemainTime()
        {
            return remainTime;
        }

        public bool GetCanPass()
        {
            return canPass;
        }
    }
}
