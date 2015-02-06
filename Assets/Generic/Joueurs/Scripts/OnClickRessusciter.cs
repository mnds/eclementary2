using UnityEngine;
using System.Collections;

public class OnClickRessusciter : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnGUI() {
		if (GUI.Button (new Rect (new Rect (Screen.width / 2, Screen.height * 9 / 10, 200, 50)), "Ressusciter")) {
			Evenement ressusciter = new Ressusciter();
			ressusciter.DeclencherEvenement();
		}
	}
	
}
