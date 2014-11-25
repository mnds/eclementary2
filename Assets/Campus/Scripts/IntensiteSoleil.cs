/**
 * \file      RotationSoleil.cs
 * \author    
 * \version   1.0
 * \date      9 novembre 2014
 * \brief     Change la couleur du soleil en fonction de l'heure.
 *
 * \details   L'heure est obtenue à partir d'un GameObject nommé Horloge, qui contient le ScriptHorloge de la scène.
 */

/*
 * Utilise ScriptHorloge
 */

using UnityEngine;
using System.Collections;

public class IntensiteSoleil : MonoBehaviour {
	
	private float dureeJour;//Duree du jour en secondes
	private float heure;//heure actuelle
	private ScriptHorloge script;//horloge

	// Use this for initialization
	void Start () 
	{
		GameObject clock = GameObject.Find ("Horloge");
		script = clock.GetComponent<ScriptHorloge> ();//on récupère l'horloge
	}
	
	// Update is called once per frame
	void Update () 
	{
		dureeJour = script.dureeDuJour;
		heure = script.heure;

		//on joue sur la luminosité
		if (heure <= dureeJour*7f /24f || heure>=20f*dureeJour/24f) //entre 20h et 7h
		{
			light.intensity=0f;
		}
		else if (heure >= dureeJour*7f /24f && heure<=9f*dureeJour/24f)//entre 7h et 9h
		{
			light.intensity=0.3f*heure/dureeJour*24f-2.1f;
			Color orange = new Color(1f,(40f*heure/dureeJour*24f-105)/255f,(127.5f*heure/dureeJour*24f-892.5f)/255f);//255/175/0 à 7h, 255/255/255 à 9h et linéaire entre les deux
			light.color = orange;
		}
		else if (heure>=9f*dureeJour/24f && heure<=18f*dureeJour/24f)//entre 9h et 18h
		{
			light.intensity=0.6f;
			Color blanc = new Color(1f,1f,1f);//255/255/255
			light.color = blanc;
		}
		else //entre 18h et 20h
		{
			light.intensity=-0.3f*heure/dureeJour*24f+6f;
			Color orange = new Color(1f,(-40f*heure/dureeJour*24f+975f)/255f,(-127.5f*heure/dureeJour*24f+2550)/255f);//255/255/255 à 18h, 255/175/0 à 20h et linéaire entre les deux
			light.color = orange;
		}
	}
}