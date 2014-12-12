/**
 * \file      Etat.cs
 * \author    
 * \version   1.0
 * \date      12 décembre 2014
 * \brief     Classe de base des états
 */

using UnityEngine;

public abstract class Etat {
	protected StateManager stateManager; // Référence à l'instance de StateManager
	public static string sceneCorrespondante; // Nom de la scène correspondant à l'état

	public Etat( StateManager manager ) {
		stateManager = manager;
		if (Application.loadedLevelName != sceneCorrespondante)
			Application.LoadLevel ( sceneCorrespondante );
	}

	public abstract void UpdateEtat(); // Permettra au StateManager d'activer l'état en question
	public abstract void AfficherRendu(); // Permet d'afficher le rendu de l'état
}
