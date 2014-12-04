using UnityEngine;
using System.Collections;

public class LancerLabyrinthe : MonoBehaviour {
	public string nomDeLaSceneLabyrinthe = "Labyrinthe";
	private bool bypass = false;
	public float tempsAttente = 30f;
	private float tempsFinal;
	private bool oculus = false;

	void OnGUI () {
		//On affiche le temps restant
		if (bypass) {
			GUIStyle skin = GUI.skin.label;
			skin.alignment = TextAnchor.MiddleCenter;
			int tempsRestant=Mathf.CeilToInt (tempsFinal-Time.time);
			string affichageTemps = "";
			if(tempsRestant<1) return; //Si on a moins d'une seconde on n'affiche rien.
			if(tempsRestant==1) affichageTemps="Il reste 1 seconde.";
			if(tempsRestant>1) affichageTemps="Il reste "+tempsRestant+" secondes.";
			GUI.Label(new Rect (Screen.width * 0.4f, Screen.height * 0.4f, Screen.width * 0.2f, Screen.height * 0.2f), affichageTemps, skin);
			return;
		}

		string texteBoutonOculus;
		if (oculus)
			texteBoutonOculus = "Cliquez pour désactiver l'oculus";
		else
			texteBoutonOculus = "Cliquez pour activer l'oculus";


		if (GUI.Button (new Rect (Screen.width * 0.4f, Screen.height * 0.2f, Screen.width * 0.2f, Screen.height * 0.2f), texteBoutonOculus)) {
			UtiliserOculus();
		}
		if (GUI.Button (new Rect (Screen.width * 0.4f, Screen.height * 0.4f, Screen.width * 0.2f, Screen.height * 0.2f), "Lancer le labyrinthe")) {
			StartCoroutine(Lancer(false));
			bypass=true;
		}
		if (GUI.Button (new Rect (Screen.width * 0.4f, Screen.height * 0.6f, Screen.width * 0.2f, Screen.height * 0.2f), "Lancer le labyrinthe - test biopack")) {
			StartCoroutine(Lancer(true));
			bypass=true;
		}
	}

	private void UtiliserOculus () {
		oculus = !oculus;
		ControlCenter.SetUtiliserOculus (oculus);
	}

	public IEnumerator Lancer (bool attendre) {
		if(attendre) {
			Debug.Log("Début d'attente");
			tempsFinal=Time.time+tempsAttente;
			yield return new WaitForSeconds(tempsAttente);
		}
		else {
			yield return new WaitForSeconds(0.1f);
		}

		Application.LoadLevel(nomDeLaSceneLabyrinthe);
	}
}
