using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FeadoutCo : MonoBehaviour {

	float fead = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		fead += Time.deltaTime;
		if (fead > 2)
			Destroy (gameObject);
	}
}
