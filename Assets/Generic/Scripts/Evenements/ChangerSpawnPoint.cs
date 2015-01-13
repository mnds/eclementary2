/**
 * \file      ChangerSpawnPoint.cs
 * \author    
 * \version   1.0
 * \date      13 janvier 2015
 * \brief     Définit les points de spawn. Utilisé pour les différents événements. Donne le nom du point de spawn et si la téléportation doit se faire immédiatement.
 */

using UnityEngine;

public class ChangerSpawnPoint : Evenement {
	Item spawnPoint;

	public ChangerSpawnPoint(Item spawnPoint_) {
		spawnPoint=spawnPoint_;
	}

	public override void DeclencherEvenement( params Item[] items ) {
		/*//items[0] contient le spawnPoint.
		Item spawnPoint = items[0];*/

		if(spawnPoint.GetNom()!=NomItem.SpawnPoint) //Si ce n'est pas un spawnPoint on laisse tomber
			return; 

		//On prend le spawn Point précédent et on le désactive
		string nomSpawnPointPrecedent = ControlCenter.GetNomSpawnPointActuel ();
		GameObject spawnPointPrecedent;
		SpawnPoint spPrecedent;
		if(nomSpawnPointPrecedent!=null) {
			spawnPointPrecedent = GameObject.Find (ControlCenter.GetNomSpawnPointActuel());
			if(spawnPointPrecedent) {//Si le spawnPoint existe
				spPrecedent = spawnPointPrecedent.GetComponent<SpawnPoint>();
				if(spPrecedent!=null) //S'il a bien un script SpawnPoint
					spPrecedent.SetEstActif (false); //On le désactive
			}
		}


		// On change de spawnPoint
		ControlCenter.SetNomSpawnPointActuel(spawnPoint.GetNomItem());

		if(spawnPoint.GetTeleportationImmediate()) { //Ajouter ici une condition sur la scène
			GameObject g0 = GameObject.Find (spawnPoint.GetNomItem());
			if(g0==null) return;
			SpawnPoint sp = g0.GetComponent<SpawnPoint>();
			if(sp==null) return;
			sp.Teleportation();
		}
	}
}