/**
 * \file      Autodestruction.cs
 * \author    BC
 * \version   1.0
 * \date      9 novembre 2014
 * \brief     Permet l'autodestruction simple de l'objet après un certain temps.
 */

using UnityEngine;
using System.Collections;

//Destruction de l'objet après un certain temps
public class Autodestruction : MonoBehaviour {
	public float dureeDeVie = 1.0f;
	// Use this for initialization
	void Update () {
		dureeDeVie -= Time.deltaTime;
		if (dureeDeVie <= 0) {
			Destroy (gameObject);
		}
	}
}
