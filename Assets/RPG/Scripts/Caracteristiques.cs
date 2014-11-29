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


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Condition {
	Aveuglé,
	Empoisonné
	//.........
}

public class Caracteristiques : MonoBehaviour {
	private bool estVivant; //Etat du joueur
	private float pointsDeVie; //Augmente les points de vie du personnage
	private float pointsEndurance; //Augmente le temps pendant lequel le personnage peut courir
	private float attaque;
	private float defense;
	private float saut;
	private float vitesseCourse;
	private float vitesseMarche;
	private float vitesseLancer;
	private List<Condition> conditionsActuelles;


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

}
