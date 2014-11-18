using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.M)) {
			CrossThreadCommunication.EmotionAlert();
		}
	}
}
