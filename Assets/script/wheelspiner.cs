using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheelspiner : MonoBehaviour {

	private float changSP = 0;
	public float changTime;

	private float RandX, RandY, RandZ;
	private float SpinX = 0, SpinY = 0, SpinZ = 0;
	// Use this for initialization
	void Start () {
		RandX = Random.Range (-3.0f, 3.0f);
		RandY = Random.Range (-3.0f, 3.0f);
		RandZ = Random.Range (-3.0f, 3.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
		changSP += Time.deltaTime;
		if (changTime < changSP) {
			changSP = 0;
			RandX = Random.Range (-3.0f, 3.0f);
			RandY = Random.Range (-3.0f, 3.0f);
			RandZ = Random.Range (-3.0f, 3.0f);
		}
		transform.Rotate(new Vector3(SpinX, SpinY, SpinZ));
		SpinX += (RandX-SpinX)*(Time.deltaTime / changTime);
		SpinY += (RandY-SpinY)*(Time.deltaTime / changTime);
		SpinZ += (RandZ-SpinZ)*(Time.deltaTime / changTime);
	}

}
