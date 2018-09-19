using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Script.Helper
{
    class AudioHelper
    {
        /// <summary>
        /// 加载音频文件
        /// </summary>
        /// <returns></returns>
        //public static AudioClip LoadAudio(string audioPath)
        //{
        //    //创建文件读取流
        //    FileStream fileStream = new FileStream(audioPath, FileMode.Open, FileAccess.Read);
        //    fileStream.Seek(0, SeekOrigin.Begin);
        //    //创建文件长度缓冲区
        //    byte[] bytes = new byte[fileStream.Length];
        //    //读取文件
        //    fileStream.Read(bytes, 0, (int)fileStream.Length);
        //    //释放文件读取流
        //    fileStream.Close();
        //    fileStream.Dispose();
        //    fileStream = null;

        //    AudioClip audio = new AudioClip();
        //    audio.SetData(bytes, 0);
        //    return audio;
        //}
    }
}
