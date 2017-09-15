using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setBOXdown : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds (0.15f);
		Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += new Vector3 (0.0f, 30.0f, 0.0f) * Time.deltaTime;
	}
}
