/**
 * \file      EtatMort.cs
 * \author    
 * \version   1.0
 * \date      12 décembre 2014
 * \brief     Etat qui gère la mort du joueur
 */

using UnityEngine;
public class EtatMort : EtatNonJouable
{ 
	
	public EtatMort ( StateManager manager ) : base( manager ) {
		sceneCorrespondante = ControlCenter.nomDeLaSceneDeMort;
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


