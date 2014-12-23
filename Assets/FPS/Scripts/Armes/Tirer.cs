/**
 * \file      Tirer.cs
 * \author    
 * \version   1.0
 * \date      11 décembre 2014
 * \brief     Indique qu'un objet peut être utilisé pour tirer.
 *
 * \details   Ce script, placé sur un objet, indique que le bouton Fire1 lui permet d'etre utilisé pour tirer.
 */

/*
 * Utilisé dans Inventaire
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Note : lorsqu'on attaque, il est possible de jeter l'objet pendant l'animation de l'attaque.
[RequireComponent(typeof(AudioSource))] //Pour le son du tir
public class Tirer : MonoBehaviour {
	public bool bypass;//Si bI, rien ne se passe. Toutes les fonctions sont ignorées.
	public bool munitionsIllimitees = false; //Si munitions illimitées, on changera le code qui enlève des munitions

	public float degatsParBalle = 10.0f; //Degats initiaux, changés pour chaque objet
	public float cooldown = 0.1f; //Temps entre deux
	float tempsDuDernierTir = 0; //Pour savoir si on peut tirer
	public int munitions = 0; //Nombre de fois où l'objet peut etre lancé
	Inventaire inventaire ; //Il est nécessaire de dire à un inventaire que les munitions changent

	public bool affecteParCaracteristiques = true; //true si les degats de l'arme sont influencés par la caractéristique Attaque du joueur
	List<Health> objetsTouchesLorsDeCetteAttaque; //Pour éviter de taper plusieurs fois les memes objets

	Transform cameraTransform; //Position de la camera
	public float range = 50f;

	AudioSource audioSource; //Pour le tir
	public AudioClip clipTir; //Bruit du tir

	void Start () {
		cameraTransform = ControlCenter.GetCameraPrincipale ().transform;
		audioSource = GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update () {
		if(bypass) return;
		if (ControlCenter.GetCinematiqueEnCours()) return; //Rien pendant une cinématique
		//Debug.Log ("Rotation : " + transform.localRotation + " Position : " + transform.localPosition);
		//On teste si l'objet a encore des munitions
		if(munitions<=0) { //S'il n'y a plus de munitions, on désactive l'objet. Pas de problème pour eETDL, car quand on réactivera l'objet, la dernière ligne de Update sera lue et le mettra comme il faut
			gameObject.SetActive(false); //On désactive l'objet pour l'instant
		}

		//Si on demande d'attaquer, et que le cooldown est terminé
		if(Input.GetButton("Fire1") && Time.time>tempsDuDernierTir+cooldown)
		{
			if(clipTir)
				audioSource.PlayOneShot(clipTir);

			tempsDuDernierTir=Time.time;
			//On vérifie par Raycast qu'on a touché quelque chose, en partant
			Ray ray = new Ray (cameraTransform.position,cameraTransform.forward);
			RaycastHit hitInfo;
			
			if(Physics.Raycast(ray,out hitInfo, range)){
				GameObject go = hitInfo.collider.gameObject;
				if(go!=ControlCenter.GetJoueurPrincipal()){ //Si l'objet touché n'est pas le tireur
					Vector3 hitPoint = hitInfo.point; //L'endroit qu'on touche
					Debug.Log("Hit Object : "+go.name);
					//Debug.Log("Hit Point : "+hitPoint);
					
					//enlever de la vie
					Transform objetAvecVie = go.transform;
					Health health = objetAvecVie.GetComponent<Health>(); //Si le truc touché a des points de vie, on doit le blesser
					while(health == null && objetAvecVie.parent){
						objetAvecVie=objetAvecVie.parent;
						health = objetAvecVie.GetComponent<Health>();
					}
					//On récupère le script Caracteristiques du joueur principal pour appliquer les modifications aux dégats
					Caracteristiques carac = ControlCenter.GetJoueurPrincipal().GetComponent<Caracteristiques>();
					float degatsInfliges=degatsParBalle; //Initialement égal à la valeur "de base"
					if(carac && affecteParCaracteristiques) { //Formule de degats
						degatsInfliges = degatsParBalle+carac.GetAttaque(); ///FORMULE DE DEGATS
						//Debug.Log ("Degats infligés après application de l'attaque du joueur : "+degatsInfliges);
					}
					if(health != null){
						health.SubirDegats(degatsInfliges);
					}
				}

				//Traitements d'inventaire
				if(inventaire!=null) {
					if(!munitionsIllimitees) {
						inventaire.ChangerMunitions (gameObject, munitions-1); //On enlève une munition de l'arme
					}
				}
				else
					Debug.Log ("Pas d'inventaire !");
				
				//On teste si l'objet a encore des munitions
				if(munitions<=0) { //S'il n'y a plus de munitions, on désactive l'objet. Pas de problème pour eETDL, car quand on réactivera l'objet, la dernière ligne de Update sera lue et le mettra comme il faut
					gameObject.SetActive(false); //On désactive l'objet pour l'instant
				}
			}
		}
	}

	public void SetBypass(bool bypass_) {
		bypass=bypass_;
	}

	public void SetMunitions (int munitions_) {
		munitions = Mathf.Max (0,munitions_);
	}
	
	public void SetMunitionsSimple (int munitions_) {
		munitions = Mathf.Max (0,munitions_);
	}
	
	public void SetInventaire (Inventaire inventaire_) {
		inventaire = inventaire_;
	}
}
