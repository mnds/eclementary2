﻿/**
 * \file      ControllerJoueur.cs
 * \author    
 * \version   1.0
 * \date      22 novembre 2014
 * \brief     Interface dont héritent toutes les classes qui servent au controle du joueur.
 */

using UnityEngine;
using System.Collections;

public class ControllerJoueur : MonoBehaviour {
	protected CharacterController cc;

	public Camera cameraOculus;
	public Camera cameraNonOculus;
	protected Camera camera;
	//Sprint
	public float vitesseMarche = 5.0f; //Vitesse maximale de marche
	public float vitesseCourse = 9.0f; //Vitesse de course
	public float jaugeMax = 10.0f;
	protected float jauge = 10.0f; //Temps maximum pendant lequel on peut courir
	protected float limiteBasseJauge = 2.0f; //Si la jauge se vide, il n'est plus possible de courir avant ce laps de temps

	// Progress bar
	public Texture2D enduranceBarTexture;
	protected int barLength = Screen.width / 6, barHeight = Screen.height / 10;

	protected float vitesseMouvement; //Vitesse actuelle max de mouvement selon qu'on marche ou qu'on court
	protected float vitesseNonVerticaleActuelle = 0f; //Vitesse actuelle de déplacement
	//Sensibilités pour la vitesse
	public float vitesseRotation = 10.0f; //Liée à la sensibilité de la souris
	public float vitesseSaut = 3.0f;
	//Angle de rotation de la camera en vertical
	protected float rotationVerticale = 0; //Relève la position de la caméra. Initialisé à 0.
	public float angleVerticalMax = 60.0f; //Pour limiter l'angle avec lequel on peut regarder vers le haut et le bas
	//Saut
	protected float velociteVerticale = 0; //Tient en compte de la gravité
	protected int nombreSautsFaits = 0; //Pour un double saut, il faut prendre en compte le nombre d'appuis sur la touche saut
	public int nombreSautsMax = 1; //Nombre de sauts maximum que le joueur peut faire. 1 pour saut, 2 pour double saut, 0 si interdit de sauter
	protected float bounce = 0f; //Pour le rebond sur des objets
	//S'accroupir
	public float characterControllerYCenterDebout = 0.84f;
	public float characterControllerHeightDebout = 1.72f;
	public float characterControllerYCenterAccroupi = 0.66f;
	public float characterControllerHeightAccroupi = 1.43f;
	
	//Bypass
	protected bool sprintPossible = true; //true si appuyer sur Sprint fait quelque chose, false sinon
	protected bool rendreImmobile = false; //Si true, les touches directionnelles sont bloquées
	protected bool bloquerTete = false; //La camera ne bouge plus
	protected bool freeze = false; //Tout bloquer. Attention, le FPC tombe pendant ce temps.
	protected bool bypass = false; //Tout bloquer. Est à true si le joueur n'est pas celui controllé par l'utilisation.
	
	//Set/Get
	public void SetRendreImmobile (bool rendreImmobile_) {
		rendreImmobile = rendreImmobile_;
	}
	
	public bool GetRendreImmobile () {
		return rendreImmobile;
	}
	
	public void SetBloquerTete (bool bloquerTete_) {
		bloquerTete = bloquerTete_;
	}
	
	public bool GetBloquerTete () {
		return bloquerTete;
	}
	
	public void SetFreeze (bool freeze_) {
		freeze = freeze_;
	}
	
	public bool GetFreeze () {
		return freeze;
	}
	
	public float GetVitesseNonVerticaleActuelle () {
		return vitesseNonVerticaleActuelle;
	}
	
	public float GetVitesseMouvement () {
		return vitesseMouvement;
	}
}
