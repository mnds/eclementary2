/**
 * \file      ActivationSelonFlags.cs
 * \author    
 * \version   1.0
 * \date      19 janvier 2015
 * \brief     Attaché aux objets dont l'activation dépend de l'état des flags
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivationSelonFlags : ActionSelonFlagsScene {
	public List<int> flagsDevantEtreActives;
	public List<int> flagsDevantEtreDesactives;

	void Start () {
		ControlCenter.AddObjetActivableSelonFlags(this,Application.loadedLevelName);
		Verifier();
	}

	/**
	 * @brief Verifie que les flags des deux listes en attribut sont bien aux bons états
     */
	override public void Verifier () {
		foreach(int idFlagA in flagsDevantEtreActives) {
			if(!FlagManager.ChercherFlagParId(idFlagA).actif) { //Pas activé, problème
				gameObject.SetActive(false); //On désactive le gO
				return; //ca ne sert plus à rien
			}
		}
		foreach(int idFlagA in flagsDevantEtreDesactives) {
			if(FlagManager.ChercherFlagParId(idFlagA).actif) { //Pas activé, problème
				gameObject.SetActive(false); //On désactive le gO
				return; //ca ne sert plus à rien
			}
		}
		//Tout est bon
		gameObject.SetActive(true);
	}
}
