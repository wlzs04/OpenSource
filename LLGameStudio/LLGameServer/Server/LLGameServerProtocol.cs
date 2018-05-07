using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLGameServer.Server
{
    class LLGameServerProtocol: LLGameProtocol
    {
        public LLGameServerProtocol(string name) :base(name)
        {
        }

        public void AddContent(string key, string value)
        {
            contentMap[key] = value;
        }
    }
}