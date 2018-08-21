using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.StoryNamespace
{
    class Actor
    {
        string name;

        public Actor(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="action"></param>
        public void Action(Action action)
        {

        }

        /// <summary>
        /// 设置位置
        /// </summary>
        /// <param name="newPosition"></param>
        private void SetPosition(Vector2 newPosition)
        {

        }

        /// <summary>
        /// 角色移动到指定位置
        /// </summary>
        /// <param name="newPosition"></param>
        private void MoveToPosition(Vector2 newPosition,float needTime,MoveState moveState)
        {

        }
    }
}
