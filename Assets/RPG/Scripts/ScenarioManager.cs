/**
 * \file      ScenarioManager.cs
 * \author    
 * \version   1.0
 * \date      7 décembre 2014
 * \brief     Contient tous les événements du jeu et permet de les lancer quand il le faut.
 *
 * \details   L'heure est obtenue à partir d'un GameObject nommé Horloge, qui contient le ScriptHorloge de la scène.
 */

using UnityEngine;
using System.Collections;

static public class ScenarioManager {
	static private int idScenePrecedente;
	/**
	 * @brief Active un événement grace à un id.
	 * @param id Numéro lié à l'événement à enclencher.
	 *
	 * @details Toutes les définitions des événements se trouvent directement dans cette fonction.
	 * 			-1 : Quitter l'application
	 * 			0 : Afficher écran de mort
	 * 			1 : Revenir à la vie à la scène précédente
	 */
	static public void ActiverEvenement (int idEvent) {
		Debug.Log ("Evénement numéro " + idEvent);
		switch (idEvent) {
		case -1:
			Debug.Log ("Quitter l'application");
			Application.Quit();
			break;
		case 0:
			Debug.Log ("Afficher écran de mort");
			idScenePrecedente = Application.loadedLevel;
			Debug.Log ("id Precedent : "+idScenePrecedente);
			Application.LoadLevel ("Ecran Mort");
			break;
		case 1:
			Debug.Log ("Revenir à la vie à la scène précédente");
			Application.LoadLevel (idScenePrecedente);
			//ControlCenter.GetJoueurPrincipal().GetComponent<Health>().Soigner(100000); //Soin total !
			break;
		default:
			Debug.Log ("Evénement inconnu.");
			break;
		}
	}
}
