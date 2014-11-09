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
	public override void DeclencherMort () {
		Destroy (gameObject);
	}
}
