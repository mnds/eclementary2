/**
 * \file      ZombieSousAttaque.cs
 * \author    ZepengLI
 * \version   1.0
 * \date      16 decembre 2014
 * \brief     Calculer les degats subis par le zombie si il est attaque par l'arme du joueur.
 *
 * \details  
 */

/*
 * 
 */

using UnityEngine;
using System.Collections;

public class ZombieSousAttaque : MonoBehaviour, IScriptEtatJouable {

	private bool enabled = true; // variable booléenne qui servira à l'implémentation des méthodes de IScriptEtatJouable

	void OntriggerEnter (Collider other)
	{ 
		if (!enabled)
			return;	
		Attaquer m_attaquer = other.GetComponent<Attaquer>();
			ZombieHealth healthZombie = this.GetComponent<ZombieHealth>(); 
			
			if (m_attaquer) {  //Si l'object qui rentre contient le script "Attaquer"
				healthZombie.SubirDegats(m_attaquer.degatsParCoup);	
				}
	}

	// Implémentation de IScriptEtatJouable
	public bool isEnabled() {
		return enabled;
	}
	
	public void setEnabled( bool ok ) {
		enabled = ok;
	}
}
