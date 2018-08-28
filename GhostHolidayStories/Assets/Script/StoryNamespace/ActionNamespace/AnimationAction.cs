using Assets.Script.Helper;
using Assets.Script.StoryNamespace.SceneNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Script.StoryNamespace.ActionNamespace
{
    /// <summary>
    /// 动画指令
    /// </summary>
    class AnimationAction : ActionBase
    {
        int loopTime = 1;//动画循环次数，-1代表永远执行
        float onceTime = 1;//动画播放一次所用的时间
        int row = 1;//行数
        int column = 1;//列数
        string imagePath = "";//动画图片名称

        ActorBase actor = null;
        Sprite oldImage = null;
        float lastTime = 0;
        float changeAfterTime = 0;
        int currentImageIndex = 0;
        int haveLoopTime = 0;
        List<Sprite> imageList = new List<Sprite>();

        public AnimationAction():base("Animation")
        {

        }

        public override void Update()
        {
            base.Update();
            if (Time.time - lastTime > changeAfterTime)
            {
                currentImageIndex = currentImageIndex + 1 < imageList.Count ? currentImageIndex + 1 : 0;
                if (currentImageIndex == 0)
                {
                    haveLoopTime++;
                    if (haveLoopTime >= loopTime)
                    {
                        actor.GetGameObject().GetComponent<SpriteRenderer>().sprite = oldImage;
                        actionCompleteCallBack.Invoke();
                    }
                }
                actor.GetGameObject().GetComponent<SpriteRenderer>().sprite = imageList[currentImageIndex];
                lastTime = Time.time;
            }
        }

        public override void Execute()
        {
            GameManager.GetCurrentStory().AddAction(this);
            actor = GameManager.GetCurrentStory().GetWorld().GetActor(actorName);

            Texture2D texture = ImageHelper.LoadTexture(GameManager.GetCurrentStory().GetStoryPath() + "/Texture/" + imagePath);

            float everyWidth = texture.width / column;
            float everyHeight = texture.height / row;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    Sprite sprite = Sprite.Create(texture, new Rect(j * everyWidth, i * everyHeight, everyWidth, everyHeight), Vector2.zero);
                    imageList.Add(sprite);
                }
            }

            lastTime = Time.time;
            changeAfterTime = onceTime / row / column;
            oldImage = actor.GetGameObject().GetComponent<SpriteRenderer>().sprite;
            actor.GetGameObject().GetComponent<SpriteRenderer>().sprite = imageList[currentImageIndex];

            if(isAsync)
            {
                //GameManager.GetCurrentStory().GetCurrentSection().ExecuteNextAction();
            }
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
                    case "image":
                        imagePath = attribute.Value;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
