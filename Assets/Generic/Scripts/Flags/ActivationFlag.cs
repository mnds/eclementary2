/**
 * \file      ActivationFlag.cs
 * \author    
 * \version   1.0
 * \date      25 décembre 2014
 * \brief     Script permettant à un GameObject d'activer un flag à l'aide du script static FlagManager.
 * 
 * \details	  Hérite de Interactif.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivationFlag : AffichageTexteEcran {
	public int flagActive = 0;
	public List<int> flagRequisPourDestruction = new List<int>(){}; //Liste des flags qui doivent etre activés pour que ça marche. Indépendant de FlagManager.
	public List<int> flagBloquantsPourDestruction = new List<int>(){}; //Liste des flags qui ne doivent etre activés pour que ça marche. Indépendant de FlagManager.
	public bool activable = true; //Si à true, on essaiera d'activer le flag associé. Si le flag passe à true, on déclenche les événements. Cette variable passe à false après l'activation du flag pour éviter de l'activer plein de fois. En pratique, on laisse à true au départ.
	public bool detruireApresActivation = true; //Le script s'autodétruit une fois le flag activé.
	private Animation animationActivation; //Si on veut lancer une animation
	public string texteSiActivable; //Pour afficher un texte lorsqu'on interagit et que ça s'active
	public string texteSiNonActivable; //Pour afficher un texte lorsqu'on interagit et que ça s'active pas

	/**
	 * @brief Méthode appelée pour activer le flag.
	 * 
	 * @details Commence par vérifier si l'activation du flag est lié à des paramètres extérieurs (inventaire...), puis regarde
	 * 			au niveau du
	 */
	public void ActivationFlagActive () {
		TestsActivation(); //Tests concernant l'inventaire / les caractéristiques du joueur... qui peuvent changer l'état de activable et le mettre à false.
		if(activable) {
			//Si le flag est activé, plus besoin de pouvoir l'activer. On désactive le script.
			activable = !FlagManager.ActiverFlag(flagActive);
			if(!activable) {
				StartCoroutine(AfficherTextePrioritaire (texteSiActivable));
				FaireActionsConnexes(); //Tout ce qui concerne uniquement cette interaction (appel à des événements...)
				animationActivation=GetComponent<Animation>();
				if(animationActivation) {
					animationActivation.Play();
					Debug.Log ("ANIMATION");
				}
			}
			else
				StartCoroutine(AfficherTextePrioritaire (texteSiNonActivable)); //Il manque un flag.
		}
		else
			StartCoroutine(AfficherTextePrioritaire (texteSiNonActivable)); //Il manque une condition extérieure.
	}

	//Destinée à etre override
	public virtual void TestsActivation() {
	
	}

	//Destinée à etre override
	public virtual void FaireActionsConnexes() {
	
	}

	public void ArreterInteraction() {

	}

	public override void ActionsApresAffichage() {
		//On détruit maintenant qu'on a mis les textes comme il faut
		if(!activable) {
			if(detruireApresActivation) {
				foreach(int id in flagRequisPourDestruction) {//Si l'un est inactif on ne peut pas activer ce Flag
					Debug.Log ("Etude pour la suppression de l'activationFlag du flag "+flagActive+" du predecesseur d'id "+id);
					if(!FlagManager.ChercherFlagParId (id).actif) {
						Debug.Log("Flag id : "+id+" pas activable");
						return;
					}
				}
				
				foreach(int id in flagBloquantsPourDestruction) {//Si l'un est inactif on ne peut pas activer ce Flag
					Debug.Log ("Etude pour la suppression de l'activationFlag du flag "+flagActive+" du bloquant d'id "+id);
					if(FlagManager.ChercherFlagParId (id).actif) {
						Debug.Log("Flag id : "+id+" pas activable");
						return;
					}
				}
				Destroy (this);
			}
		}
	}
}
