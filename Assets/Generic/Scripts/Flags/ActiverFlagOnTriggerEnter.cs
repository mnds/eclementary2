/**
 * \file      ActiverFlagOnTriggerEnter.cs
 * \author    
 * \version   1.0
 * \date      11 décembre 2014
 * \brief     Active un flag après avoir avoir passé un trigger.
 */

using UnityEngine;
using System.Collections;

public class ActiverFlagOnTriggerEnter : ActivationFlag {
	void OnTriggerEnter(Collider collider) {
		if(collider.gameObject==ControlCenter.GetJoueurPrincipal ())
			ActivationFlagActive();
	}
}
