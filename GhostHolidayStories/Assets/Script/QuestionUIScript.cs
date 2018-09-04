using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionUIScript : MonoBehaviour {

    Transform back = null;
    GameObject optionPrefab = null;

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

    }

    /// <summary>
    /// 添加选项
    /// </summary>
    public void AddOption(string content,Action callBack)
    {
        GameObject.Instantiate(optionPrefab, back).GetComponent<OptionItemUIScript>().SetContent(content,callBack);
    }
}
