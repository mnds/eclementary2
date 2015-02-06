/**
 * \file      HealthHide.cs
 * \author    
 * \version   1.0
 * \date      3 février 2015
 * \brief     Contient les points de vie de l'objet. A sa mort, il est caché.
 */

using UnityEngine;
using System.Collections;

public class HealthHide : Health {
	public GameObject effetDeMort; //Instantié si mort
	public GameObject objetADetruire; //Autre objet que le gameObject qui pourrait etre amené à disparaitre

	public override void DeclencherMort () {
		gameObject.SetActive(false);
	}
}
