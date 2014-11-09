/**
 * \file      MazeOscillerSon.cs
 * \author    
 * \version   1.0
 * \date      9 novembre 2014
 * \brief     Crée un son qui oscille derrière le joueur.
 *
 * \details   Placé sur un objet qui va constamment osciller derrière la tete du joueur.
 * 			  On obtient l'effet désiré en attachant ce script à un gameObject contenant une AudioSource et un son approprié.
 */

using UnityEngine;
using System.Collections.Generic;

public class MazeOscillerSon : MonoBehaviour {
	GameObject fpc;

	// Use this for initialization
	void Start () {
		fpc = GameObject.Find ("First Person Controller");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float weight = Mathf.Cos (Time.time * 2 * Mathf.PI + 0.5F);
		transform.position = fpc.transform.position - fpc.transform.forward + fpc.transform.right * weight;
	}
}
