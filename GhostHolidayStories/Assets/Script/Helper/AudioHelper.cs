using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Helper
{
    class AudioHelper
    {
        /// <summary>
        /// 加载音频文件
        /// </summary>
        /// <returns></returns>
        public static AudioClip LoadAudio(string audioPath)
        {
            WWW www = new WWW(audioPath);
            if (!string.IsNullOrEmpty(www.error))
            {
                GameManager.ShowErrorMessage("加载音频文件：" + audioPath + "失败！");
            }
            return www.GetAudioClip(true, true, AudioType.MPEG);
        }
    }
}
