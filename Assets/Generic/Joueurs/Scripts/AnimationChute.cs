using UnityEngine;
using System.Collections;

public class AnimationChute : MonoBehaviour {
	public Quaternion rotationInitiale;
	public Quaternion rotationFinale;
	public GameObject cible; //Ce qui va tourner
	float avancementChute = 1;
	float avancementReveil = 1;
	float tempsChute;
	float tempsAvantReveil;
	public AudioSource audioSource;
	public bool enChute = false;

	// Use this for initialization
	void Start () {
		rotationInitiale = cible.transform.rotation;
	}
	
	public void Chuter (float tempsChute_,float tempsAvantReveil_) {
		enChute = true;
		tempsChute = tempsChute_;
		tempsAvantReveil = tempsAvantReveil_;
		avancementChute = 0;
		audioSource.Play ();
		cible.GetComponent<FPCClassic> ().SetFreeze (true);
	}

	void Retablir () {
		cible.transform.rotation = rotationInitiale;
		enChute = false;
	}

	void Update () {
		if (avancementChute<1) {
			avancementChute+=Time.deltaTime/tempsChute;
			if(avancementChute>=1) {//Si c'est fini
				avancementReveil = 0; //On lance le réveil
				avancementChute = 1;
			}
			cible.transform.rotation = Quaternion.Lerp(rotationInitiale,rotationFinale,avancementChute);
		}
		if (avancementReveil < 1) {
			avancementReveil+=Time.deltaTime/tempsAvantReveil;
			if (avancementReveil >= 1) {
				cible.GetComponent<FPCClassic> ().SetFreeze (false);
				Retablir();
				avancementReveil = 1;
			}
		}

	}
}
