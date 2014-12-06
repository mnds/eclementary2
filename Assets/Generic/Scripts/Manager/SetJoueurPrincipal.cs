/**
 * \file      SetJoueurPrincipal.cs
 * \author    
 * \version   1.0
 * \date      22 novembre 2014
 * \brief     Indique à ControlCenter que le gameObject sur lequel est placé ce script est le joueur principal.
 */

using UnityEngine;
using System.Collections;

public class SetJoueurPrincipal : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		ControlCenter.SetJoueurPrincipal (gameObject);
		Destroy (this);
	}
}
