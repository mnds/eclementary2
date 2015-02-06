/**
 * \file      EtatChambreGarsFoyer.cs
 * \author    
 * \version   1.0
 * \date      20 janvier 2015
 * \brief     Arrivée dans la scène de la chambre du gars du foyer à Comparat
 */

using UnityEngine;

public class EtatChambreGarsFoyer : EtatJouable {
	
	public EtatChambreGarsFoyer( StateManager manager ) : base( manager ) {
		sceneCorrespondante = ControlCenter.nomDeLaSceneDeLaChambreGarsFoyer;
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
