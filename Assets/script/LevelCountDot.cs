using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class LevelCountDot : MonoBehaviour {
	

	public Image[] DotNumber;//配列は3つ　0が１の位 1が１０の位 2が１００の位

	public Sprite[] DotTEXT;

	int LevelGet;
	int set001,set010,set100;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (LevelGet != MainGameSp.DelBoxLevel) {
			set100 = MainGameSp.DelBoxLevel / 100;
			set010 = (MainGameSp.DelBoxLevel - set100*100) / 10;
			set001 = (MainGameSp.DelBoxLevel - set100*100 - set010*10) % 10;
			DotNumber [0].sprite = DotTEXT [set001];
			DotNumber [1].sprite = DotTEXT [set010];
			DotNumber [2].sprite = DotTEXT [set100];
			LevelGet = MainGameSp.DelBoxLevel;
		}
	}
}
