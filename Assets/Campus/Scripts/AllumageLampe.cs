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
		if(heure >= 8f*dureeJour/24 && heure <= 19f*dureeJour/24)
		{
			light.intensity=0f;
		}
		else
		{
			if(light.intensity<1f)
				light.intensity=1f;
		}
	}
}
