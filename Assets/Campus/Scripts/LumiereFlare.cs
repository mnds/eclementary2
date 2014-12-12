/**
 * \file      LumiereFlare.cs
 * \author    
 * \version   1.0
 * \date      15 Octobre 2014
 * \brief     Change la couleur et l'intensité du Lens Flare Soleil en fonction de l'heure.
 *
 * \details   L'heure est obtenue à partir d'un GameObject nommé Horloge, qui contient le ScriptHorloge de la scène.
 */

using UnityEngine;
using System.Collections;

public class LumiereFlare : MonoBehaviour {

	private float dureeJour;//Duree du jour en secondes
	private float heure;//heure actuelle
	private ScriptHorloge script;//horloge
	private LensFlare lens;

	void Start () 
	{
		GameObject clock = GameObject.Find ("Horloge");
		script = clock.GetComponent<ScriptHorloge> ();//on récupère l'horloge
		lens = GetComponent<LensFlare> ();
	}

	void Update () 
	{
		//on récupère l'heure actuelle
		dureeJour = script.dureeDuJour;
		heure = script.heure;
		
		//on joue sur la luminosité et sur la couleur en fonction de l'heure
		if (heure <= dureeJour*7f /24f || heure>=20f*dureeJour/24f) //entre 20h et 7h
		{
			lens.brightness=0;
		}
		else if (heure >= dureeJour*7f /24f && heure<=9f*dureeJour/24f)//entre 7h et 9h
		{
			lens.brightness=0.8f;
			Color orange = new Color(1f,(40f*heure/dureeJour*24f-105)/255f,(127.5f*heure/dureeJour*24f-892.5f)/255f);//couleur à 255/175/0 à 7h, 255/255/255 à 9h et linéaire entre les deux
			lens.color = orange;
		}
		else if (heure>=9f*dureeJour/24f && heure<=18f*dureeJour/24f)//entre 9h et 18h
		{
			lens.brightness=0.8f;
			Color blanc = new Color(1f,1f,1f);//couleur à 255/255/255
			lens.color = blanc;
		}
		else //entre 18h et 20h
		{
			lens.brightness=0.8f;
			Color orange = new Color(1f,(-40f*heure/dureeJour*24f+975f)/255f,(-127.5f*heure/dureeJour*24f+2550)/255f);//couleur à 255/255/255 à 18h, 255/175/0 à 20h et linéaire entre les deux
			lens.color = orange;
		}
	}
}
