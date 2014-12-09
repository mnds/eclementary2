/**
 * \file      Interactif.cs
 * \author    
 * \version   1.0
 * \date      9 novembre 2014
 * \brief     Interface commune aux scripts d'interaction avec l'environnement.
 */

using UnityEngine;
using System.Collections;

public interface Interactif{
	void DemarrerInteraction();
	void ArreterInteraction();
	void SetDistanceMinimaleInteraction (float distanceMinimaleInteraction_);
	float GetDistanceMinimaleInteraction ();
}
