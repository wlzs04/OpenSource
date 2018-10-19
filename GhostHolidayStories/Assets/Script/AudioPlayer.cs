using Assets.Script;
using Assets.Script.Helper;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public class AudioPlayer : MonoBehaviour {

    GameManager gameManager = null;

    AudioSource audioSource = null;

    Dictionary<string, AudioClip> audioMap = new Dictionary<string, AudioClip>();

    void Start ()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = GameManager.GetInstance();
    }
	
	void Update ()
    {
        gameManager.Update();
    }

    /// <summary>
    /// 播放音乐
    /// </summary>
    /// <param name="audioPath"></param>
    public void PlayAudio(string audioPath)
    {
        if(!audioMap.ContainsKey(audioPath))
        {
            StartCoroutine(AddAudioClip(audioPath,true));
            return;
        }
        audioSource.clip = audioMap[audioPath];
        audioSource.Play();
    }

    /// <summary>
    /// 添加音乐并播放：可能出现先加载的声音后播放的问题
    /// </summary>
    /// <param name="audioPath"></param>
    /// <returns></returns>
    IEnumerator AddAudioClip(string audioPath,bool playIt)
    {
        WWW www = new WWW(audioPath);
        while (!www.isDone)
        {
            yield return null;
        }
        AudioClip audio = www.GetAudioClip(false, false, AudioType.OGGVORBIS);
        audioMap[audioPath] = audio;
        if(playIt)
        {
            audioSource.clip = audioMap[audioPath];
            audioSource.Play();
        }
        yield return audio;
    }

    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public void StopBackAudio()
    {
        audioSource.Stop();
    }

    /// <summary>
    /// 设置音量大小
    /// </summary>
    public void SetVolume(float value)
    {
        audioSource.volume = value;
    }
}
