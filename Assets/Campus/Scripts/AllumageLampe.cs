/**
 * \file      AllumageLampe.cs
 * \author    
 * \version   1.0
 * \date      9 novembre 2014
 * \brief     Modifie l'intensité de la lumière émise en fonction de l'heure.
 *
 */

/*
 * Utilise ScriptHorloge
 */

using UnityEngine;
using System.Collections;

public class AllumageLampe : MonoBehaviour {

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
		//Debug.Log ("1 " + 3 * dureeJour / 10 + " 2 " + 7 * dureeJour / 10 + " 3 " + heure);
		if(heure >= 3.5f*dureeJour/10 && heure <= 6.5f*dureeJour/10)
		{
			light.intensity=0f;
		}
		else
		{
			light.intensity=1f;
		}
	}
}
