/**
 * \file      ActiverFlagInteractif.cs
 * \author    
 * \version   1.0
 * \date      11 décembre 2014
 * \brief     Active un flag après avoir interagit avec le gameObject auquel le script est attaché. Les actions à faire en meme temps que l'activation du flag sont faites dans ce meme script à travers une méthode virtuelle.
 * 
 * \details	  Hérite de Interactif.
 */

using UnityEngine;
using System.Collections;

public class ActiverFlagInteractif : ActivationFlag, Interactif {
	public float distanceMinimaleInteraction = 4.0f; //La distance à laquelle on doit etre pour pouvoir interagir avec l'objet

	public void DemarrerInteraction() {
		ActivationFlagActive();
	}

	public void ArreterInteraction() {

	}

	public void SetDistanceMinimaleInteraction (float distanceMinimaleInteraction_)
	{
		distanceMinimaleInteraction = distanceMinimaleInteraction_;
	}

	public float GetDistanceMinimaleInteraction ()
	{
		return distanceMinimaleInteraction;
	}
}
