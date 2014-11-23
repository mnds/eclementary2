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
