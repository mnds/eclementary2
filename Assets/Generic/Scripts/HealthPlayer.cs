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
	private bool joueurPrincipal = true; //pour le multijoueur, pour savoir qui afficher

	void Awake () {
		if(joueurPrincipal)
			ControlCenter.SetHealthPlayer (this);
	}

	void OnGUI () {
		if(!joueurPrincipal) return;
		GUI.Label (new Rect (Screen.width * 5 / 6, Screen.height * 1 / 10, Screen.width / 6, Screen.height / 10), "Points de vie : "+pointsDeVieActuels);
	}

	public override void DeclencherMort () {

	}
}
