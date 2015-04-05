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
				ControlCenter.GetJoueurPrincipal().GetComponent<ControllerJoueur> ().SetFreeze(true);
			} else {
				Time.timeScale = 1;
				ControlCenter.GetJoueurPrincipal().GetComponent<ControllerJoueur> ().SetFreeze(false);
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
			if (GUI.Button (new Rect (Screen.width / 3, Screen.height / 3 + 110, 500, 100), "Charger partie", guiStyle)) {
				Application.Quit();
						}
				}
		}
}