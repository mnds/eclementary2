/**
 * \file      ContactArmeScript.cs
 * \author    
 * \version   1.0
 * \date      6 décembre 2014
 * \brief     Ajoute les scripts nécessaires aux armes placées sous MainCamera. A utiliser avec l'éditeur prévu.
 */

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Attaquer))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Lancer))]
[RequireComponent(typeof(Collider))]
public class ContactArmeScript : MonoBehaviour {
	// Use this for initialization
	void Awake () {
		Initialiser ();
	}

	/*
	 * @brief Inialise les paramètres des scripts attachés pour pouvoir attaquer et lancer
	 */
	public void Initialiser () {
		Attaquer a = GetComponent<Attaquer> ();
		a.SetBypass (false); //Pouvoir attaquer
		a.SetEnTrainDAttaquer (false);
		a.SetEnCoursDeRetour (false);

		Rigidbody r = GetComponent<Rigidbody> ();
		r.isKinematic = true; //Modification par script des transform
		r.useGravity = false; //Ne pas tomber, l'objet reste dans les mains 

		Lancer l = GetComponent<Lancer> ();
		l.SetBypass (false); //On veut pouvoir lancer

		//Colliders
		Vector3 centreCollider = new Vector3 (.18f, -.45f, .6f);
		BoxCollider bc = GetComponent<BoxCollider> ();
		if (bc) {
			bc.center = centreCollider; //On met le centre au bon endroit pour que l'animation se fasse bien
			bc.isTrigger = true; //On veut traverser les objets
		}
		CapsuleCollider cc = GetComponent<CapsuleCollider> ();
		if(cc) {
			cc.center=centreCollider;
			cc.isTrigger=true;
		}
		SphereCollider sc = GetComponent<SphereCollider> ();
		if(sc) {
			sc.center=centreCollider;
			sc.isTrigger=true;
		}

		//Autres scripts
		ObjetLance ol = GetComponent<ObjetLance> ();
		if(ol)
			DestroyImmediate (ol);

		Soigner s = GetComponent<Soigner> ();
		if(s)
			DestroyImmediate(s);

		Pickable p = GetComponent<Pickable> ();
		if(p)
			DestroyImmediate(p);

		GlowSimple gs = GetComponent<GlowSimple> ();
		if (gs)
			DestroyImmediate (gs);

		DestroyImmediate (this);
	}
}
