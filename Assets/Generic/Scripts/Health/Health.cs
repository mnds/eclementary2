﻿/**
 * \file      Health.cs
 * \author    
 * \version   1.0
 * \date      29 novembre 2014
 * \brief     Contient les points de vie de l'objet. A sa mort, on appelle DeclencherMort.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Health : MonoBehaviour {
	public bool bypass = false; //A true quand on ne peut pas se prendre de dégats / etre régénéré
	public float pointsDeVieMax = 5f;
	public float pointsDeVieActuels = 5f;
	public int flagAssocie = -1; //-1 si pas de flag à la mort. Sinon, le changer pour activer un flag à la mort
	public List<int> flagsRequis = new List<int>(){};
	public List<int> flagsBloquants = new List<int>(){};
	protected bool mort = false;

	//Marche aussi pour le mana
	public float pointsDeManaMax = 5f;
	public float pointsDeManaActuels = 5f;


	void Start () {
		pointsDeVieActuels = Mathf.Min (pointsDeVieActuels, pointsDeVieMax);
		pointsDeManaActuels = Mathf.Min (pointsDeManaActuels,pointsDeManaMax);
	}

	/**
	 * @brief Permet de rajouter des points de vie à l'objet.
	 * @param soin De combien augmenter les points de vie.
	 *
	 * @return Renvoie true si un soin a été effectué.
	 */
	public bool Soigner(float soin) {
		if(bypass) return false;
		if(!TesterFlags()) return false;
		float pdvaAvantSoin = pointsDeVieActuels;
		pointsDeVieActuels = Mathf.Min (pointsDeVieActuels + soin, pointsDeVieMax);
		return (pdvaAvantSoin != pointsDeVieActuels);
	}
	
	/**
	 * @brief Permet de rajouter des points de mana à l'objet.
	 * @param mana De combien augmenter les points de mana.
	 *
	 * @return Renvoie true si un soin de mana a été effectué.
	 */
	public bool SoignerMana(float mana) {
		if(bypass) return false;
		if(!TesterFlags()) return false;
		float pdmaAvantSoin = pointsDeManaActuels;
		pointsDeManaActuels = Mathf.Min (pointsDeManaActuels + mana, pointsDeManaMax);
		return (pdmaAvantSoin != pointsDeManaActuels);
	}

	/**
	 * @brief Permet de faire subir des degats de mana à l'objet
	 * @param degats De combien diminuer les points de mana.
	 * @return true s'il y avait assez de points de mana.
	 * 
	 * @details Que pour le joueur en principe, donc on ne teste pas les flags ou le bypass
	 * 
	 */
	public bool SubirDegatsMana(float degats) {
		pointsDeManaActuels = Mathf.Min (pointsDeManaActuels, pointsDeManaMax); //Au cas où il y ait eu un problème
		if(degats>pointsDeManaActuels) {//Pas assez de mana !
			Debug.Log ("Pas assez de mana");
			return false;
		}
		pointsDeManaActuels -= degats;
		Debug.Log ("Points de Mana après le coup : " + pointsDeManaActuels);
		return true;
	}

	/**
	 * @brief Permet de faire subir des degats à l'objet, si l'attaquant n'a pas de script Caracteristiques.
	 * @param degats De combien diminuer les points de vie.
	 */
	public void SubirDegats(float degats,Caracteristiques caracAttaquant=null) {
		if(bypass) return;
		if(!TesterFlags ()) return;
		TesterFlags();
		Caracteristiques caracDefenseur = gameObject.GetComponent<Caracteristiques> (); //Attention : Health et Carcteristiques doivent se trouver sur le meme gameObject
		
		
		//Application de la formule de degats
		float degatsSubis = ControlCenter.FormuleDeDegats (degats,caracAttaquant,caracDefenseur); //Avant application des caractéristiques
		
		//Debug.Log ("Degats subis apres application de la statistique Defense : " + degatsSubis);
		
		pointsDeVieActuels = Mathf.Min (pointsDeVieActuels, pointsDeVieMax); //Au cas où il y ait eu un problème
		//Debug.Log ("Points de vie avant le coup : " + pointsDeVieActuels);
		pointsDeVieActuels -= degatsSubis;
		OnChangementPointsDeVie(); //On a changé les points de vie, on a peut-etre des choses à faire

//		Debug.Log ("Points de vie après le coup : " + pointsDeVieActuels);
		if (pointsDeVieActuels <= 0 && !mort ) {
			if(flagAssocie!=-1)
				FlagManager.ActiverFlag(flagAssocie);
			mort = true;
			DeclencherMort ();
		}
	}

	/**
	 * @brief Ne pas tenir en compte des scripts caracteristiques
	 * @param degats Degats initiaux
	 * @param nePasTenirEnCompteDesCarac Peu importe sa valeur, n'est là que pour surcharger la fonction
	 */
	public void SubirDegats(float degats,bool nePasTenirEnCompteDesCarac) {
		if(bypass) return;
		if(!TesterFlags ()) return;
		TesterFlags();
		Caracteristiques caracDefenseur = gameObject.GetComponent<Caracteristiques> (); //Attention : Health et Carcteristiques doivent se trouver sur le meme gameObject

		//Application de la formule de degats
		float degatsSubis = degats;
		
		//Debug.Log ("Degats subis apres application de la statistique Defense : " + degatsSubis);
		
		pointsDeVieActuels = Mathf.Min (pointsDeVieActuels, pointsDeVieMax); //Au cas où il y ait eu un problème
		//Debug.Log ("Points de vie avant le coup : " + pointsDeVieActuels);
		pointsDeVieActuels -= degatsSubis;
		OnChangementPointsDeVie(); //On a changé les points de vie, on a peut-etre des choses à faire

		Debug.Log ("Points de vie après le coup : " + pointsDeVieActuels);
		if (pointsDeVieActuels <= 0 && !mort ) {
			if(flagAssocie!=-1)
				FlagManager.ActiverFlag(flagAssocie);
			mort = true;
			DeclencherMort ();
		}
	}

	/**
	 * @brief Vérifie si les dégats peuvent etre infligés
	 */
	private bool TesterFlags() {
		foreach(int id in flagsRequis) //Verification flags requis
			if(!FlagManager.ChercherFlagParId(id).actif)
				return false;
		foreach(int id in flagsBloquants) //Verification flags bloquants
			if(FlagManager.ChercherFlagParId(id).actif)
				return false;
		return true;
	}

	/**
	 * @brief Permet de faire des choses quand les pdv changent
	 * @details Peut avoir son importance dans le cas d'ennemis qui changent de pattern en fonction de leurs points de vie
	 */
	virtual protected void OnChangementPointsDeVie () {
		Debug.Log ("Pas hérité");
	}

	virtual public void DeclencherMort () {

	}

	public void SetPointsDeVieMax (float pdvm_) {
		pointsDeVieMax = pdvm_;
		pointsDeVieActuels = Mathf.Min (pointsDeVieMax, pointsDeVieActuels);
	}
	
	public float GetPointsDeVieMax () {
		return pointsDeVieMax;
	}
	
	public void SetPointsDeManaMax (float pdmm_) {
		pointsDeManaMax = pdmm_;
		pointsDeManaActuels = Mathf.Min (pointsDeManaMax, pointsDeManaActuels);
	}
	
	public float GetPointsDeManaMax () {
		return pointsDeManaMax;
	}

	public void SetBypass (bool bypass_) {
		bypass=bypass_;
	}

	public bool GetBypass () {
		return bypass;
	}

	public bool IsMort() {
		return mort;
	}

	public void SetMort( bool etat ) {
		mort = etat;
	}

	public void SetPointsDeVie( float pointsDeVie ) {
		pointsDeVieActuels = pointsDeVie;
	}

	public float GetPointsDeVieActuels() {

		return pointsDeVieActuels;
	}

	public float GetPointsDeManaActuels() {
		
		return pointsDeManaActuels;
	}
}
