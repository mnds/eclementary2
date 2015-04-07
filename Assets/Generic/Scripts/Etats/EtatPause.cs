using UnityEngine;
using System.Collections;

public class EtatPause : EtatNonJouable {

	public EtatPause (StateManager manager, string nomScene):base(manager){
		sceneCorrespondante = nomScene;
	}

	// Update is called once per frame
public override void UpdateEtat () {
		ChargerSceneCorrespondante ();  // Juste pour tester si la scène 
		if (Input.GetButtonDown ("Pause")) {
			if (Time.timeScale > 0) {  //  Au cas où, normalement le Timescale doit forcément etre 0 s il rentre dans cet état...
				Time.timeScale = 0;
			} else { // On quitte l'état pause
				Time.timeScale = 1;

				GameObject joueur = ControlCenter.GetJoueurPrincipal();
				InteractionManager im = joueur.GetComponent<InteractionManager> (); // Récupération du gestionnaire d'interactions
				if( !im.GetInteractionEnCours() )
					ControlCenter.GetJoueurPrincipal().GetComponent<ControllerJoueur> ().SetFreeze(false); // La tête n'est débloquée que lorsqu'il n'y a pas d'intéraction (bloquant la tête) déjà en cours

				StateManager.getInstance().BasculerEtat(StateManager.getInstance().ancienEtat);
			}
				
		}
	}
	public override void AfficherRendu(){
				if (Time.timeScale == 0) { 
			GUIStyle guiStyle = new GUIStyle(GUI.skin.GetStyle("Button"));
			guiStyle.fontSize = 20;
			if (GUI.Button (new Rect (Screen.width / 3, Screen.height / 3, 500, 100), "Revenir au jeu", guiStyle)) {
				Time.timeScale = 1;
				ControlCenter.GetJoueurPrincipal().GetComponent<ControllerJoueur> ().SetFreeze(false);
				StateManager.getInstance().BasculerEtat(StateManager.getInstance().ancienEtat);
						}
			if (GUI.Button (new Rect (Screen.width / 3, Screen.height / 3 + 110, 500, 100), "Quitter le jeu", guiStyle)) {
				Application.Quit();
						}
				}
		}
}