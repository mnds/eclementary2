/**
 * \file      EtatGymnase.cs
 * \author    
 * \version   1.0
 * \date      20 janvier 2015
 * \brief     Arrivée dans le gymnase
 */

using UnityEngine;

public class EtatGymnase : EtatJouable {
	
	public EtatGymnase( StateManager manager ) : base( manager ) {
		sceneCorrespondante = ControlCenter.nomDeLaSceneDuGymnase;
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
