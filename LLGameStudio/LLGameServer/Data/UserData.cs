using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLGameServer.Data
{
    class UserData
    {
        Dictionary<string, string> userDataMap = new Dictionary<string, string>();

        public string GetContent(string key)
        {
            return userDataMap[key];
        }

        public void SetContent(string key,string value)
        {
            userDataMap[key] = value;
        }
    }
}
