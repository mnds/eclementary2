/**
 * \file      EtatDepart.cs
 * \author    
 * \version   1.1
 * \date      13 janvier 2015
 * \brief     Etat de début du jeu
 */

using UnityEngine;
using System.IO;

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
		GUIStyle guiStyle = new GUIStyle(GUI.skin.GetStyle("Button"));
		guiStyle.fontSize = 20;
		if (GUI.Button (new Rect (Screen.width / 3, Screen.height / 3, 500, 100), "Entrez dans le campus", guiStyle)) {
			StateManager.getInstance().BasculerEtat( new EtatDepart( StateManager.getInstance() ) );
			FlagManager.ActiverFlag(10); // Activation du flag 10, correspondant au début du jeu
		}
		// Proposition de chargement d'une partie précédemment sauvegardée
		if ( File.Exists( ControlCenter.nomFichierSauvegarde ) && GUI.Button (new Rect (Screen.width / 3, Screen.height / 3 + 110, 500, 100), "Charger partie", guiStyle)) {
			StateManager.getInstance().gameData.ChargerSauvegarde ();
		}
	}
}

