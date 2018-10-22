using Assets.Script;
using Assets.Script.StoryNamespace.SceneNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIScript : MonoBehaviour {

    ScrollRect objectItemScrollView;
    public GameObject objectItemPrefab;

    Text moneyText;

    void Awake()
    {
        objectItemScrollView = transform.Find("BackImage/ObjectItemScrollView").GetComponent<ScrollRect>();
        moneyText = transform.Find("BackImage/MoneyText").GetComponent<Text>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            DirectorActor.UIReturnFromMenu();
        }
    }

    /// <summary>
    /// 刷新物品列表
    /// </summary>
    public void RefreshObjectList()
    {
        for (int i = objectItemScrollView.content.childCount - 1; i >= 0; i--)
        {
            GameObject.DestroyImmediate(objectItemScrollView.content.GetChild(i).gameObject);
        }

        foreach (var item in DirectorActor.GetInstance().GetObjectItemMap())
        {
            if(item.Key!=10001)
            {

                GameObject objectItem = GameObject.Instantiate(objectItemPrefab, objectItemScrollView.content);
                objectItem.GetComponent<ObjectItemScript>().SetInfo(item.Key, item.Value);
            }
        }

        moneyText.text = "金钱：" + DirectorActor.GetInstance().GetMoney();
    }
    
    /// <summary>
    /// 从菜单界面返回到游戏场景界面
    /// </summary>
    public void ReturnButtonClick()
    {
        DirectorActor.UIReturnFromMenu();
    }

    /// <summary>
    /// 保存按钮点击事件
    /// </summary>
    public void SaveButtonClick()
    {
        DirectorActor.SaveGameData();
    }

    /// <summary>
    /// 从游戏场景界面退出到游戏进入界面
    /// </summary>
    public void ExitButtonClick()
    {
        DirectorActor.ExitStory();
    }
}
