/**
 * \file      EtatLabyrinthe.cs
 * \author    
 * \version   1.0
 * \date      20 janvier 2015
 * \brief     Arrivée dans la scène de la scolarite
 */

using UnityEngine;

public class EtatLabyrinthe : EtatJouable {
	
	public EtatLabyrinthe( StateManager manager ) : base( manager ) {
		sceneCorrespondante = ControlCenter.nomDeLaSceneLabyrinthe;
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
