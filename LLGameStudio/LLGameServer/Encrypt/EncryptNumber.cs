using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLGameServer.Encrypt
{
    /// <summary>
    /// 自定义加密算法类
    /// 类型：对称加密
    /// 秘钥格式：第一个字符为原文分组长度（范围1~9），
    /// 根据长度获得字符分组后排列顺序（是0开始连续指定长度后打乱顺序），
    /// 余下数字代表对ASCII码移动位数，
    /// 默认值：401230。
    /// </summary>
    class EncryptNumber :IEncryptClass
    {
        int teamLength = 4;
        int byteLength = 255;
        string key = "01230";

        public byte[] Encrypt(byte[] content)
        {
            List<byte> byteSource = new List<byte>();
            int[] keyInt = new int[teamLength];
            int keyLength = key.Length;
            int moveLength = keyLength - teamLength;
            int contentLength = content.Length;

            for (int i = 0; i < teamLength; i++)
            {
                keyInt[i] = int.Parse(key[i].ToString());
            }

            int moveInt = int.Parse(key.Substring(teamLength));
            int tempi = 0;
            byte tempb = 0;
            int tempBase = 0;
            int teamCount = (int)Math.Floor((double)contentLength/ teamLength);
            for (int i = 0; i < teamCount; i++)
            {
                for (int j = 0; j < teamLength; j++)
                {
                    tempb = content[tempBase + keyInt[j]];
                    tempi = (tempb + moveInt) % byteLength;
                    byteSource.Add((byte)tempi);
                }
                tempBase += teamLength;
            }
            if(tempBase< contentLength)
            {
                for (int i = 0; i < teamLength; i++)
                {
                    if(tempBase + keyInt[i]< contentLength)
                    {
                        tempb = content[tempBase + keyInt[i]];
                        tempi = (tempb + moveInt) % byteLength;
                        byteSource.Add((byte)tempi);
                    }
                }
            }
            return byteSource.ToArray();
        }

        public byte[] Decode(byte[] content)
        {
            int[] keyInt = new int[teamLength];
            int keyLength = key.Length;
            int moveLength = keyLength - teamLength;
            int contentLength = content.Length;
            for (int i = 0; i < teamLength; i++)
            {
                keyInt[i] = int.Parse(key[i].ToString());
            }
            int moveInt = byteLength - int.Parse(key.Substring(teamLength));
            byte[] byteSource = new byte[content.Length];
            int tempi = 0;
            byte tempb = 0;
            int tempBase = 0;
            int teamCount = (int)Math.Floor((double)contentLength / teamLength);
            for (int i = 0; i < teamCount; i ++)
            {
                for (int j = 0; j < teamLength; j++)
                {
                    tempb = content[tempBase + j];
                    tempi = (tempb + moveInt) % byteLength;
                    byteSource[tempBase + keyInt[j]]= (byte)tempi;
                }
                tempBase += teamLength;
            }
            if (tempBase < contentLength)
            {
                int re = contentLength - tempBase;
                int reIndex = 0;
                for (int i = 0; i < teamLength; i++)
                {
                    if( keyInt[i]< re)
                    {
                        tempb = content[tempBase+ reIndex];
                        tempi = (tempb + moveInt) % byteLength;
                        byteSource[tempBase + keyInt[i]] = (byte)tempi;
                        reIndex++;
                    }
                }
            }
            return byteSource;
        }

        public void SetKey(string key)
        {
            teamLength = int.Parse(key.Substring(0,1));
            this.key = key.Substring(1);
        }

        /// <summary>
        /// 随机获得一个合法的秘钥
        /// </summary>
        /// <returns></returns>
        public string GetRandomLegalKey()
        {
            Random random = new Random();
            int teamLength = random.Next(9)+1;
            int[] keyInt = new int[teamLength];
            for (int i = 0; i < teamLength; i++)
            {
                keyInt[i] = i;
            }

            for (int i = 0; i < 10; i++)
            {
                int left = random.Next(teamLength);
                int right = random.Next(teamLength);
                int tempInt = keyInt[left];
                keyInt[left] = keyInt[right];
                keyInt[right] = tempInt;
            }

            string key = "";
            for (int i = 0; i < teamLength; i++)
            {
                key += keyInt[i];
            }

            int moveInt = random.Next(256);
            return teamLength+ key + moveInt;
        }
    }
}
