using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleHyotan : MonoBehaviour {
	float backTimer = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		backTimer += Time.deltaTime;
		if (backTimer <= 0.5) {
			transform.localScale = new Vector3 (transform.localScale.x + Time.deltaTime, transform.localScale.y + Time.deltaTime, transform.localScale.z);
		} else {
			transform.localScale = new Vector3 (transform.localScale.x - Time.deltaTime, transform.localScale.y - Time.deltaTime, transform.localScale.z);
		}
	}
}
