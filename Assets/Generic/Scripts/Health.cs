/**
 * \file      Health.cs
 * \author    BC
 * \version   1.0
 * \date      9 novembre 2014
 * \brief     Contient les points de vie de l'objet. A sa mort, on appelle DeclencherMort.
 */

using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
	public float pointsDeVieMax = 5f;
	public float pointsDeVieActuels = 5f;

	void Start () {
		pointsDeVieActuels = Mathf.Min (pointsDeVieActuels, pointsDeVieMax);
	}

	public void SubirDegats(float degats) {
		pointsDeVieActuels = Mathf.Min (pointsDeVieActuels, pointsDeVieMax); //Au cas où il y ait eu un problème
		pointsDeVieActuels -= degats;
		if (pointsDeVieActuels <= 0) {
			DeclencherMort ();
		}
	}

	virtual public void DeclencherMort () {

	}
}
