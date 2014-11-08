using UnityEngine;
using System.Collections;

//Opens/closes doors. To be put on an object tagged with Door
//Warning : the door must rotate around its y axis, with an y rotation fixed initially so that its eulerAngles stay between 0 and 360.
//For example, if the door is not supposed to open more than 180 degrees, fix initially rotation.y to 180
public class PorteSimple : MonoBehaviour {
	public bool openedStart = false;
	public bool interactiveStart = true;
	bool interactive = true;
	bool opened;
	bool opening = false;
	bool closed;
	bool closing = false;

	public float doorAngle = 100.0f;
	public float speed = 40f;
	float angleActuel = 0f;

	public string toucheOuverture = "InteractionButton";
	public string toucheFermeture = "InteractionButton";


	void Awake () {
		interactive = interactiveStart;
		opened = openedStart;
		closed = !opened;
		if (opened)
						angleActuel = doorAngle;
	}

	void Update () {
		MoveDoor ();
		Debug.Log (angleActuel);
	}

	void MoveDoor () {
		Close ();
		Open ();
	}

	void Close () {
		//Close the door
		if (!closed && closing) { //Door not already closing or closed
			opened = false;
			gameObject.transform.Rotate (0,Time.deltaTime*speed,0);
			angleActuel-=Time.deltaTime*speed;
			if(Mathf.Abs(angleActuel)<2*Time.deltaTime*speed)
				closed = true; //If closed position, is closed
		}
	}

	void Open () {
		//Open the door
		if (!opened && opening) {
			closed = false;
			gameObject.transform.Rotate (0,-Time.deltaTime*speed,0);
			angleActuel+=Time.deltaTime*speed;
			if(Mathf.Abs(angleActuel-doorAngle)<2*Time.deltaTime*speed)
				opened = true;
		}
	}

	public void Interact () {
		if (!interactive) return; //Cannot interact

		if (opening) { //Si on était en train d'ouvrir, on ferme
			opening = false;
			closing = true;
			return;
		}
		if (closing) { //Si on était en train de ferme, on ouvre
			opening = true;
			closing = false;
			return;
		}
		//No movement yet
		if(!closed) { //Si pas fermé, on ferme
			closing = true;
			return;
		}
		else {
			opening = true;
			return;
		}
	}
}
