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

	static public bool getCinematiqueEnCours () {
		return cinematiqueEnCours;
	}

	static public void setCinematiqueEnCours (bool cec) {
		Debug.Log ("CC : sCEC "+cec);
		cinematiqueEnCours = cec;
	}
}
