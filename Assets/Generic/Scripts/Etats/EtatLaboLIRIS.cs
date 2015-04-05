/**
 * \file      EtatLaboLIRIS.cs
 * \author    
 * \version   1.0
 * \date      20 janvier 2015
 * \brief     Arrivée dans la scène de la chambre
 */

using UnityEngine;

public class EtatLaboLIRIS : EtatJouable {
	
	public EtatLaboLIRIS( StateManager manager ) : base( manager ) {
		sceneCorrespondante = ControlCenter.nomDeLaSceneLIRIS;
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
