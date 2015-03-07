/**
 * \file      ActivationBossFinal.cs
 * \author    
 * \version   1.0
 * \date      7 mars 2015
 * \brief     Attaché au boss final.
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//ActivationSelonFlags pour que le boss ne s'active que lorsque le flag de début de combat est lancé.
public class ActivationBossFinal : ActionSelonFlagsScene {
	//Flag
	public int idFlagDebut; //Flag qui doit etre activé
	private bool estActive = false; //False au début

	//Phase 1 : le boss devient énorme
	private bool phaseUn = false; //Phase un : le boss devient énorme
	public float tempsPhaseUn = 5f; //Temps de la phase
	private float tempsDebutPhaseUn; //Pour les calculs rapides de ratio
	private float tempsFinPhaseUn; //Pour savoir quand finir
	private Vector3 tailleInitialePhaseUn; //Pour faire un ratio
	public float ratio = 3f; //De combien ça doit grossir

	private Health h; //Pour savoir quand sont les différentes phases

	void Start () {
		ControlCenter.AddObjetActivableSelonFlags(this,Application.loadedLevelName);
		h=gameObject.GetComponent<Health>();
		Verifier();
	}

	void Update () {
		//Seulement si le boss est activé
		if(!estActive)
			return;

		float temps = Time.time;
		if(phaseUn) { //Pas de conditions sur temps ; le test est fait dans la boucle.
			//On cherche le ratio entre 0 et 1
			float ratioTemps = Mathf.Min(1f,(temps-tempsDebutPhaseUn)/tempsFinPhaseUn);
			//On change de manière sinusoidale
			gameObject.transform.localScale = Vector3.Slerp(tailleInitialePhaseUn,tailleInitialePhaseUn*ratio,ratioTemps);
			//On teste si on a fini
			if(temps>tempsFinPhaseUn) { //On a fini
				FinirPhaseUn();
			}
		}
	}

	/**
	 * @brief Verifie que les flags des deux listes en attribut sont bien aux bons états
     */
	override public void Verifier () {
		Debug.Log ("Verifier ABF");
		if(FlagManager.ChercherFlagParId(idFlagDebut).actif) {
			Debug.Log ("Flag actif");
			estActive = true; //On active
			CommencerPhaseUn();
		}
	}

	/**
	 * @brief Rassemble les opérations à faire au début de la phase un.
     */
	private void CommencerPhaseUn () {
		Debug.Log ("COMMENCER PHASE 1");
		h.bypass=true; //On bloque health
		tailleInitialePhaseUn = gameObject.transform.localScale; //On garde en mémoire
		tempsDebutPhaseUn = Time.time; //On initialise
		tempsFinPhaseUn = tempsDebutPhaseUn+tempsPhaseUn;
		phaseUn = true; //On finit par ça pour ne pas lancer l'animation trop tot.
	}

	/**
	 * @brief Rassemble les opérations à faire à la fin de la phase un.
     */
	private void FinirPhaseUn () {
		phaseUn = false; //On commence par ça pour ne pas continuer les calculs.
		h.bypass=false; //On peut enfin taper
		Debug.Log ("Fin phase 1");
	}

	virtual protected void ActionSiMauvaisFlag() {}
	virtual protected void ActionSiBonFlag() {}
}
