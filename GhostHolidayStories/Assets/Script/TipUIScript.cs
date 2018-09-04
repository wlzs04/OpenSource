using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TipUIScript : MonoBehaviour, IPointerClickHandler
{
    Text contentText = null;
    Image clickImage = null;

    bool autoDestroy = false;
    float destroyDelayTime = 2;

    float showTime = 0;

    public TipUIScript()
    {
        
    }

    // Use this for initialization
    void Start () {
        
    }

    void Awake()
    {
        contentText = transform.Find("BackImage").Find("ContentText").GetComponent<Text>();
        clickImage = transform.Find("BackImage").Find("ClickImage").GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.J)|| Input.GetKeyDown(KeyCode.Space))
        {
            GameObject.DestroyImmediate(gameObject);
        }
		if(autoDestroy&&Time.time-showTime>= destroyDelayTime)
        {
            GameObject.DestroyImmediate(gameObject);
        }
	}

    /// <summary>
    /// 设置提示框内容
    /// </summary>
    /// <param name="content"></param>
    /// <param name="autoDestroy"></param>
    /// <param name="destroyDelayTime"></param>
    public void SetContent(string content,bool autoDestroy = false, float destroyDelayTime = 2)
    {
        contentText.text = content;
        this.autoDestroy = autoDestroy;
        this.destroyDelayTime = destroyDelayTime;
        showTime = Time.time;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject.Destroy(gameObject);
    }
}
