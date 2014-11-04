using UnityEngine;
using System.Collections;

public class scriptHorloge : MonoBehaviour {

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
