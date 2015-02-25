using UnityEngine;
using System.Collections;

/**
 * \file      Sauvegarde.cs
 * \author    
 * \version   1.0
 * \date      25 février 2015
 * \brief     Interaction qui permet la sauvegarde de l'état actuel du jeu, lorsque le joueur interagit avec le lit
 */

public class SauvegardeLit : MonoBehaviour, Interactif {

	public float distanceMinimaleInteraction = 4.0f; //La distance à laquelle on doit etre pour pouvoir interagir avec l'objet

	// Use this for initialization
	void Start () {
	
	}

	// Sauvegarde lorsque le joueur interagit avec le lit
	public void DemarrerInteraction() {
		Debug.Log ("Sauvegarde du jeu...");
		new SauverGameData ().DeclencherEvenement ();
	}

	public void ArreterInteraction() {

	}

	public void SetDistanceMinimaleInteraction (float distanceMinimaleInteraction_) {
		distanceMinimaleInteraction = distanceMinimaleInteraction_;
	}

	public float GetDistanceMinimaleInteraction () {
		return distanceMinimaleInteraction;
	}
}
