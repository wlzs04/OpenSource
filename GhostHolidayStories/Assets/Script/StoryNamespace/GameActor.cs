using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.StoryNamespace
{
    /// <summary>
    /// 游戏主体
    /// </summary>
    class GameActor : Actor
    {
        static GameActor gameActor = null;
        private GameActor():base("Game")
        {

        }
    }
}
