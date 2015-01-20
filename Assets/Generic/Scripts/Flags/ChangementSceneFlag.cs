/**
 * \file      ChangementSceneFlag.cs
 * \author    
 * \version   1.0
 * \date      25 décembre 2014
 * \brief     Script permettant à un GameObject d'activer un flag à l'aide du script static FlagManager, puis
 * 			  de changer de scène.
 * 
 * \details	  Similaire à ActivationFlag
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChangementSceneFlag : MonoBehaviour {
	[System.Serializable]
	public class FlagsRequisInterditsChangementSceneFlag { //Pour les déclarations publiques. Uniquement pour empecher l'inspecteur de Unity de faire n'importe quoi.
		public string nomScene; //Nom de la scène à laquelle accéder
		public List<int> flagsNecessairesPourTransition;
		public List<int> flagsInterditsPourTransition;
		public string nomDuSpawnPoint; //Nom du spawnPoint sur lequel on doit se téléporter
	}
	
	public List<int> listeDesFlagsPouvantEtreActives; //Liste de tous les flags qu'on doit tenter d'activer avant de changer de scène
	public List<bool> activationPossibleDesFlags; //Pour éviter d'activer tous les flags plein de fois
	public List<FlagsRequisInterditsChangementSceneFlag> nomsDesScenesAccessibles; //Noms de toutes les scènes auxquelles on peut accéder.
	
	/**
	* @brief Active les flags puis change de scène.
	* 
	* @details Commence par vérifier si l'activation du flag est lié à des paramètres extérieurs (inventaire...)
    */
	public void ActivationFlagActive () {
		//Activation des flags
		for(int i=0;i<listeDesFlagsPouvantEtreActives.Count;i++) {
			int idFlag = listeDesFlagsPouvantEtreActives[i];
			TestsActivation(idFlag); //Tests concernant l'inventaire / les caractéristiques du joueur... qui peuvent changer l'état de activationPossibleDesFlags[idFlag] et le mettre à false.
			if(activationPossibleDesFlags[i]) {//Si on a le droit de l'activer
				//Si le flag est activé, plus besoin de pouvoir l'activer. On désactive le script.
				activationPossibleDesFlags[i] = !FlagManager.ActiverFlag(idFlag);
				if(!activationPossibleDesFlags[i]) {
					FaireActionsConnexes(idFlag); //Tout ce qui concerne uniquement cette interaction (appel à des événements...)
				}
			}
		}
		foreach(int idFlag in listeDesFlagsPouvantEtreActives) { //On fait tous les flags

		}

		//Changement de scène
		foreach(FlagsRequisInterditsChangementSceneFlag fricsf in nomsDesScenesAccessibles) {
			bool changerDeScene = FlagManager.VerifierFlags(fricsf.flagsNecessairesPourTransition,fricsf.flagsInterditsPourTransition); //Peut changer pendant les tests
			if(changerDeScene) { //Tous les flags sont bons, on change de scène.
				ControlCenter.SetNomSpawnPointActuel(fricsf.nomDuSpawnPoint);
				ChargerNiveau(fricsf.nomScene);
			}
		}
	}

	//Destinée à etre override
	public virtual void TestsActivation(int idFlag_) {
		
	}
	
	//Destinée à etre override, actions à faire selon l'id. On met un switch dans la méthode pour savoir qui fait quoi
	public virtual void FaireActionsConnexes(int idFlag_) {
		
	}

	private void ChargerNiveau(string nomScene_) {
		Debug.Log ("Chargement d'un niveau : "+nomScene_);
		StateManager.getInstance().ChargerEtatSelonScene(nomScene_);
	}
}
