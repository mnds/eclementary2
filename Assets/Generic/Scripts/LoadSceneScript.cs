using UnityEngine;
using System.Collections;

public class LoadSceneScript : MonoBehaviour {
	public string nomScene;
	// Use this for initialization
	void OnTriggerEnter (Collider collider) {
		if(collider.tag=="Player")
			Application.LoadLevel (nomScene);
	}
}
