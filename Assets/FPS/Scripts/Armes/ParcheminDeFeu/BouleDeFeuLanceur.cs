/**
 * \file      BouleDeFeuLanceur.cs
 * \author    
 * \version   1.0
 * \date      22 février 2015
 * \brief     Indique qu'un objet peut être utilisé pour lancer de chatoyantes boules de feu.
 *
 * \details   Ce script, placé sur un objet, indique que le bouton Fire1 lui permet d'etre utilisé pour lancer une boule de feu et Fire2 une aura de feu.
 */

/*
 * Utilisé dans Inventaire
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Note : lorsqu'on attaque, il est possible de jeter l'objet pendant l'animation de l'attaque.
[RequireComponent(typeof(AudioSource))] //Pour le son du tir
public class BouleDeFeuLanceur : MonoBehaviour {
	public bool bypass;//Si bI, rien ne se passe. Toutes les fonctions sont ignorées.
	public float cooldown = 1f; //Temps entre deux tirs de boule de feu;
	float tempsDuDernierTir = 0f; //Pour savoir si on peut tirer
	float tempsDernierAura = 0f; //Pour savoir si on peut utiliser le cercle de feu
	public GameObject orbeDeFeu;
	public float manaBouleDeFeu = 2f;
	public float manaAnneau = 3f;
	private Camera mainCamera; //pour lancer l'objet depuis la position de la caméra


	public float cooldownRing = 20f; //cooldown avant de relancer l'aura
	public GameObject anneauDeFeu;
	public GameObject anneauRender;
	private bool boolSuivi= false;

	public bool affecteParCaracteristiques = true; //true si les degats de l'arme sont influencés par la caractéristique Attaque du joueur
	List<Health> objetsTouchesLorsDeCetteAttaque; //Pour éviter de taper plusieurs fois les memes objets

	private float vitesseBouleDeFeu = 25f; //la vitesse de la boule lorsqu'elle part

	AudioSource audioSource; //Pour le tir
	public AudioClip clipTir; //Bruit du tir
	

	void Start () {
		mainCamera = ControlCenter.GetCameraPrincipale ();
		audioSource = GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update () {
		if(bypass) return;
		if (ControlCenter.GetCinematiqueEnCours()) return; //Rien pendant une cinématique

		//Si on demande d'attaquer, et que le cooldown est terminé
		if(Input.GetButton("Fire1") && Time.time>tempsDuDernierTir+cooldown)
		{
			if(!ControlCenter.GetJoueurPrincipal().GetComponent<Health>().SubirDegatsMana(manaBouleDeFeu))
				return; // on enlève le mana consommé par le sort

			if(clipTir)
				audioSource.PlayOneShot(clipTir);


			tempsDuDernierTir=Time.time;
			//On lance une boule de type Rigidbody avec une vitesse 25
			GameObject cloneOrbe;
			cloneOrbe = (GameObject)Instantiate (orbeDeFeu, mainCamera.transform.position+mainCamera.transform.forward, mainCamera.transform.rotation);
			cloneOrbe.rigidbody.AddForce(vitesseBouleDeFeu*mainCamera.transform.forward,ForceMode.Impulse);

		}

		// attaque secondaire cercle de feu
		if(Input.GetButton("Fire2") && Time.time>tempsDernierAura+cooldownRing)
		{
			if(!ControlCenter.GetJoueurPrincipal().GetComponent<Health>().SubirDegatsMana(manaAnneau))
				return;  // on enleve le mana consommé par le sort

			tempsDernierAura=Time.time;

			GameObject cloneAnneau;
			cloneAnneau = (GameObject)Instantiate (anneauDeFeu, mainCamera.transform.position+mainCamera.transform.forward, mainCamera.transform.rotation);
			cloneAnneau.transform.parent=ControlCenter.GetJoueurPrincipal().transform;

			GameObject cloneRender;
			cloneRender = (GameObject)Instantiate (anneauRender, mainCamera.transform.position+mainCamera.transform.forward-new Vector3(0f,1f,0f), Quaternion.identity);
			cloneRender.transform.parent=ControlCenter.GetJoueurPrincipal().transform;
			Destroy (cloneRender,10f);
		}

	}

	public void SetBypass(bool bypass_) {
		bypass=bypass_;
	}
}
