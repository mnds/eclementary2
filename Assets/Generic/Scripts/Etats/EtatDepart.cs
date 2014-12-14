﻿/**
 * \file      EtatDepart.cs
 * \author    
 * \version   1.0
 * \date      12 décembre 2014
 * \brief     Etat de début du jeu
 */

using UnityEngine;

public class EtatDepart : Etat {

	HealthPlayer healthPlayer = new HealthPlayer();

	public EtatDepart( StateManager manager ) : base( manager ) {
		sceneCorrespondante = "CampusExterieurLie";
		ChargerSceneCorrespondante ();
	}

	// Redéfinition des méthodes de la classe abstraite Etat

	public override void UpdateEtat() {

	}

	public override void AfficherRendu() {

	}
}
