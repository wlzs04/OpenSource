using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {

    GameManager gameManager = null;

	// Use this for initialization
	void Start ()
    {
        gameManager = GameManager.GetInstance();
	}
	
	// Update is called once per frame
	void Update ()
    {
        gameManager.Update();
    }
}
