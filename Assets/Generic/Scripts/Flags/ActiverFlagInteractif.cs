/**
 * \file      ActiverFlagInteractif.cs
 * \author    
 * \version   1.0
 * \date      11 décembre 2014
 * \brief     Active un flag après avoir interagit avec le gameObject auquel le script est attaché. Les actions à faire en meme temps que l'activation du flag sont faites dans ce meme script à travers une méthode virtuelle.
 * 
 * \details	  Hérite de Interactif.
 */

using UnityEngine;
using System.Collections;

public class ActiverFlagInteractif : MonoBehaviour, Interactif {
	public float distanceMinimaleInteraction = 4.0f; //La distance à laquelle on doit etre pour pouvoir interagir avec l'objet
	public int flagActive = 0;
	public bool activable = true; //Si à true, on essaiera d'activer le flag associé. Si le flag passe à true, on déclenche les événements. Cette variable passe à false après l'activation du flag pour éviter de l'activer plein de fois. En pratique, on laisse à true au départ.
	public bool detruireApresActivation = true; //Le script s'autodétruit une fois le flag activé.
	private Animation animationActivation; //Si on veut lancer une animation
	public string texteSiActivable; //Pour afficher un texte lorsqu'on interagit et que ça s'active
	public string texteSiNonActivable; //Pour afficher un texte lorsqu'on interagit et que ça s'active pas

	public void DemarrerInteraction() {
		if(activable) {
			//Si le flag est activé, plus besoin de pouvoir l'activer. On désactive le script.
			activable = !FlagManager.ActiverFlag(flagActive);
			if(!activable) {
				StartCoroutine(AfficherTexte (texteSiActivable));
				FaireActionsConnexes();
				animationActivation=GetComponent<Animation>();
				if(animationActivation) {
					animationActivation.Play();
					Debug.Log ("ANIMATION");
				}
			}
			else
				StartCoroutine(AfficherTexte (texteSiNonActivable)); //Il manque un flag.
		}
	}

	//Destinée à etre override
	public virtual void FaireActionsConnexes() {
	
	}

	public void ArreterInteraction() {

	}

	public void SetDistanceMinimaleInteraction (float distanceMinimaleInteraction_)
	{
		distanceMinimaleInteraction = distanceMinimaleInteraction_;
	}

	public float GetDistanceMinimaleInteraction ()
	{
		return distanceMinimaleInteraction;
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
