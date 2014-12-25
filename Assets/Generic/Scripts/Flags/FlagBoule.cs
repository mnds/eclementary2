/**
 * \file      FlagBoule.cs
 * \author    
 * \version   1.0
 * \date      12 décembre 2014
 * \brief     Déclenche la boule une fois son flag activé.
 * 
 * \details	  Hérite de ActiverFlagInteractif.
 */
using UnityEngine;
using System.Collections;

public class FlagBoule : ActiverFlagInteractif {
	GameObject cible; //La chose à toucher
	public float vitesseRotation = 2.0f; //Vitesse à laquelle la tourelle tourne. 0.5f est amplement suffisant pour garantir le suivi d'un perso qui marche.
	public float distanceMini = 30.0f;

	public bool estActiveeInitial = false;
	public bool estActivee;

	void Start () {
		cible = ControlCenter.GetJoueurPrincipal ();
		estActivee = estActiveeInitial;
	}

	public override void FaireActionsConnexes() {
		GetComponentInChildren<Health>().SetBypass(false);

		//Activer la boule
		estActivee = true;
		CapsuleCollider cc = GetComponent<CapsuleCollider> ();
		cc.center = new Vector3 (-0.26f, 0.01f, -0.01f);
		cc.radius = 0.41f;
		cc.height = 1.75f;
		cc.direction = 0; //Selon le x-axis

		//Activation des tourelles
		Tourelle[] tourelles = GetComponentsInChildren<Tourelle> ();
		foreach (Tourelle t in tourelles) {
			t.SetActive(true);
		}
	}

	void Update () {
		if(!estActivee) return; //Si la tourelle n'est pas activée elle ne fait rien.

		Vector3 directionVoulue = cible.transform.position+new Vector3(0f,0.75f) - transform.position;
		//La boule est mal orientée, son forward est selon x.
		directionVoulue = new Vector3 (-directionVoulue.z, directionVoulue.y, directionVoulue.x);

		float distanceCible = Vector3.Magnitude (directionVoulue);
		float angleEntreDirections = Vector3.Angle (cible.transform.position, transform.position);
		if (distanceCible < distanceMini) {
			Quaternion rotationVoulue = Quaternion.LookRotation(new Vector3(directionVoulue.x,0,directionVoulue.z));
			transform.rotation=Quaternion.Lerp(transform.rotation,rotationVoulue,Time.deltaTime*vitesseRotation/angleEntreDirections);
		}
		else {
			transform.Rotate(0,Time.deltaTime*vitesseRotation,0);
		}
	}
}
