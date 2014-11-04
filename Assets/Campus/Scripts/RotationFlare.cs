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
