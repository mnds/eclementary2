/**
 * \file      ComportementBossFinal2.cs
 * \author    
 * \version   1.0
 * \date      22 mars 2015
 * \brief     Comportement du deuxième boss final.
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//ActivationSelonFlags pour que le boss ne s'active que lorsque le flag de début de combat est lancé.
public class ComportementBossFinal2 : ActionSelonFlagsScene {
	private GameObject cible; //Le joueur

	//Flag
	public int idFlagDebut; //Flag qui doit etre activé
	private bool estActive = false; //False au début

	//Le boss envoie des boules de feu
	private bool phaseTrois = false;
	public GameObject bouleDeFeu; //Prefab de la boule
	public float cooldown = 2f;
	private float tempsDerniereBoule;
	public Transform positionDepartBoule; //Placé au niveau de la main
	private float vitesseRotation = 10f; //Vitesse à laquelle le boss se tourne
	public GameObject bossFinal1; //Pour ne pas que les boules le tapent

	private Health h; //Pour savoir quand sont les différentes phases
	private Health healthBossFinal1; //Celle du boss

	void Start () {
		ControlCenter.AddObjetActivableSelonFlags(this,Application.loadedLevelName);
		h=gameObject.GetComponent<Health>();
		healthBossFinal1=bossFinal1.GetComponent<Health>();
		cible = ControlCenter.GetJoueurPrincipal();
	}

	void Update () {
		//Seulement si le boss est activé
		if(!estActive)
			return;

		float temps = Time.time;

		//On fait se tourner vers le joueur le boss
		Vector3 directionVoulue = cible.transform.position - transform.position;
		float distanceAvecCible = Vector3.Distance(cible.transform.position,transform.position);
		//La boule est mal orientée, son forward est selon x.

		float angleEntreDirections = Vector3.Angle (cible.transform.position, transform.position);
		Quaternion rotationVoulue = Quaternion.LookRotation(new Vector3(directionVoulue.x,0,directionVoulue.z));
		transform.rotation=Quaternion.Lerp(transform.rotation,rotationVoulue,Time.deltaTime*vitesseRotation/angleEntreDirections);

		//On envoie une boule de feu sur le joueur
		if(Time.time>tempsDerniereBoule+cooldown) {
			tempsDerniereBoule=Time.time;
			GameObject boule = Instantiate(bouleDeFeu,new Vector3(0f,0f,0f),Quaternion.identity) as GameObject; //Instantie loin

			//On dit à la boule d'ignorer les deux boss
			BouleDeFeuBossFinal bdfbf = boule.GetComponent<BouleDeFeuBossFinal>();
			bdfbf.healthBossFinal1=healthBossFinal1;
			bdfbf.healthBossFinal2=h;

			Physics.IgnoreCollision(boule.collider,gameObject.collider); //Pour ne pas que la boule tue le boss
			Physics.IgnoreCollision(boule.collider,bossFinal1.collider); //Pour ne pas que la boule tue le boss
			boule.transform.position=positionDepartBoule.transform.position;
			boule.transform.rotation=positionDepartBoule.transform.rotation;
			//On ajoute une force
			boule.rigidbody.AddForce((directionVoulue+
			                          new Vector3(Random.Range(-0.5f,0.5f),Random.Range(-0.5f,0.5f),Random.Range(-0.5f,0.5f))
			            			)*distanceAvecCible*5f);
		}
	}

	/**
	 * @brief Verifie que les flags des deux listes en attribut sont bien aux bons états
     */
	override public void Verifier () {

	}

	/**
	 * @brief Rassemble les opérations à faire pour activer le boss.
     */
	public void Activer () {
		h.bypass=false;
		estActive=true;
	}

	virtual protected void ActionSiMauvaisFlag() {}
	virtual protected void ActionSiBonFlag() {}
}
