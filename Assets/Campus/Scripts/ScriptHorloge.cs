/**
 * \file      ScriptHorloge.cs
 * \author    
 * \version   1.0
 * \date      15 Septembre 2014
 * \brief     Permet de gérer l'heure dans le jeu
 *
 * \details   
 * 			  
 */

using UnityEngine;
using System.Collections;

public class ScriptHorloge : MonoBehaviour {

	public int dureeDuJour = 60;//duree d'un cycle jour/nuit en secondes
	public float heure;//heure à l'instant t en seconde

	void Start () 
	{
		ChangerHeure(13.5f);
	}

	void Update () {
		heure += Time.deltaTime;//On met l'heure actuelle à jour

		if (heure >= dureeDuJour) 
		{
			//Si on dépasse la durée du jour, on remet heure à 0
			heure -= dureeDuJour;
		}
	}

	void OnLevelWasLoaded (int level) { //Changer selon la scène
		if(Application.loadedLevelName==ControlCenter.nomDeLaSceneDuCampus) { //Numero de la scène du campus
			ChangerHeure(13.5f);
		}
	}

	/**
	 * @brief Change l'heure de l'horloge
	 * @param heureDeLaJournee 13.5f correspond à 13h30
	 */
	void ChangerHeure (float heureDeLaJournee) {
		heure = dureeDuJour/24f*heureDeLaJournee;
	}
}
