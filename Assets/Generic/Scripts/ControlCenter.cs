/**
 * \file      ControlCenter.cs
 * \author    
 * \version   1.0
 * \date      9 novembre 2014
 * \brief     Contient des variables globales liées au bon déroulement du jeu.
 *
 * \details   Quand une cinématique est lancée, cinematiqueEnCours passe à true. Les appels à getCEC renvoient true et permettent d'empecher des actions de se produire.
 */

/*
 * Utilisé dans Inventaire, MoveCamera , Attaquer, Lancer
 */

using UnityEngine;
using System.Collections;


static public class ControlCenter {
	static bool cinematiqueEnCours = false; //Les interactions doivent s'arreter si on est en cinématique
	static bool afficherBarreDeVieJoueur = true;
	static bool afficherBarreEnduranceJoueur = true;
	static private bool abdvjAncien; //Pour les changements
	static private bool abejAncien;
	static private bool utiliserOculus = false;
	static private bool pause = false;

	static HealthPlayer healthPlayer;

	static public bool GetAfficherBarreDeVieJoueur () {
		return afficherBarreDeVieJoueur;
	}

	static public bool GetAfficherBarreEnduranceJoueur () {
		return afficherBarreEnduranceJoueur;
	}

	static public bool GetCinematiqueEnCours () {
		return cinematiqueEnCours;
	}

	static public void SetCinematiqueEnCours (bool cec) {
		Debug.Log ("Control Center : cinematique "+cec);
		cinematiqueEnCours = cec;
		//L'affichage dépend de cinematique
		if(cec) {
			abdvjAncien = afficherBarreDeVieJoueur;
			abejAncien = afficherBarreEnduranceJoueur;
			afficherBarreDeVieJoueur = cec;
			afficherBarreEnduranceJoueur = cec;
		}
		else
		{
			afficherBarreDeVieJoueur = abdvjAncien;
			afficherBarreEnduranceJoueur = abejAncien;
		}


	}

	static public void SetAfficherBarreDeVieJoueur (bool abdvj) {
		Debug.Log ("Control Center : affichage vie joueur "+abdvj);
		afficherBarreDeVieJoueur = abdvj;
	}

	static public void SetAfficherBarreEnduranceJoueur (bool abdej) {
		Debug.Log ("Control Center : affichage stamina joueur "+abdej);
		afficherBarreEnduranceJoueur = abdej;
	}

	static public void SetHealthPlayer (HealthPlayer hp) {
		healthPlayer = hp;
	}

	static public bool GetUtiliserOculus () {
		return utiliserOculus;
	}

	static public void SetPause (bool pause_) {
		pause=pause_;
		if(pause) //Le control center se charge d'arreter le temps
			Time.timeScale=0;
		else
			Time.timeScale=1f;
	}

	static public bool GetPause () {
		return pause;
	}
}
