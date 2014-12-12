/**
 * \file      AllumageLampe.cs
 * \author    
 * \version   1.0
 * \date      10 Octobre 2014
 * \brief     Permet d'allumer et d'éteindre les lampes à une heure donnée
 *
 * \details   Permet d'allumer les lampes à 19h et de les éteindre à 8h. 
 * 			  L'heure est obtenue à partir d'un GameObject nommé Horloge, qui contient le ScriptHorloge de la scène.
 */

using UnityEngine;
using System.Collections;

public class AllumageLampe : MonoBehaviour {

	private float dureeJour;//Duree du jour en secondes
	private float heure;//heure actuelle
	private ScriptHorloge script;//horloge
	
	void Start () 
	{
		GameObject clock = GameObject.Find ("Horloge");
		script = clock.GetComponent<ScriptHorloge> ();//on récupère l'horloge
	}

	void Update () 
	{
		//on récupère l'heure actuelle
		dureeJour = script.dureeDuJour;
		heure = script.heure;
		if(heure >= 8f*dureeJour/24 && heure <= 19f*dureeJour/24)
		{
			//entre 8h et 19h, on éteint
			light.intensity=0f;
		}
		else
		{
			//et sinon on allume
			if(light.intensity<1f)
				light.intensity=1f;
		}
	}
}
