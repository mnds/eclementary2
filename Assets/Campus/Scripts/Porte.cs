using UnityEngine;
using System.Collections;

public class Porte : MonoBehaviour {
	bool interactive = true;
	bool open = false;
	bool opening = false;
	bool close = true;
	bool closing = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Close () {
		
	}

	void Open () {
		if (open || opening || !interactive)
						return;
		if (!open && !opening) {
				
		}
	}
}
