/**
 * \file      BlobShadowFollow.cs
 * \author    
 * \version   1.0
 * \date      9 novembre 2014
 * \brief     Empeche un blob shadow projector de tourner en meme temps qu'une sphere en conservant son orientation au cours du temps.
 *
 * \details   On associe un blob shadow à un objet parent, au dessus duquel l'élément doit rester sans tourner.
 */

using UnityEngine;
using System.Collections;

//Empeche un blob shadow projector de tourner en meme temps qu'une sphere
public class BlobShadowFollow : MonoBehaviour {
	Vector3 distanceInitialeBlobObjet;
	Transform parent;
	// Use this for initialization
	void Start () {
		parent = transform.parent;
		distanceInitialeBlobObjet = transform.position - parent.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = parent.position + distanceInitialeBlobObjet;
		transform.rotation = Quaternion.LookRotation (-distanceInitialeBlobObjet, parent.forward);
	}
}
