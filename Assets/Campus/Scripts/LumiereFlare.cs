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
		if (heure <= dureeJour*7f /24f || heure>=20f*dureeJour/24f) //entre 20h et 7h
		{
			lens.brightness=0;
		}
		else if (heure >= dureeJour*7f /24f && heure<=9f*dureeJour/24f)//entre 7h et 9h
		{
			lens.brightness=0.8f;
			Color orange = new Color(1f,(40f*heure/dureeJour*24f-105)/255f,(127.5f*heure/dureeJour*24f-892.5f)/255f);//255/175/0 à 7h, 255/255/255 à 9h et linéaire entre les deux
			lens.color = orange;
		}
		else if (heure>=9f*dureeJour/24f && heure<=18f*dureeJour/24f)//entre 9h et 18h
		{
			lens.brightness=0.8f;
			Color blanc = new Color(1f,1f,1f);//255/255/255
			lens.color = blanc;
		}
		else //entre 18h et 20h
		{
			lens.brightness=0.8f;
			Color orange = new Color(1f,(-40f*heure/dureeJour*24f+975f)/255f,(-127.5f*heure/dureeJour*24f+2550)/255f);//255/255/255 à 18h, 255/175/0 à 20h et linéaire entre les deux
			lens.color = orange;
		}
	}
}
