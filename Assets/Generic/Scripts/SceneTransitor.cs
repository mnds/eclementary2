using UnityEngine;
using System.Collections;

public class SceneTransitor : MonoBehaviour {
	public string nomScene;
	// Use this for initialization
	void OnTriggerEnter () {
		Application.LoadLevel (nomScene);
	}
}
