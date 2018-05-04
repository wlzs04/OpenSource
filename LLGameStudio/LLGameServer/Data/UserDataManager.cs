using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLGameServer.Data
{
    class UserDataManager
    {
        int maxUserNumber = 2;
        Dictionary<string, UserData> userDataMap = new Dictionary<string, UserData>();
        static UserDataManager instance = new UserDataManager();

        private UserDataManager() { }

        public static UserDataManager GetInstance()
        {
            return instance;
        }

        public UserData GetUserData(string key)
        {
            return userDataMap[key];
        }

        public bool AddUserData(string key, UserData value)
        {
            if(userDataMap.Count>=maxUserNumber)
            {
                return false;
            }
            if (userDataMap.ContainsKey(key))
            {
                return false;
            }
            userDataMap[key] = value;
            return true;
        }

        public void SetMaxUserNumber(int maxUserNumber)
        {
            this.maxUserNumber = maxUserNumber;
        }

        public int GetUserCount()
        {
            return userDataMap.Count;
        }
    }
}
