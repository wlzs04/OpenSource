using Assets.Script;
using Assets.Script.Helper;
using Assets.Script.StoryNamespace.ActionNamespace;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TalkUIScript : MonoBehaviour, IPointerClickHandler
{
    Text actorNameText = null;
    Text contentText = null;
    AudioSource talkAudio = null;

    ActionCompleteCallBack completeAction = null;

    public TalkUIScript()
    {
    }

    void Awake()
    {
        actorNameText = transform.Find("BackImage").Find("ActorNameText").GetComponent<Text>();
        contentText = transform.Find("BackImage").Find("ContentText").GetComponent<Text>();
        talkAudio = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update () {
		if(Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Space))
        {
            if (completeAction != null)
            {
                completeAction();
            }
            //GameManager.GetInstance().SetUI(UIState.Clean);
        }
	}

    /// <summary>
    /// 初始化TalkUI，清空角色名称等信息
    /// </summary>
    public void Init()
    {
        actorNameText.text = "";
        contentText.text = "";
        talkAudio.Stop();
        talkAudio.clip = null;
        completeAction = null;
    }

    /// <summary>
    /// 设置演员名称
    /// </summary>
    /// <param name="actorName"></param>
    public void SetActorName(string actorName)
    {
        actorNameText.text = actorName;
    }

    /// <summary>
    /// 设置谈话内容
    /// </summary>
    /// <param name="content"></param>
    public void SetContent(string content)
    {
        contentText.text = content;
    }

    /// <summary>
    /// 设置谈话声音
    /// </summary>
    /// <param name="audioName"></param>
    public void SetAudio(string audioName)
    {
        if(audioName!="")
        {
            //talkAudio.clip = AudioHelper.PlayAudio(GameManager.GetCurrentStory().GetStoryPath() + "/Audio/" + audioName);
            talkAudio.volume = 1;
            talkAudio.Play();
        }
        else
        {
            talkAudio.Stop();
            talkAudio.clip = null;
        }
    }

    public void SetCompleteCallBack(ActionCompleteCallBack callBack)
    {
        completeAction = callBack;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(completeAction!=null)
        {
            completeAction();
        }
    }
}
