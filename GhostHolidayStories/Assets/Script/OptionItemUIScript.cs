using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionItemUIScript : MonoBehaviour, IPointerClickHandler
{
    Text optionText = null;
    Action callBack = null;

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

    public void SetContent(string content, Action callBack)
    {
        optionText.text = content;
        this.callBack = callBack;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(callBack!=null)
        {
            callBack();
        }
    }
}
