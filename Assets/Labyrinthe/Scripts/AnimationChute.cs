/**
 * \file      AnimationChute.cs
 * \author    
 * \version   1.0
 * \date      9 novembre 2014
 * \brief     Effectue une chute de l'objet sur le coté, accompagné d'un son.
 *
 * \details   Fait passer l'objet d'une rotation initiale à une rotation finale à l'aide de Quaternions. Il revient à sa rotation initiale après un certain temps.
 */

/*
 * Utilisé dans MazeEnemy
 */

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
				cible.GetComponent<ControllerJoueur> ().SetFreeze (false);
				Retablir();
				avancementReveil = 1;
			}
		}
		
	}

	/**
	 * @brief Entraine l'animation de chute.
	 * @param tempsChute_ Le temps pendant lequel la rotation s'effectue
	 * @param tempsAvantReveil_ Le temps avant de redresser l'objet
	 *
	 * @details Bloque le script de l'objet, joue le son associé et lance l'animation en mettant à 0 son avancement.
	 */
	public void Chuter (float tempsChute_,float tempsAvantReveil_) {
		enChute = true;
		tempsChute = tempsChute_;
		tempsAvantReveil = tempsAvantReveil_;
		avancementChute = 0;
		audioSource.Play ();
		cible.GetComponent<ControllerJoueur> ().SetFreeze (true);
	}

	/**
	 * @brief Remet l'objet à sa position initiale.
	 * 
	 * @details Remet la cible à sa position initiale. Appelé dans Update à la fin du temps de reveil.
	 */
	void Retablir () {
		cible.transform.rotation = rotationInitiale;
		enChute = false;
	}
}
