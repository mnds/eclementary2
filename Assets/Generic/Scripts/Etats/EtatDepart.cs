/**
 * \file      EtatDepart.cs
 * \author    
 * \version   1.1
 * \date      12 décembre 2014
 * \brief     Etat de début du jeu
 */

using UnityEngine;

public class EtatDepart : EtatJouable {
	
	public EtatDepart( StateManager manager ) : base( manager ) {
		sceneCorrespondante = "CampusExterieurLie";
		etatJouable = true;
		ChargerSceneCorrespondante ();
	}

	// Redéfinition des méthodes de la classe abstraite Etat

	public override void UpdateEtat() {
		ChargerSceneCorrespondante ();
	}

	public override void AfficherRendu() {
	
	}
}
