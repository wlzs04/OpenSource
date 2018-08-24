using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Assets.Script.StoryNamespace.SceneNamespace
{
    /// <summary>
    /// 游戏主体
    /// </summary>
    class GameActor : ActorBase
    {
        static GameActor gameActor = null;

        private GameActor():base("Game")
        {

        }

        protected override ActorBase CreateActor(XElement node)
        {
            throw new NotImplementedException();
        }
    }
}
