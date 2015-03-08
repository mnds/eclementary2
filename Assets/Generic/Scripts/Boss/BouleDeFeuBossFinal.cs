/**
 * \file      BouleDeFeuBossFinal.cs
 * \author    
 * \version   1.0
 * \date      8 mars 2015
 * \brief     Attaché aux boules de feu. Inflige des dégats au contact.
 */

using UnityEngine;
using System.Collections;

public class BouleDeFeuBossFinal : MonoBehaviour {
	void OnCollisionEnter (Collision c) {
		Health h = c.gameObject.GetComponent<Health>();
		if(h) {
			h.SubirDegats(10000f); //10000 dégats
			Debug.Log ("Degats boule de feu infligés à "+c.gameObject.name);
		}
		Destroy (gameObject);
	}
}
