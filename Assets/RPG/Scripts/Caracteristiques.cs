/**
 * \file      Caracteristiques.cs
 * \author    
 * \version   1.0
 * \date      29 novembre 2014
 * \brief     Contient les statistiques du gameObject sur lequel ce script est attaché.
 *
 * \details   Contient les caractéristiques naturelles du personnage (bonus de vie, endurance, attaque, défense...)
 * 			  ainsi que son état (empoisonné, brulé...) qui peuvent affecter l'expérience de jeu.
 */

/*
 * Utilisé dans Attaquer , Health , ControllerJoueur
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Condition {
	Aveuglé,
	Empoisonné
	//.........
}

public class Caracteristiques : MonoBehaviour {
	//Caractéristiques maximales du personnage
	public bool estVivant; //Etat du joueur
	public float pointsDeVie = 50f; //Augmente les points de vie du personnage
	public float pointsEndurance = 10f; //Augmente le temps pendant lequel le personnage peut courir
	public float attaque = 1f;
	public float defense = 1f;
	public float saut = 3f;
	public float vitesseCourse = 9f;
	public float vitesseMarche = 5f;
	public float vitesseLancer = 1f;
	public List<Condition> conditionsActuelles;


	public int experience = 0; //L'expérience acquise par le joueur
	public int experience_avant_niveau_suivant = 0;
	public int niveau = 0; //Niveau du gameObject associé
	
	public ControllerJoueur cj; //Lié au ControllerJoueur pour en modifier les statistiques
	public Health h; //Lié à Health pour en modifier les statistiques


	void Start () {
//		conditionsActuelles = new List<Condition> ();
//		cj = gameObject.GetComponent<ControllerJoueur> ();
//		//Debug.Log ("ControllerJoueur de " + this + " est défini : " + (cj == true));
//		h = gameObject.GetComponent<Health> ();
//		//Debug.Log ("Health de " + this + " est défini : " + (h == true));
//		Actualiser ();


	}
	
	
	public void Actualiser () {
		if(cj) {
			cj.SetJaugeMax (pointsEndurance);
			cj.SetVitesseCourse (vitesseCourse);
			cj.SetVitesseMarche (vitesseMarche);
			cj.SetVitesseSaut (saut);
		}
		if (h) {
			h.SetPointsDeVieMax (pointsDeVie);
		}
		//Debug.Log ("Actualisation des statistiques effectuée");
	}

	
	//Set/Get
	public void SetEstVivant (bool estVivant_) {
		estVivant = estVivant_;
	}
	
	public bool GetEstVivant () {
		return estVivant;
	}
	
	public void SetPointsDeVie (float pointsDeVie_) {
		pointsDeVie = pointsDeVie_;
	}
	
	public float GetPointsDeVie () {
		return pointsDeVie;
	}
	
	public void SetPointsEndurance (float pointsEndurance_) {
		pointsEndurance = pointsEndurance_;
	}
	
	public float GetPointsEndurance () {
		return pointsEndurance;
	}
	
	public void SetAttaque (float attaque_) {
		attaque = attaque_;
	}
	
	public float GetAttaque () {
		return attaque;
	}
	
	public void SetDefense (float defense_) {
		defense = defense_;
	}
	
	public float GetDefense () {
		return defense;
	}
	
	public void SetSaut (float saut_) {
		saut = saut_;
	}
	
	public float GetSaut () {
		return saut;
	}
	
	public void SetVitesseCourse (float vitesseCourse_) {
		vitesseCourse = vitesseCourse_;
	}
	
	public float GetVitesseCourse () {
		return vitesseCourse;
	}
	
	public void SetVitesseMarche (float vitesseMarche_) {
		vitesseMarche = vitesseMarche_;
	}
	
	public float GetVitesseMarche () {
		return vitesseMarche;
	}
	
	public void SetVitesseLancer (float vitesseLancer_) {
		vitesseLancer = vitesseLancer_;
	}
	
	public float GetVitesseLancer () {
		return vitesseLancer;
	}
	
	public void SetConditionsActuelles (List<Condition> conditionsActuelles_) {
		conditionsActuelles = conditionsActuelles_;
	}
	
	public List<Condition> GetConditionsActuelles () {
		return conditionsActuelles;
	}
	
	public void SetCj (ControllerJoueur cj_) {
		cj = cj_;
	}
	
	public ControllerJoueur GetCj () {
		return cj;
	}
	
	public void SetH (Health h_) {
		h = h_;
	}
	
	public Health GetH () {
		return h;
	}

	public int GetNiveau ()
	{
		return niveau;
	}

	public float GetXp ()
	{
		return experience;
	}

	public float GetXpAvantNiveauSuivant ()
	{
		return experience_avant_niveau_suivant;
	}
	public virtual void AjouterExperience(int _expe){
	}


}

///几个问题 ：1. 加的那些和control joueur 相关的script， 是否应该改一下？  把它们变为joueurCarac？
/// 如果getcomponent<caracterisitiques> 的话，可以不可以get 一个joueurcarac？