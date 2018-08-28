using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Assets.Script.StoryNamespace.ActionNamespace;

namespace Assets.Script.StoryNamespace
{
    class Section
    {
        List<ActionBase> actionList = new List<ActionBase>();
        int index = 0;
        string sceneName;
        string description = "";
        int actionIndex = 0;

        public void LoadContent(XElement node)
        {
            foreach (var attribute in node.Attributes())
            {
                switch (attribute.Name.ToString())
                {
                    case "index":
                        index = Convert.ToInt32(attribute.Value);
                        break;
                    case "sceneName":
                        sceneName = attribute.Value;
                        break;
                    case "description":
                        description = attribute.Value;
                        break;
                    default:
                        break;
                }
            }

            foreach (var item in node.Elements())
            {
                ActionBase action = ActionBase.LoadAction(item);
                if (action != null)
                {
                    actionList.Add(action);
                }
                else
                {
                    GameManager.ShowErrorMessage("未知指令：" + item.Name.ToString());
                }
            }
        }

        /// <summary>
        /// 开始本节
        /// </summary>
        public void Start()
        {
            GameManager.GetCurrentStory().SetCurrentSceneByName(sceneName);
            ExecuteNextAction();
        }

        /// <summary>
        /// 执行下一条指令
        /// </summary>
        /// <returns></returns>
        public void ExecuteNextAction()
        {
            if(actionIndex>= actionList.Count)
            {
                GameManager.ShowDebugMessage("当前节的指令已经执行完！");
                GameManager.GetCurrentStory().Cut();
            }
            else
            {
                actionList[actionIndex].Execute();
                actionIndex++;
            }
        }
    }
}
