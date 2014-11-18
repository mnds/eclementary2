/**
 * \file      RotationFlare.cs
 * \author    
 * \version   1.0
 * \date      9 novembre 2014
 * \brief     Entraine le mouvement circulaire de la lentille représentant le soleil en fonction de l'heure.
 *
 * \details   La position de la lentille est modifiée selon l'heure donnée par l'instance de ScriptHorloge de la scène.

/*
 * Utilise ScriptHorloge
 */

using UnityEngine;
using System.Collections;

public class RotationFlare : MonoBehaviour {


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

		transform.Rotate (-360/dureeJour*Time.deltaTime, 0,0,Space.World);

	}
}
