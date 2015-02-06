/**
 * \file      EtatClubBD.cs
 * \author    
 * \version   1.0
 * \date      20 janvier 2015
 * \brief     Arrivée dans le club BD
 */

using UnityEngine;

public class EtatClubBD : EtatJouable {
	
	public EtatClubBD( StateManager manager ) : base( manager ) {
		sceneCorrespondante = ControlCenter.nomDeLaSceneDuClubBD;
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
