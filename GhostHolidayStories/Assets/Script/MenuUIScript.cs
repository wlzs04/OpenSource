using Assets.Script;
using Assets.Script.StoryNamespace.SceneNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIScript : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        
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
