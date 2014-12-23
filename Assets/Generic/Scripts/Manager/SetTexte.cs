/**
 * \file      SetTexte.cs
 * \author    
 * \version   1.0
 * \date      16 décembre 2014
 * \brief     Indique à ControlCenter que le gameObject sur lequel est placé ce script est le texte qui s'affichera pour les interactions.
 */

using UnityEngine;
using System.Collections;

public class SetTexte : MonoBehaviour {
	
	// Use this for initialization
	void Awake () {
		ControlCenter.SetTexte (gameObject.GetComponent<GUIText>());
		Destroy (this);
	}
}
