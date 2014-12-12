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
		heure = dureeDuJour/24f*13.5f;//on commence le jeu à 13h30
	}

	void Update () {
		heure += Time.deltaTime;//On met l'heure actuelle à jour

		if (heure >= dureeDuJour) 
		{
			//Si on dépasse la durée du jour, on remet heure à 0
			heure -= dureeDuJour;
		}
	}
}
