using Assets.Script.StoryNamespace.ActionNamespace;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionItemUIScript : MonoBehaviour, IPointerClickHandler,IPointerEnterHandler, IPointerExitHandler
{
    QuestionUIScript questionUIScript=null;
    Text optionText = null;
    Action callBack = null;
    bool showSelectedState = false;//是否显示已经被选过
    int index = 0;

    // Use this for initialization
    void Start () {
		
	}

    private void Awake()
    {
        optionText = transform.Find("OptionText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void SetContent(QuestionUIScript questionUIScript, int index,string content, Action callBack,bool showSelectedState=false)
    {
        this.questionUIScript = questionUIScript;
        this.index = index;
        optionText.text = content;
        this.callBack = callBack;
        if(showSelectedState)
        {
            optionText.color = Color.gray;
        }
        else
        {
            optionText.color = Color.black;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Click();
    }

    /// <summary>
    /// 用于主动触发点击事件
    /// </summary>
    public void Click()
    {
        if (callBack != null)
        {
            callBack();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        questionUIScript.SetFocusIndex(index);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

    public void FocusOption()
    {
        transform.localScale = new Vector3(1.05f, 1.05f, 1);
    }

    public void UnfocusOption()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }
}
