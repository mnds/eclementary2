/**
 * \file      ScriptHorloge.cs
 * \author    
 * \version   1.0
 * \date      9 novembre 2014
 * \brief     Conserve l'heure qu'il est.
 *
 * \details   Il est possible de définir la durée d'un cycle jour/nuit et de récupérer l'heure avec ScriptHorloge.heure.
 */

/*
 * Script utilisé dans RotationSoleil , LumiereFlare , RotationFlare , LumiereSoleil , AllumageLampe
 */

using UnityEngine;
using System.Collections;

public class ScriptHorloge : MonoBehaviour {

	public int dureeDuJour = 60;//duree d'un cycle jour/nuit en secondes
	public float heure;//heure à l'instant t en seconde

	// Use this for initialization
	void Start () 
	{
		heure = dureeDuJour/2;//on commence à midi
	}
	
	// Update is called once per frame
	void Update () {
		heure += Time.deltaTime;

		if (heure >= dureeDuJour) 
		{
			heure -= dureeDuJour;
		}
	}

}
