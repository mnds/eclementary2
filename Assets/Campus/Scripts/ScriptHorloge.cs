using UnityEngine;
using System.Collections;

public class ScriptHorloge : MonoBehaviour {

	public int dureeDuJour = 60;//duree d'un cycle jour/nuit en secondes
	public float heure;//heure à l'instant t en seconde

	// Use this for initialization
	void Start () 
	{
		heure = dureeDuJour/24f*13.5f;//on commence à 13h30
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
