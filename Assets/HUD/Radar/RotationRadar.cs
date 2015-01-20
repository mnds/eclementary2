/**
 * \file      RotationRadar.cs
 * \author    
 * \version   1.0
 * \date      20 janvier 2015
 * \brief     Permet de faire tourner le radar
 */

using UnityEngine;
using System.Collections;

public class RotationRadar : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Rotate(0,0,-200f*Time.deltaTime,Space.World);
	}
}
