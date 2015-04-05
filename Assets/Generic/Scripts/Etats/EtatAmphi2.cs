/**
 * \file      EtatAmphi2.cs
 * \author    
 * \version   1.0
 * \date      21 janvier 2015
 * \brief     Arrivée dans l'amphi2
 */

using UnityEngine;

public class EtatAmphi2: EtatJouable {
	
	public EtatAmphi2( StateManager manager ) : base( manager ) {
		sceneCorrespondante = ControlCenter.nomDeLaSceneAmphi2;
		etatJouable = true;
		ChargerSceneCorrespondante ();
	}

	// Redéfinition des méthodes de la classe abstraite Etat

	public override void UpdateEtat() {
		base.UpdateEtat ();
		ChargerSceneCorrespondante ();
	}

	public override void AfficherRendu() {
	
	}
}
