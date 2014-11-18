/**
 * \file      LumiereSoleil.cs
 * \author    
 * \version   1.0
 * \date      9 novembre 2014
 * \brief     Modifie la couleur de la lumière du soleil en fonction de l'heure.
 *
 * \details   Les propriétés de la lumière sont modifiées selon l'heure donnée par l'instance de ScriptHorloge de la scène.
 */

/*
 * Utilise ScriptHorloge
 */

using UnityEngine;
using System.Collections;

public class LumiereSoleil : MonoBehaviour {
	
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
		if (heure <= dureeJour / 4 || heure>=3*dureeJour/4) 
		{
			light.intensity=0;
		}
		else
		{
			light.intensity = (-2.4f*Mathf.Abs(heure/dureeJour-0.5f)+0.6f);
			Color rose = new Color(1f,-1.2f*Mathf.Abs(heure/dureeJour-0.5f)+1f,-1.2f*Mathf.Abs(heure/dureeJour-0.5f)+1f);
			light.color = rose;
		}
	}
}