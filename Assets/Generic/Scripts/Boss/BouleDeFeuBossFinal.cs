/**
 * \file      BouleDeFeuProjectile.cs
 * \author    TM	
 * \version   1.0
 * \date      24 février 2015
 * \brief     Permet de faire exploser les projectiles bouledefeu en blessant et éjectant objets et ennemis
 */





using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BouleDeFeuBossFinal : MonoBehaviour {
	
	float rayonExplosion = 2f; //zone d'effet de l'explosion
	float forceExplosion = 100f; //force de l'explosion
	float degatsExplosion = 10f; //dégâts de l'explosion
	public GameObject lensFlare; //pour faire joli
	public GameObject particules; //là aussi

	public Health healthBossFinal1; //Pas de friendly fire
	public Health healthBossFinal2; //Pas de friendly fire


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	// Lorsque la boule de feu touche le sol ou un autre collider, elle explose
	void OnCollisionEnter (Collision col)
	{   

		ContactPoint contact = col.contacts[0]; //point de contact
		Vector3 pos = contact.point; //on convertit en coordonnées

		GameObject cloneLensFlare;
		cloneLensFlare = Instantiate (lensFlare, pos, Quaternion.identity) as GameObject;  //on fait apparaitre un bref lens flare
		GameObject cloneParticules;
		cloneParticules = Instantiate (particules, pos, Quaternion.identity) as GameObject; //on fait apparaitre une explosion de particules


		Collider[] hitColliders = Physics.OverlapSphere(pos, rayonExplosion); //on prend tous ceux touchés par la boule
		foreach (Collider collid in hitColliders){

			//enlever de la vie pour chaque personne prise dans l'explosion (MEME LE JOUEUR)
			Transform c=collid.gameObject.transform;
			Health health = c.GetComponent<Health>(); //Si le truc touché a des points de vie, on doit le blesser
			while(health == null && c.transform.parent){
				c=c.transform.parent;
				health = c.GetComponent<Health>();
			}

			float degatsInfliges=degatsExplosion; //Initialement égal à la valeur "de base"
			if(health != null && health != healthBossFinal1 && health != healthBossFinal2){
				health.SubirDegats(degatsInfliges);
				Debug.Log ("Touché : "+health.gameObject);
			}

			if (collid && collid.rigidbody){ // si on touche un rigid body, on le repousse
				collid.rigidbody.AddExplosionForce(forceExplosion,pos,rayonExplosion,30f);
			}
		}
		Destroy (gameObject); // à la fin, la boule de feu explose !
		Destroy (cloneLensFlare, 0.15f);
		Destroy (cloneParticules, 6f);
	}
	
}