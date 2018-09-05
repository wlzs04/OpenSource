using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TipUIScript : MonoBehaviour, IPointerClickHandler
{
    Text contentText = null;
    Image clickImage = null;

    bool autoDestroy = false;//是否主动销毁
    float showTime = 2;//显示时间
    float startShowTime = 0;//开始显示的时间

    void Start () {
        
    }

    void Awake()
    {
        contentText = transform.Find("BackImage").Find("ContentText").GetComponent<Text>();
        clickImage = transform.Find("BackImage").Find("ClickImage").GetComponent<Image>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Space))
        {
            GameObject.DestroyImmediate(gameObject);
        }
        if (autoDestroy && Time.time - startShowTime >= showTime)
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
    public void SetContent(string content,bool autoDestroy = false, float showTime = 2)
    {
        contentText.text = content;
        this.autoDestroy = autoDestroy;
        this.showTime = showTime;
        showTime = Time.time;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject.Destroy(gameObject);
    }
}
