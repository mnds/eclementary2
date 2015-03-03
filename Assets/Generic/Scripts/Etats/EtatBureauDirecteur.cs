/**
 * \file      EtatBureauDirecteur.cs
 * \author    
 * \version   1.0
 * \date      21 janvier 2015
 * \brief     Arrivée dans le bureau du directeur
 */

using UnityEngine;

public class EtatBureauDirecteur : EtatJouable {
	
	public EtatBureauDirecteur( StateManager manager ) : base( manager ) {
		sceneCorrespondante = ControlCenter.nomDeLaSceneDirecteur;
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
