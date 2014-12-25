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

public class ActivationFlag : MonoBehaviour {
	public int flagActive = 0;
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
				StartCoroutine(AfficherTexte (texteSiActivable));
				FaireActionsConnexes(); //Tout ce qui concerne uniquement cette interaction (appel à des événements...)
				animationActivation=GetComponent<Animation>();
				if(animationActivation) {
					animationActivation.Play();
					Debug.Log ("ANIMATION");
				}
			}
			else
				StartCoroutine(AfficherTexte (texteSiNonActivable)); //Il manque un flag.
		}
		else
			StartCoroutine(AfficherTexte (texteSiNonActivable)); //Il manque une condition extérieure.
	}

	//Destinée à etre override
	public virtual void TestsActivation() {
	
	}

	//Destinée à etre override
	public virtual void FaireActionsConnexes() {
	
	}

	public void ArreterInteraction() {

	}

	public IEnumerator AfficherTexte(string texte) {

		ControlCenter.GetTexte ().text = texte;
		yield return new WaitForSeconds(ControlCenter.tempsAffichageTexteInteraction);
		Debug.Log ("Fin texte");
		if(ControlCenter.GetTexte ().text==texte) //Si le texte n'a pas changé, on met un texte vide. Sinon on ne fait rien, le texte a changé !
			ControlCenter.GetTexte ().text="";

		//On détruit maintenant qu'on a mis les textes comme il faut
		if(!activable)
			if(detruireApresActivation)
				Destroy (this);
	}
}
