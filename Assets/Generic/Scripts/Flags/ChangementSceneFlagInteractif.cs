/**
 * \file      ChangementSceneFlagInteractif.cs
 * \author    
 * \version   1.0
 * \date      18 janvier 2015
 * \brief     Active un script ChangementSceneFlag par interaction.
 * 
 * \details	  Hérite de Interactif.
 */

using UnityEngine;
using System.Collections;

public class ChangementSceneFlagInteractif : ChangementSceneFlag, Interactif {
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
