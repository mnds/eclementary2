/**
 * \file      HealthPlayer.cs
 * \author    
 * \version   1.0
 * \date      9 novembre 2014
 * \brief     Contient les points de vie d'un joueur. Le champ joueurPrincipal permet de déterminer qui est le joueur principal.
 * 			  S'occupe de l'affichage des points de vie à l'écran.
 */

using UnityEngine;
using System.Collections;

public class HealthPlayer : Health {
	public bool joueurPrincipal; //pour le multijoueur, pour savoir qui afficher
	public Texture2D healthBarTexture;
	int barLength = Screen.width / 6, barHeight = Screen.height / 10;

	void Start () {
		if(gameObject==ControlCenter.GetJoueurPrincipal())
			joueurPrincipal=true;
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Backspace)) {
			DeclencherMort();		
		}
	}

	void OnGUI () {
		if(!joueurPrincipal) return; //N'afficher que la barre du joueur controllé par l'utilisateur
		if (!ControlCenter.GetAfficherBarreDeVieJoueur ()) return;
		GUI.Box (new Rect (Screen.width * 5 / 6, Screen.height * 1 / 10, barLength, barHeight), "Vie"); // Points de vie max
		if(! (pointsDeVieActuels/pointsDeVieMax < 0.1) ) // La barre n'est affichée qu'au delà d'un certain seuil
			GUI.Box (new Rect (Screen.width * 5 / 6, Screen.height * 1 / 10, Mathf.Ceil(pointsDeVieActuels/pointsDeVieMax * barLength), barHeight), healthBarTexture); // Points de vie du joueur
	}

	public override void DeclencherMort () {
//		ScenarioManager.ActiverEvenement (0); //Ecran de mort
	}
}
