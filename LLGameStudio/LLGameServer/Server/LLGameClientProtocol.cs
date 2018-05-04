using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLGameServer.Server
{
    abstract class LLGameClientProtocol : LLGameProtocol
    {
        public LLGameClientProtocol(string name):base(name)
        {

        }

        public abstract LLGameClientProtocol GetInstance();

        public string GetContent(string key)
        {
            return contentMap[key];
        }

        public abstract void Process();
    }
}
