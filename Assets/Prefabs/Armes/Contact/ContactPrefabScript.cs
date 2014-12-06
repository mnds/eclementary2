/**
 * \file      ContactPrefabScript.cs
 * \author    
 * \version   1.0
 * \date      6 décembre 2014
 * \brief     Ajoute les scripts nécessaires aux prefabs des armes. A utiliser avec l'éditeur prévu.
 */

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Pickable))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ObjetLance))]
[RequireComponent(typeof(Collider))]
public class ContactPrefabScript : MonoBehaviour {
	// Use this for initialization
	void Awake () {
		Initialiser ();
	}

	/*
	 * @brief Inialise les paramètres des scripts attachés pour pouvoir attaquer et lancer
	 */
	public void Initialiser () {
		Pickable p = GetComponent<Pickable> ();
		p.SetBypass (true);
		p.SetPickableDebut (true);

		ObjetLance ol = GetComponent<ObjetLance> ();
		ol.SetBypass (false);
		ol.aUneDureeDeVie = false; //L'objet ne doit pas disparaitre

		Rigidbody r = GetComponent<Rigidbody> ();
		r.isKinematic = true;
		r.useGravity = true;


		//Colliders
		Vector3 centreCollider = new Vector3 (.18f, -.45f, .6f);
		BoxCollider bc = GetComponent<BoxCollider> ();
		if(bc)
			bc.center =centreCollider;
		CapsuleCollider cc = GetComponent<CapsuleCollider> ();
		if(cc)
			cc.center=centreCollider;
		SphereCollider sc = GetComponent<SphereCollider> ();
		if(sc)
			sc.center=centreCollider;

		//Autres scripts
		Attaquer a = GetComponent<Attaquer> ();
		if(a)
			DestroyImmediate (a);

		Soigner s = GetComponent<Soigner> ();
		if(s)
			DestroyImmediate(s);

		GlowSimple gs = GetComponent<GlowSimple> ();
		if (gs)
			DestroyImmediate (gs);

		Lancer l = GetComponent<Lancer> ();
		if (l)
			DestroyImmediate (l);
	}
}
