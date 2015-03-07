/**
 * \file      DegatsEcrasementBossFinal.cs
 * \author    
 * \version   1.0
 * \date      7 mars 2015
 * \brief     Script contenant les dégats d'écrasement faits par le boss final lors de la deuxième phase.
 * 
 * \details	  Attaché à deux capsules cachées sur les jambes du modèle.
 */

using UnityEngine;
using System.Collections;

public class DegatsEcrasementBossFinal : MonoBehaviour {
	private bool estActif = false; //Ne s'active que par le script ActivationBossFinal
	public Health healthBossFinal; //Savoir qui est le script Health du boss final pour ne pas qu'il s'inflige des dégats.
	public float degats = 10000f; //ca tue

	//Si on touche quelque chose
	void OnTriggerEnter (Collider c) {
		Health h = c.gameObject.GetComponent<Health>();
		if(h!=healthBossFinal) { //Si ce n'est pas le boss
			h.SubirDegats(degats); //Pas soumis aux caractéristiques
		}
	}

	void OnTriggerExit (Collider c) {
		Health h = c.gameObject.GetComponent<Health>();
		if(h!=healthBossFinal) { //Si ce n'est pas le boss
			h.SubirDegats(degats); //Pas soumis aux caractéristiques
		}
	}
}
