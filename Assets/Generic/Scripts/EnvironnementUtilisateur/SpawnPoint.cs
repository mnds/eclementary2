/**
 * \file      SpawnPoint.cs
 * \author    
 * \version   1.0
 * \date      7 janvier 2015
 * \brief     Transporte le joueurPrincipal donné par ControlCenter à sa position.
 */


using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour {
	public bool estActif = true;

	void Awake () {
		if(estActif) {
			Teleportation();
		}
	}

	public void Teleportation () {
		ControlCenter.GetJoueurPrincipal ().transform.position=gameObject.transform.position;
		ControlCenter.GetJoueurPrincipal ().transform.rotation=gameObject.transform.rotation;
	}

	public bool GetEstActif () {
		return estActif;
	}

	public void SetEstActif (bool estActif_) {
		estActif=estActif_;
	}
}
