using Assets.Script.StoryNamespace.SceneNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Helper
{
    /// <summary>
    /// 游戏帮助类
    /// </summary>
    class GameHelper
    {
        /// <summary>
        /// 检测角色是否在范围内
        /// </summary>
        /// <returns></returns>
        public static bool CheckActorInArea(ActorBase actor, Vector2 position ,float radius)
        {
            return (position - actor.GetWorldPosition()).magnitude<= radius;
        }
    }
}
