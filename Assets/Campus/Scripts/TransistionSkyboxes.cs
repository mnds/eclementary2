using UnityEngine;
using System.Collections;

public class TransistionSkyboxes : MonoBehaviour {

	private float dureeJour;//Duree du jour en secondes
	private float heure;//heure actuelle
	private ScriptHorloge script;//horloge
	private Skybox skybox;//la skybox

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
		//nuit de 20h à 7h, levé du jour de 7h à 9h, couché du jour de 18h à 20h

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
