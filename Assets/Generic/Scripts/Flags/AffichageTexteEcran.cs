/**
 * \file      AffichageTexteEcran.cs
 * \author    
 * \version   1.0
 * \date      1 janvier 2015
 * \brief     Script permettant d'écrire un texte à l'écran en se servant d'un script ControlCenter.
 * 
 * \details	  Permet de démarrer une coroutine qui à l'aide de variables dans ControlCenter et d'un prefab Texte d'afficher du texte à l'écran.
 * 		      Attention : ce script ne doit pas etre detruit si son texte est permanent et pas remis à temporaire dans ControlCenter !
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AffichageTexteEcran : MonoBehaviour {
	public List<int> flagsDevantEtreActives; //Liste des flags devant etre activés pour que l'affichage se fasse
	public List<int> flagsDevantEtreDesactives; //Liste des flags devant etre desactivés pour que l'affichage se fasse

	/**
	 * @brief Remplace le texte à l'écran par un texte temporaire, donc prioritaire.
	 * @param texte Texte à mettre.
	 */
	public IEnumerator AfficherTextePrioritaire(string texte) {
		if(!FlagManager.VerifierFlags(flagsDevantEtreActives,flagsDevantEtreDesactives)) //Pas les bons flags, on ne fait rien
			yield return new WaitForSeconds(0.1f);
		else
			{
				//Texte prioritaire, on change peu importe ce qui se passe.
				ControlCenter.GetTexte ().text = texte;
				ControlCenter.SetTexteEstPrioritaire (false); //Pas le droit de le changer par un texte non prioritaire !
				yield return new WaitForSeconds(ControlCenter.tempsAffichageTexteInteraction);
				Debug.Log ("Fin affichage texte");
				if(ControlCenter.GetTexte ().text==texte) {//Si le texte n'a pas changé, on met un texte vide. Sinon on ne fait rien, le texte a changé !
					ControlCenter.GetTexte ().text="";
					ControlCenter.SetTexteEstPrioritaire (true); //Donc on peut le changer
				}
				ActionsApresAffichage(); //Si le yield
			}
	}

	public void RemplacerTexteNonPrioritaire (string texte) {
		//Pas les bons flags ou un texte prioritaire est affiché, on ne fait rien
		if(ControlCenter.GetTexteEstPrioritaire () || !FlagManager.VerifierFlags(flagsDevantEtreActives,flagsDevantEtreDesactives)) {
			//Debug.Log ("Texte prioritaire");
			return; //Texte prioritaire.
		}
		ControlCenter.GetTexte ().text = texte;
		if(!ControlCenter.GetTexte ())
			Debug.LogWarning ("PROBLEME");
		ControlCenter.SetTexteEstPrioritaire (false); //On reprécise au cas où
	}

	public virtual void ActionsApresAffichage() {
	
	}
}
