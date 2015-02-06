/**
 * \file      EtatDepart.cs
 * \author    
 * \version   1.1
 * \date      13 janvier 2015
 * \brief     Etat de début du jeu
 */

using UnityEngine;

public class EtatLancement : EtatNonJouable
{
	public EtatLancement ( StateManager manager ) :  base( manager )
	{
		sceneCorrespondante = ControlCenter.nomDeLaSceneDepart;
		ChargerSceneCorrespondante ();
	}

	// Redéfinition des méthodes de la classe abstraite Etat
	public override void UpdateEtat() {
		ChargerSceneCorrespondante ();
	}
	
	public override void AfficherRendu() {
		if (GUI.Button (new Rect (Screen.width / 2, Screen.height / 2, 500, 100), "Entrez dans le campus")) {
			StateManager.getInstance().BasculerEtat( new EtatDepart( StateManager.getInstance() ) );
			FlagManager.ActiverFlag(10); // Activation du flag 10, correspondant au début du jeu
		}
	}
}

