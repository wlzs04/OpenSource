using Assets.Script.StoryNamespace.ActionNamespace;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionUIScript : MonoBehaviour {

    Transform back = null;
    GameObject optionPrefab = null;

    int currentFocusIndex = 0;//当前选择的选项位置

    private void Awake()
    {
        optionPrefab = Resources.Load<GameObject>("UI/OptionItemPrefab");
        back = transform.Find("BackImage").transform;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Space)|| Input.GetKeyDown(KeyCode.J))
        {
            back.GetChild(currentFocusIndex).GetComponent<OptionItemUIScript>().Click();
        }
        if (Input.GetKeyDown(KeyCode.W)|| Input.GetKeyDown(KeyCode.UpArrow)) 
        {
            SetFocusIndex(currentFocusIndex - 1);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            SetFocusIndex(currentFocusIndex + 1);
        }
    }

    /// <summary>
    /// 添加选项
    /// </summary>
    public void AddOption(string content,Action callBack,bool showSelectedState=false)
    {
        GameObject.Instantiate(optionPrefab, back).GetComponent<OptionItemUIScript>().SetContent(this,back.childCount-1, content,callBack,showSelectedState);
    }

    /// <summary>
    /// 清空所有选项
    /// </summary>
    public void ClearAllOption()
    {
        for (int i = back.childCount-1; i >=0 ; i--)
        {
            GameObject.DestroyImmediate(back.GetChild(i).gameObject);
        }
    }

    public void SetFocusIndex(int index)
    {
        back.GetChild(currentFocusIndex).GetComponent<OptionItemUIScript>().UnfocusOption();
        currentFocusIndex = Mathf.Clamp(index,0,back.childCount-1);
        back.GetChild(currentFocusIndex).GetComponent<OptionItemUIScript>().FocusOption();
    }
}
