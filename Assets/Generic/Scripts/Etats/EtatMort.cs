/**
 * \file      EtatMort.cs
 * \author    
 * \version   1.0
 * \date      12 décembre 2014
 * \brief     Etat qui gère la mort du joueur
 */

using UnityEngine;
public class EtatMort : Etat
{ 
	
	public EtatMort ( StateManager manager ) : base( manager ) {
		sceneCorrespondante = "Ecran Mort Lie";
		ChargerSceneCorrespondante ();
	}

	// Redéfinition des méthodes de la classe abstraite Etat
	
	public override void UpdateEtat() {
		
	}
	
	public override void AfficherRendu() {
		
	}

}


