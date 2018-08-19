using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameManager.GetInstance().ChooseStory("捕蛇");

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
