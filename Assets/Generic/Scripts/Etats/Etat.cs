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
	protected string sceneCorrespondante; // Nom de la scène correspondant à l'état
	protected static bool etatJouable; // Renseigne si oui ou non le joueur peut jouer lorsqu'il est dans ce état

	public Etat( StateManager manager ) {
		stateManager = manager;
		ConfigurerScripts ();
	}

	public virtual void ChargerSceneCorrespondante() {
		if (Application.loadedLevelName != sceneCorrespondante) {
			Debug.Log ("Chargement de la scène " + sceneCorrespondante );
			Application.LoadLevel (sceneCorrespondante);
			ControlCenter.VerifierLesOASFs();
		}
	}

	public string getSceneCorrespondante() {
		return sceneCorrespondante;
	}

	public virtual void DesactiverEtat() {

	}

	public abstract void ConfigurerScripts(); // Ensemble de fonctions appelées lorsque l'état est chargé
	public abstract void UpdateEtat(); // Permettra au StateManager d'activer l'état en question
	public abstract void AfficherRendu(); // Permet d'afficher le rendu de l'état
}
