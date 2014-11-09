/**
 * \file      PorteSimple.cs
 * \author    
 * \version   1.0
 * \date      9 novembre 2014
 * \brief     Permet de faire tourner une porte.
 *
 * \details   Permet la rotation selon l'axe y d'une porte déjà placée dans la scène. Le paramètre openedStart permet de choisir
 * 			  si la porte tourne dans le sens horaire ou anti-horaire. doorAngle et speed modifie le mouvement de la porte.
 */

/*
 * Utilisé dans InteractionEnvironnement
 */

using UnityEngine;
using System.Collections;

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

	/**
	 * @brief Permet le mouvement de la porte selon sa position et l'action du joueur.
	 *
	 * @details Appelle deux autres méthodes pour fermer/ouvrir la porte. La fermeture est prioritaire.
	 */
	void MoveDoor () {
		Close ();
		Open ();
	}

	/**
	 * @brief Ferme la porte.
	 *
	 * @details La porte se ferme si elle n'est pas déjà fermée ou en train de se fermer. On utilise pour cela Roa
	 */
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

	/**
	 * @brief Ouvre la porte.
	 *
	 * @details La porte s'ouvre si elle n'est pas déjà ouverte ou en train de s'ouvrir. On utilise pour cela Rotate.
	 */
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

	/**
	 * @brief Permet l'interaction avec la porte depuis un autre script.
	 *
	 * @details Quand la porte est sollicitée et qu'elle peut interagir, l'appel à ce script la fait se fermer ou s'ouvrir.
	 * 	        Si la porte est fermée ou en train de se fermer, on l'ouvre et vice-versa.
	 */
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
