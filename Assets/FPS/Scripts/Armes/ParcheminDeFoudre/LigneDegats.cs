/**
 * \file      LigneDegats.cs
 * \author    TM	
 * \version   1.0
 * \date      05 mars 2015
 * \brief     Permet d'infliger des dégats aux gens traversant le collider
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LigneDegats : MonoBehaviour {

	public float degatsEclair = 10f; // dégats subis
//	public GameObject eclairLine; // si vous résolvez le problème du rendu des éclairs en coordonnées locales
//	private float cooldown = 0.2f;
//	private float tempsDernierEclair = 0f;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
//	void Update () {
//		if (Time.time > tempsDernierEclair + cooldown) {
//			Instantiate(eclairLine,gameObject.transform.position,Quaternion.identity);
//			eclairLine.transform.parent = gameObject.transform;
//		}
//
//
//		}


	void OnTriggerEnter(Collider collider)
	{	//enlever de la vie pour chaque personne traversant le collider (MEME LE JOUEUR)
		Transform c=collider.gameObject.transform;
		Health health = c.GetComponent<Health>(); //Si le truc touché a des points de vie, on doit le blesser

		while(health == null && c.transform.parent){
			c=c.transform.parent;
			health = c.GetComponent<Health>();
		}
		
		float degatsInfliges=degatsEclair; //Initialement égal à la valeur "de base"
		if(health != null){
			health.SubirDegats(degatsEclair);
		}
	}
}
