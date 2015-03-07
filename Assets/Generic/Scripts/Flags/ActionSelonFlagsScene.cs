/**
 * \file      ActionSelonFlagsScene.cs
 * \author    
 * \version   1.0
 * \date      7 mars 2015
 * \brief     Attaché aux objets dont certaines actions dans des scènes dépendent des flags.
 * \details	  Pour des actions sur des objets étant présents sur toutes les scènes, utiliser ActionSelonFlags
 */


using UnityEngine;
using System.Collections;

public class ActionSelonFlagsScene : MonoBehaviour {

	void Start () {
		ControlCenter.AddObjetActivableSelonFlags(this,Application.loadedLevelName);
		Verifier();
	}

	// A remettre dans chaque héritage
	virtual public void Verifier () {
	
	}
}
