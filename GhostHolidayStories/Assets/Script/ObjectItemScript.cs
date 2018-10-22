using Assets.Script.StoryNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectItemScript : MonoBehaviour {

    Text nameText;
    Text countText;

    void Awake()
    {
        nameText = transform.Find("NameText").GetComponent<Text>();
        countText = transform.Find("CountText").GetComponent<Text>();
    }

    /// <summary>
    /// 设置信息
    /// </summary>
    /// <param name="name"></param>
    /// <param name="count"></param>
    public void SetInfo(int id,int count)
    {
        nameText.text = ObjectItem.GetNameById(id);
        countText.text = count+"";
    }
}
