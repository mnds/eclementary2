/**
 * \file      ActionSelonFlags.cs
 * \author    
 * \version   1.0
 * \date      2 mars 2015
 * \brief     Attaché aux objets ne disparaissant pas dont une action dépend de l'état des flags
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionSelonFlags : MonoBehaviour {
	public List<int> flagsDevantEtreActives;
	public List<int> flagsDevantEtreDesactives;
	
	void Start () {
		ControlCenter.AddActionSelonFlags(this);
		Verifier();
	}

	//Mis tout le temps pour les rechargements de partie
	void OnLevelWasLoaded () {
		Verifier ();
	}

	/**
	 * @brief Verifie que les flags des deux listes en attribut sont bien aux bons états
     */
	public void Verifier () {
		foreach(int idFlagA in flagsDevantEtreActives) {
			if(!FlagManager.ChercherFlagParId(idFlagA).actif) { //Pas activé, problème
				ActionSiMauvaisFlag();
				return; //ca ne sert plus à rien
			}
		}
		foreach(int idFlagA in flagsDevantEtreDesactives) {
			if(FlagManager.ChercherFlagParId(idFlagA).actif) { //Pas activé, problème
				ActionSiMauvaisFlag();
				return; //ca ne sert plus à rien
			}
		}
		//Tout est bon
		ActionSiBonFlag();
	}

	virtual protected void ActionSiMauvaisFlag() {}
	virtual protected void ActionSiBonFlag() {}
}
