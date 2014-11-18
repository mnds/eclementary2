/**
 * \file      LumiereFlare.cs
 * \author    
 * \version   1.0
 * \date      9 novembre 2014
 * \brief     Modifie la couleur de la lentille modélisant le soleil en fonction de l'heure.
 *
 * \details   Les propriétés de la lentille sont modifiées selon l'heure donnée par l'instance de ScriptHorloge de la scène.
 */

/*
 * Utilise ScriptHorloge
 */

using UnityEngine;
using System.Collections;

public class LumiereFlare : MonoBehaviour {

	private float dureeJour;//Duree du jour en secondes
	private float heure;//heure actuelle
	private ScriptHorloge script;//horloge
	private LensFlare lens;
	
	// Use this for initialization
	void Start () 
	{
		GameObject clock = GameObject.Find ("Horloge");
		script = clock.GetComponent<ScriptHorloge> ();//on récupère l'horloge
		lens = GetComponent<LensFlare> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		dureeJour = script.dureeDuJour;
		heure = script.heure;
		
		//on joue sur la luminosité
		if (heure <= dureeJour / 4 || heure>=3*dureeJour/4) 
		{
			lens.brightness=0;
		}
		else
		{
			lens.brightness = (-1.0f*Mathf.Abs(heure/dureeJour-0.5f)+1.0f);
			Color rose = new Color(1f,-4.0f*Mathf.Abs(heure/dureeJour-0.5f)+1f,-4.0f*Mathf.Abs(heure/dureeJour-0.5f)+1f);
			lens.color = rose;
		}
	}
}
