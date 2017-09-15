using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class NumberDot : MonoBehaviour {
	public Image[] DotNumber;//配列は2つ　0が１の位 1が１０の位

	public Sprite[] DotTEXT;

	int ComboGet;
	int set01,set10;
	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {
		if (MainGameSp.ActiveCombo > 0) {
			if (ComboGet != MainGameSp.ActiveCombo) {
				set10 = MainGameSp.ActiveCombo / 10;
				set01 = MainGameSp.ActiveCombo % 10;
//				Debug.Log (set10+" "+set01);
				DotNumber [0].sprite = DotTEXT [set01];
				DotNumber [1].sprite = DotTEXT [set10];
				ComboGet = MainGameSp.ActiveCombo;
			}
		} else {
			DotNumber [0].sprite = DotTEXT [0];
			DotNumber [1].sprite = DotTEXT [0];
		}
	}
}
