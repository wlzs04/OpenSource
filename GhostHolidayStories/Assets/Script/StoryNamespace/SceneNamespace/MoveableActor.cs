using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Script.StoryNamespace.SceneNamespace
{
    /// <summary>
    /// 可移动的演员：用来可以接受移动等动态指令
    /// </summary>
    class MoveableActor : ActorBase
    {
        protected Sprite image = null;
        protected float Speed = 1000;

        public MoveableActor() : base("Moveable")
        {

        }


        protected override ActorBase CreateActor(XElement node)
        {
            MoveableActor actor = new MoveableActor();
            actor.LoadContent(node);
            return actor;
        }

        protected override void LoadContent(XElement node)
        {
            base.LoadContent(node);
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            image = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            gameObject.GetComponent<SpriteRenderer>().sprite = image;
        }

        public float GetSpeed()
        {
            return Speed;
        }
    }
}
