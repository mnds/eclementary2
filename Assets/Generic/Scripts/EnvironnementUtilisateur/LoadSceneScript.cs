/**
 * \file      LoadSceneScript.cs
 * \author    
 * \version   1.0
 * \date      9 novembre 2014
 * \brief     Lance la scène dont le nom est mis en argument si un objet de tag Player entre dans le collider de l'objet.
 */

using UnityEngine;
using System.Collections;

public class LoadSceneScript : MonoBehaviour {
	public string nomScene;
	// Use this for initialization
	void OnTriggerEnter (Collider collider) {
		if(collider.tag=="Player")
			Application.LoadLevel (nomScene);
	}
}
