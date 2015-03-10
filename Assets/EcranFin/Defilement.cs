/**
 * \file      Defilement.cs
 * \author    
 * \version   1.0
 * \date      9 mars 2015
 * \brief     Gère le défilement d'un texte vers le haut
 *
 * \details   Permet de faire défiler un texte vers le haut à une vitesse "vitesse"
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Defilement : MonoBehaviour {



	public float vitesse = 1f;
	private Vector3 translation;

	// Use this for initialization
	void Start () 
	{

	}

	// Update is called once per frame
	void Update () 
	{
		translation.y = vitesse * Time.deltaTime;
		transform.Translate (translation);
	}

}
