/**
 * \file      HealthDestroy.cs
 * \author    BC
 * \version   1.0
 * \date      9 novembre 2014
 * \brief     Contient les points de vie de l'objet. A sa mort, il est détruit (appel à Destroy).
 */

using UnityEngine;
using System.Collections;

public class HealthDestroy : Health {
	public GameObject effetDeMort; //Instantié si mort
	public GameObject objetADetruire; //Autre objet que le gameObject qui pourrait etre amené à disparaitre

	public override void DeclencherMort () {
		GameObject objetDestruction;
		if (objetADetruire)
			objetDestruction = objetADetruire;
		else
			objetDestruction=gameObject;

		if (effetDeMort)
			Instantiate (effetDeMort, objetDestruction.transform.position, Quaternion.identity);
		Destroy (objetDestruction);
	}

	override protected void OnChangementPointsDeVie () {

	}
}
