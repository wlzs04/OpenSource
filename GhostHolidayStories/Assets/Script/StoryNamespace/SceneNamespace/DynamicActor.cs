using Assets.Script.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Script.StoryNamespace.SceneNamespace
{
    /// <summary>
    /// 动态演员：展示动态物品，如被风吹动的草等
    /// </summary>
    class DynamicActor : ActorBase
    {
        int row = 1;//行数
        int column = 1;//列数
        float onceTime = 1;//播放时间
        List<Sprite> imageList = new List<Sprite>();

        float lastTime = 0;
        float changeAfterTime = 0;
        int currentImageIndex = 0;

        public DynamicActor() : base("Dynamic")
        {

        }

        public override void Update()
        {
            base.Update();
            if(Time.time -lastTime> changeAfterTime)
            {
                currentImageIndex = currentImageIndex + 1 < imageList.Count ? currentImageIndex + 1 : 0;
                gameObject.GetComponent<SpriteRenderer>().sprite = imageList[currentImageIndex];
                lastTime = Time.time;
            }
        }

        protected override ActorBase CreateActor(XElement node)
        {
            DynamicActor animationActor = new DynamicActor();
            animationActor.LoadContent(node);
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
                    case "onceTime":
                        onceTime = (float)Convert.ToDouble(attribute.Value);
                        break;
                    default:
                        break;
                }
            }

            float everyWidth = texture.width/ column;
            float everyHeight = texture.height / row;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    Sprite sprite = Sprite.Create(texture, new Rect(j * everyWidth, i * everyHeight, everyWidth, everyHeight), Vector2.zero);
                    imageList.Add(sprite);
                }
            }
            changeAfterTime = onceTime / row / column;
            gameObject.GetComponent<SpriteRenderer>().sprite = imageList[currentImageIndex];
        }
    }
}
