using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class ScoreCountDot : MonoBehaviour {


	int ScoreGet;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		if (ScoreGet != MainGameSp.score) {
			ScoreGet = MainGameSp.score;
			this.GetComponent<Text> ().text =""+ MainGameSp.score;
		}
	}

}
