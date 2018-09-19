using Assets.Script;
using Assets.Script.Helper;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public class AudioPlayer : MonoBehaviour {

    GameManager gameManager = null;

    //AudioClip backAudio = null;
    AudioSource audioSource = null;

    Dictionary<string, AudioClip> audioMap = new Dictionary<string, AudioClip>();

    // Use this for initialization
    void Start ()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = GameManager.GetInstance();
    }
	
	// Update is called once per frame
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
            StartCoroutine(AddAudioClip(audioPath));
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
    IEnumerator AddAudioClip(string audioPath)
    {
        WWW www = new WWW(audioPath);
        while (!www.isDone)
        {
            yield return null;
        }
        AudioClip audio = www.GetAudioClip(false, false, AudioType.OGGVORBIS);
        audioMap[audioPath] = audio;
        audioSource.clip = audioMap[audioPath];
        audioSource.Play();
        yield return audio;
    }

}
