/**
 * \file      EtatFin.cs
 * \author    
 * \version   1.0
 * \date      3 février 2015
 * \brief     Etat exécuté lorsque le joueur a atteint l'objectif final
 */

using UnityEngine;

public class EtatFin : EtatNonJouable {

	public EtatFin ( StateManager manager ) : base( manager ) {
		sceneCorrespondante = ControlCenter.nomDeLaSceneDeFin;
		ChargerSceneCorrespondante ();
	}

	// Redéfinition des méthodes de la classe abstraite Etat	
	public override void UpdateEtat() {
		ChargerSceneCorrespondante ();
	}
	
	public override void AfficherRendu() {
		GUI.Label (new Rect (Screen.width / 2, Screen.height / 4, 200, 50), "F\u00E9licitations, vous venez de terminer le jeu !");
		if ( GUI.Button( new Rect(Screen.width / 2, Screen.height / 3, 200, 50), "Rejouer ?" ) ) {
			StateManager.getInstance().Restart();
		}
	}
}



