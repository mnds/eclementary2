/**
 * \file      Health.cs
 * \author    
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

	/**
	 * @brief Permet de rajouter des points de vie à l'objet.
	 * @param soin De combien augmenter les points de vie.
	 *
	 * @return Renvoie true si un soin a été effectué.
	 */
	public bool Soigner(float soin) {
		float pdvaAvantSoin = pointsDeVieActuels;
		pointsDeVieActuels = Mathf.Min (pointsDeVieActuels + soin, pointsDeVieMax);
		return (pdvaAvantSoin != pointsDeVieActuels);
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

	public void SetPointsDeVieMax (float pdvm_) {
		pointsDeVieMax = pdvm_;
		pointsDeVieActuels = Mathf.Max (pointsDeVieMax, pointsDeVieActuels);
	}
	
	public float GetPointsDeVieMax () {
		return pointsDeVieMax;
	}

	public float GetPointsDeVieActuels () {
		return pointsDeVieActuels;
	}
}
