using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class ScoreCountDot : MonoBehaviour {

	public Image[] DotNumber;//配列は7つうう　0から1増えるごとに桁が増えるううう

	public Sprite[] DotTEXT;

	int ScoreGet;
	int[] setNum = new int[7];


	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (ScoreGet != MainGameSp.score) {
			ScoreGet = MainGameSp.score;
			for (int i = 6; i >= 0; i--){
				setNum[i] = ScoreGet /SQUARE(i+1);
				DotNumber [i].sprite = DotTEXT [setNum [i]];
				ScoreGet = ScoreGet - SQUARE(i+1) * setNum [i];
			}
			ScoreGet = MainGameSp.score;
		}
	}

	int SQUARE (int i){
		int b = 1;
		for (int s = 0; s < i-1; s++) {
			b = b * 10;
		}
		return b;
	}
}
