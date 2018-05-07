using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLGameServer.Encrypt
{
    /// <summary>
    /// 加密算法接口类
    /// </summary>
    interface IEncryptClass
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        byte[] Encrypt(byte[] content);
        
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        byte[] Decode(byte[] content);
    }
}
