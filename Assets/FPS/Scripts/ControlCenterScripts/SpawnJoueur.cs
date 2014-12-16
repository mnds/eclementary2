/**
 * \file      SpawnJoueur.cs
 * \author    
 * \version   1.0
 * \date      9 novembre 2014
 * \brief     Point de départ du joueur dans la scène.
 *
 * \details   Instantie le joueur donné en attribut au point de départ choisi.
 */

using UnityEngine;
using System.Collections;

public class SpawnJoueur : MonoBehaviour {
	public GameObject pointDeDepartJoueur;
	public GameObject joueur;
	// Use this for initialization
	void Awake () {
		GameObject.Find ("Joueur").transform.position=transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
