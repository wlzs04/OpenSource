using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLGameServer.Server
{
    abstract class LLGameProtocol
    {
        string name;
        string content;

        public LLGameProtocol(string name)
        {
            this.name = name;
        }

        public string GetName()
        {
            return name;
        }

        public void LoadContent(string content)
        {
            this.content = content;
        }

        public string GetContent()
        {
            return content;
        }

        public abstract void Process();
    }
}
