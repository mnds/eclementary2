/**
 * \file      IntensiteSoleil.cs
 * \author    
 * \version   1.0
 * \date      15 Octobre 2014
 * \brief     Change la couleur et l'intensité de la lumière du soleil en fonction de l'heure.
 *
 * \details   L'heure est obtenue à partir d'un GameObject nommé Horloge, qui contient le ScriptHorloge de la scène.
 */

using UnityEngine;
using System.Collections;

public class IntensiteSoleil : MonoBehaviour {
	
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

		//on joue sur la luminosité et la couleur en fonction de l'heure 
		if (heure>=20f*dureeJour/24f || heure <= dureeJour*7f /24f ) //entre 20h et 7h
		{
			light.intensity=0f;
		}
		else if (heure >= dureeJour*7f /24f && heure<=9f*dureeJour/24f)//entre 7h et 9h
		{
			light.intensity=0.3f*heure/dureeJour*24f-2.1f;
			Color orange = new Color(1f,(40f*heure/dureeJour*24f-105)/255f,(127.5f*heure/dureeJour*24f-892.5f)/255f);//couleur à 255/175/0 à 7h, 255/255/255 à 9h et linéaire entre les deux
			light.color = orange;
		}
		else if (heure>=9f*dureeJour/24f && heure<=18f*dureeJour/24f)//entre 9h et 18h
		{
			light.intensity=0.6f;
			Color blanc = new Color(1f,1f,1f);//couleur à 255/255/255
			light.color = blanc;
		}
		else //entre 18h et 20h
		{
			light.intensity=-0.3f*heure/dureeJour*24f+6f;
			Color orange = new Color(1f,(-40f*heure/dureeJour*24f+975f)/255f,(-127.5f*heure/dureeJour*24f+2550)/255f);//couleur à 255/255/255 à 18h, 255/175/0 à 20h et linéaire entre les deux
			light.color = orange;
		}
	}
}