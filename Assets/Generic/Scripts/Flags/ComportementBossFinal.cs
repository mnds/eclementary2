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
public class ComportementBossFinal : ActionSelonFlagsScene {
	private Animator anim; //L'animator attaché au boss pour les différentes transitions
	private GameObject cible; //Le joueur

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

	//Phase 2 : le boss marche sur le terrain
	private bool phaseDeux = true;
	public DegatsEcrasementBossFinal debfGauche; //Zones d'écrasement des pieds
	public DegatsEcrasementBossFinal debfDroite;
	private float vitesseRotation = 2f; //Vitesse à laquelle le boss se tourne

	//Phase 3 : le boss envoie des boules de feu
	private bool phaseTrois = false;
	public GameObject bouleDeFeu; //Prefab de la boule
	public float cooldown = 1f;
	private float tempsDerniereBoule;
	public Transform positionDepartBoule; //Placé au niveau de la main

	private Health h; //Pour savoir quand sont les différentes phases

	void Start () {
		ControlCenter.AddObjetActivableSelonFlags(this,Application.loadedLevelName);
		h=gameObject.GetComponent<Health>();
		anim=gameObject.GetComponent<Animator>();
		cible = ControlCenter.GetJoueurPrincipal();

		//Désactivation des debf
		debfGauche.SetEstActif (false);
		debfDroite.SetEstActif (false);

		//Verification des flags
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
		else if(phaseDeux) {
			//On fait se tourner vers le joueur le boss
			Vector3 directionVoulue = cible.transform.position - transform.position;
			//La boule est mal orientée, son forward est selon x.
			
			float angleEntreDirections = Vector3.Angle (cible.transform.position, transform.position);
			Quaternion rotationVoulue = Quaternion.LookRotation(new Vector3(directionVoulue.x,0,directionVoulue.z));
			transform.rotation=Quaternion.Lerp(transform.rotation,rotationVoulue,Time.deltaTime*vitesseRotation/angleEntreDirections);

			//On teste si la phase est finie
			if(h.GetPointsDeVieActuels()/h.GetPointsDeVieMax()<0.9999f) { //5% de la vie enlevée
				FinirPhaseDeux();
			}
		}
		else if(phaseTrois) {
			//On fait se tourner vers le joueur le boss
			Vector3 directionVoulue = cible.transform.position - transform.position;
			//La boule est mal orientée, son forward est selon x.
			
			float angleEntreDirections = Vector3.Angle (cible.transform.position, transform.position);
			Quaternion rotationVoulue = Quaternion.LookRotation(new Vector3(directionVoulue.x,0,directionVoulue.z));
			transform.rotation=Quaternion.Lerp(transform.rotation,rotationVoulue,Time.deltaTime*vitesseRotation/angleEntreDirections);

			//On envoie une boule de feu sur le joueur
			if(Time.time>tempsDerniereBoule+cooldown) {
				tempsDerniereBoule=Time.time;
				GameObject boule = Instantiate(bouleDeFeu,new Vector3(0f,0f,0f),Quaternion.identity) as GameObject; //Instantie loin
				Physics.IgnoreCollision(boule.collider,gameObject.collider); //Pour ne pas que la boule tue le boss
				boule.transform.position=positionDepartBoule.transform.position;
				boule.transform.rotation=positionDepartBoule.transform.rotation;
				//On ajoute une force
				boule.rigidbody.AddForce(directionVoulue*10f);
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
		CommencerPhaseDeux();
	}

	/**
	 * @brief Rassemble les opérations à faire au début de la phase 2.
     */
	private void CommencerPhaseDeux () {
		Debug.Log ("Début phase 2");
		phaseDeux = true;
		anim.SetBool("Walk",true); //Faire avancer le boss
		//Le boss marche, il écrase
		debfGauche.SetEstActif(true);
		debfDroite.SetEstActif(true);
	}
	
	/**
	 * @brief Rassemble les opérations à faire à la fin de la phase 2.
     */
	private void FinirPhaseDeux () {
		Debug.Log ("Fin phase3");
		phaseDeux = false;
		//phaseTrois=true; //Passage phase 3
	}
	

	virtual protected void ActionSiMauvaisFlag() {}
	virtual protected void ActionSiBonFlag() {}
}
