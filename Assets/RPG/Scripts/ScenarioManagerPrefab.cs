/**
 * \file      ScenarioManagerPrefab.cs
 * \author    
 * \version   1.0
 * \date      7 décembre 2014
 * \brief     Contient les références des objets liés aux événements du jeu.
 *
 * \details   Les objets sont listés dans une liste de listes. Cette liste globale contient les objets aux événements d'indices correspondants.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScenarioManagerPrefab : MonoBehaviour {
	public List<ObjetsEvenements> _listeOE = new List<ObjetsEvenements>();

	[System.Serializable]
	public class ObjetsEvenements
	{
		public int idEvenement;
		public List<GameObject> objets;
	}

	/**
	 * @brief Récupère la liste liée à l'événement demandé.
	 * @param id Numéro lié à l'événement à enclencher.
	 */
	public List<GameObject> GetListObjetsEvenements (int id) {
		foreach (ObjetsEvenements oe in _listeOE) {
			if(oe.idEvenement==id) {
				return oe.objets;
			}
		}
		return null;
	}
}
