using UnityEngine;
using System.Collections.Generic;

public class MazeOscillerSon : MonoBehaviour {
	GameObject fpc;

	// Use this for initialization
	void Start () {
		fpc = GameObject.Find ("First Person Controller");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float weight = Mathf.Cos (Time.time * 2 * Mathf.PI + 0.5F);
		transform.position = fpc.transform.position - fpc.transform.forward + fpc.transform.right * weight;
	}
}
