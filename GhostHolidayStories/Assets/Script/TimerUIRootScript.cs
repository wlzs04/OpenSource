using Assets.Script;
using Assets.Script.StoryNamespace.ActionNamespace;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUIRootScript : MonoBehaviour {

    Text remainTimeText;
    Button passButton;
    float remainTime = 0;
    bool canPass = true;
    ActionCompleteCallBack completeAction = null;
    bool haveFinished = false;
    float startTime = 0;

    private void Awake()
    {
        remainTimeText = transform.Find("RemainTimeText").GetComponent<Text>();
        passButton = transform.Find("PassButton").GetComponent<Button>();
        passButton.onClick.AddListener(()=> { Pass(); });
    }

	
	void Update ()
    {
		if(!haveFinished)
        {
            float haveFinishedTime = Time.time - startTime;
            if(remainTime - haveFinishedTime <0)
            {
                Pass();
            }
            else
            {
                SetRemainText(remainTime - haveFinishedTime);
            }
        }
	}

    /// <summary>
    /// 自动跳过或倒计时结束
    /// </summary>
    void Pass()
    {
        haveFinished = true;
        if (completeAction!=null)
        {
            completeAction();
        }
        else
        {
            GameManager.ShowErrorMessage("当前计时器没有完成事件。");
        }
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 设置消息
    /// </summary>
    public void SetInfo(float remainTime,bool canPass, ActionCompleteCallBack completeAction)
    {
        this.remainTime = remainTime;
        SetRemainText(remainTime);
        this.canPass = canPass;
        passButton.gameObject.SetActive(canPass);
        this.completeAction = completeAction;
        haveFinished = false;
        startTime = Time.time;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 设置剩余时间的文字
    /// </summary>
    void SetRemainText(float remainTime)
    {
        remainTimeText.text = "剩余时间：" + Math.Floor(remainTime);
    }
}
