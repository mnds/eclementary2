/**
 * \file      TransitionSkyboxes.cs
 * \author    
 * \version   1.0
 * \date      11 Novembre 2014
 * \brief     Permet de modifier la Skybox en fonction de l'heure
 *
 * \details   On a la Skybox de jour entre 9h et 18h, la Skybox de nuit entre 20h et 7h et une transition entre les deux 
 * 			  L'heure est obtenue à partir d'un GameObject nommé Horloge, qui contient le ScriptHorloge de la scène.
 */

using UnityEngine;
using System.Collections;

public class TransitionSkyboxes : MonoBehaviour {

	private float dureeJour;//Duree du jour en secondes
	private float heure;//heure actuelle
	private ScriptHorloge script;//horloge
	private Skybox skybox;//la skybox

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

		//nuit de 20h à 7h, levé du jour de 7h à 9h, couché du jour de 18h à 20h
		if(RenderSettings.skybox) {
			if (heure < dureeJour*7/24 || heure > dureeJour*20/24)
			{
				RenderSettings.skybox.SetFloat("_Blend",1f);
			}
			else if (heure >= dureeJour*7/24 && heure<= dureeJour*9/24) 
			{
				RenderSettings.skybox.SetFloat("_Blend",(dureeJour*9/24-heure)/(dureeJour*2/24));
			}
			else if (heure > dureeJour*9/24 && heure < dureeJour*18/24)
			{
				RenderSettings.skybox.SetFloat("_Blend",0f);
			}
			else if (heure > dureeJour*18/24 && heure < dureeJour*20/24)
			{
				RenderSettings.skybox.SetFloat("_Blend",(heure-dureeJour*18/24)/(dureeJour*2/24));
			}
		}
	}
}
