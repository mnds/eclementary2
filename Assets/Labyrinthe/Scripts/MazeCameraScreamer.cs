using UnityEngine;
using System.Collections.Generic;

public class MazeCameraScreamer : MonoBehaviour {
	public List<AudioClip> mazeSounds;
	public List<GameObject> mazeScreamers;
	private int rangScreamerActuel = -1; //Contiendra l'information du screamer à désactiver

	// Use this for initialization
	void Start () {
		for(int i=0;i<mazeScreamers.Count;i++)
		{
			mazeScreamers[i].GetComponent<SpriteRenderer>().enabled=false;
		}
	}
	
	public void Activer () {
		if(rangScreamerActuel==-1) { //On prend garde à ne pas activer deux screamers à la fois. Si c'est -1, pas de screamer. Sinon, on ne fait rien
			//Récupération d'un screamer et d'un son. Chaque screamer a un son.
			rangScreamerActuel=Random.Range (0, mazeScreamers.Count);
			GameObject screamer = mazeScreamers [rangScreamerActuel];
			AudioClip sound = mazeSounds [rangScreamerActuel];
			//On joue le clip et on affiche le screamer
			AudioSource sourceAudio = GetComponent<AudioSource>();
			sourceAudio.PlayOneShot(sound);
			screamer.GetComponent<SpriteRenderer> ().enabled = true;
		}
	}

	public void Desactiver () {
		//On retrouve le screamer, et on l'enlève
		GameObject screamer = mazeScreamers [rangScreamerActuel];
		screamer.GetComponent<SpriteRenderer> ().enabled = false;
		rangScreamerActuel = -1; //On peut refaire un affichage
	}
}
