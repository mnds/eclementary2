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

public class HealthPlayer : Health, IScriptEtatJouable {
	public bool joueurPrincipal; //pour le multijoueur, pour savoir qui afficher
	public Texture2D healthBarTexture;
	int barLength = Screen.width / 6, barHeight = Screen.height / 10;

	private bool enabled = true; // variable booléenne qui servira à l'implémentation des méthodes de IScriptEtatJouable

	void Start () {
		if(gameObject==ControlCenter.GetJoueurPrincipal())
			joueurPrincipal=true;
	}

	public void Update () {
		if (!enabled)
			return;
	}

	public void OnGUI () {
		if (!enabled)
			return;
		if(!joueurPrincipal) return; //N'afficher que la barre du joueur controllé par l'utilisateur

		//Barre de vie
		if (ControlCenter.GetAfficherBarreDeVieJoueur ()) {
			GUI.Box (new Rect (Screen.width * 5 / 6, Screen.height * 1 / 10, barLength, barHeight), "Vie"); // Points de vie max
			if(! (pointsDeVieActuels/pointsDeVieMax < 0.1) ) // La barre n'est affichée qu'au delà d'un certain seuil
				GUI.Box (new Rect (Screen.width * 5 / 6, Screen.height * 1 / 10, Mathf.Ceil(pointsDeVieActuels/pointsDeVieMax * barLength), barHeight), healthBarTexture); // Points de vie du joueurGUI.Box (new Rect (Screen.width * 5 / 6, Screen.height * 1 / 10, barLength, barHeight), "Vie"); // Points de vie max
			if(! (pointsDeVieActuels/pointsDeVieMax < 0.1) ) // La barre n'est affichée qu'au delà d'un certain seuil
				GUI.Box (new Rect (Screen.width * 5 / 6, Screen.height * 1 / 10, Mathf.Ceil(pointsDeVieActuels/pointsDeVieMax * barLength), barHeight), healthBarTexture); // Points de vie du joueur
		}

		//Barre de mana
		if(ControlCenter.GetAfficherBarreDeManaJoueur()) {
			GUI.Box (new Rect (Screen.width * 5 / 6, Screen.height * 3 / 10, barLength, barHeight), "Mana"); // Points de vie max
			if(! (pointsDeManaActuels/pointsDeManaMax < 0.1) ) // La barre n'est affichée qu'au delà d'un certain seuil
				GUI.Box (new Rect (Screen.width * 5 / 6, Screen.height * 3 / 10, Mathf.Ceil(pointsDeManaActuels/pointsDeManaMax * barLength), barHeight), healthBarTexture); // Points de vie du joueur
		}
	}

	public override void DeclencherMort () {
		//ScenarioManager.ActiverEvenement (0); //Ecran de mort
		Evenement mourir = new Mourir ();
		mourir.DeclencherEvenement ();
	}

	// Implémentation de IScriptEtatJouable
	public bool isEnabled() {
		return enabled;
	}

	public void setEnabled( bool ok ) {
		enabled = ok;
	}
	
}
