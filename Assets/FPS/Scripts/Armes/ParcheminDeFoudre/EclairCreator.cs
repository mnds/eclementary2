/**
 * \file      EclairCreator.cs
 * \author    TM	
 * \version   1.0
 * \date      27 février 2015
 * \brief     Permet de générer des éclairs au cours du temps pour créer l'arc électrique
 */

using UnityEngine;
using System.Collections;

public class EclairCreator : MonoBehaviour {

	public EclairRender eclairPrefab;
	public float cooldown = 0.5f; //Temps entre deux éclairs consécutifs;
	float tempsDuDernierEclair = 0f; //Tout est dans le nom

	// Update is called once per frame
	void Update () {
		if(Time.time>tempsDuDernierEclair+cooldown)
		{
			tempsDuDernierEclair=Time.time;
			Instantiate (eclairPrefab, this.transform.position, Quaternion.identity);
		}
	
}
}
