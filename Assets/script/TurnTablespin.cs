using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTablespin : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (new Vector3 (0f, 2f, 0f)*Time.deltaTime);
	}
}
