using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Doit etre placé sur le meme gameObject que le controller de mouvements du joueur
public class InteractionEnvironnement : MonoBehaviour {
	public Camera camera; //Main camera

	public float distancePorteMax = 3.0f;
	public bool afficherTextePorte = false;
	public string textePorte = "E";

	// Update is called once per frame
	void Update () {
		//Tester si un objet est dans le champ de vision
		RaycastHit hitInfo; //On enverra un raycast
		PorteSimple porteGameObject; //On cherche un dialogue
		GameObject objet; //Objet touché
		
		if (Physics.Raycast (camera.transform.position, camera.transform.forward, out hitInfo, distancePorteMax)) { //On regarde très loin pour trouver l'objet en face du joueur
						objet = hitInfo.collider.gameObject;
						//On cherche la porte
						porteGameObject = objet.GetComponent<PorteSimple> (); //On cherche si l'objet touché en lui-meme
						if (!porteGameObject)
								porteGameObject = objet.GetComponentInParent<PorteSimple> ();
						if (!porteGameObject)
								porteGameObject = objet.GetComponentInChildren<PorteSimple> ();
			
						//Si on l'a, ce qu'une porte est devant nous
						if (porteGameObject) {
								afficherTextePorte = true;
								if (Input.GetButtonDown ("InteractionButton"))
										porteGameObject.Interact ();
						} else
								afficherTextePorte = false;
		} else
						afficherTextePorte = false;
	}

	void OnGUI () {
		if (afficherTextePorte)
						GUI.Label (new Rect (Screen.width / 2, Screen.height / 2, 500, 100), textePorte);
	}
}