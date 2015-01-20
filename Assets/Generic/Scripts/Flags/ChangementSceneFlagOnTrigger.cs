/**
 * \file      ChangementSceneFlagOnTrigger.cs
 * \author    
 * \version   1.0
 * \date      20 janvier 2015
 * \brief     Active un script ChangementSceneFlag par interaction.
 */

using UnityEngine;
using System.Collections;

public class ChangementSceneFlagOnTrigger : ChangementSceneFlag {
	void OnTriggerEnter(Collider collider) {
		if(collider.gameObject==ControlCenter.GetJoueurPrincipal ())
			ActivationFlagActive();
	}
}
