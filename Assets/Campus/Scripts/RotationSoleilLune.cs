/**
 * \file      RotationSoleilLune.cs
 * \author    
 * \version   1.0
 * \date      20 Octobre 2014
 * \brief     Permet la rotation du Soleil et la Lune.
 *
 * \details   L'heure est obtenue à partir d'un GameObject nommé Horloge, qui contient le ScriptHorloge de la scène.
 * 			  
 */

using UnityEngine;
using System.Collections;

public class RotationSoleilLune : MonoBehaviour {


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
		dureeJour = script.dureeDuJour;
		heure = script.heure;

		if(heure/dureeJour*24f>=7f && heure/dureeJour*24f<=20f)//Pendant la journée on tourne de 180°
		{
			transform.Rotate (-180/(dureeJour*(13f/24f))*Time.deltaTime, 0,0,Space.World);
		}
		else //Pendant la nuit on tourne de 180°
		{
			transform.Rotate (-180/(dureeJour*(11f/24f))*Time.deltaTime, 0,0,Space.World);
		}

	}
}
