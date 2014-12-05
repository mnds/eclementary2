using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lancement : MonoBehaviour {
	public List<string> nomsDesScenes;
	string nomDeLaScene = "";
	private bool bypass = false;
	public float tempsAttente = 30f;
	private float tempsFinal;
	private bool oculus = false;
	private bool utiliserBiopack=false;

	void Awake () {
		Debug.Log ("Ok");
		GetComponent<AudioSource> ().Play ();
	}

	void OnGUI () {
		GUIStyle skin = GUI.skin.label;
		skin.alignment = TextAnchor.MiddleCenter;
		//On affiche le temps restant
		if (bypass) {
			int tempsRestant=Mathf.CeilToInt (tempsFinal-Time.time);
			string affichageTemps = "";
			if(tempsRestant<1) return; //Si on a moins d'une seconde on n'affiche rien.
			if(tempsRestant==1) affichageTemps="Il reste 1 seconde.";
			if(tempsRestant>1) affichageTemps="Il reste "+tempsRestant+" secondes.";
			GUI.Label(new Rect (Screen.width * 0.4f, Screen.height * 0.4f, Screen.width * 0.2f, Screen.height * 0.2f), affichageTemps, skin);
			return;
		}

		//Bouton Oculus placé en bas de l'écran
		string texteBoutonOculus;
		if (oculus)
			texteBoutonOculus = "Cliquez pour désactiver l'oculus";
		else
			texteBoutonOculus = "Cliquez pour activer l'oculus";
		if (GUI.Button (new Rect (Screen.width * 0.0f, Screen.height * 0.8f, Screen.width * 0.5f, Screen.height * 0.2f), texteBoutonOculus)) {
			UtiliserOculus();
		}

		//Bouton BioPack placé en bas de l'écran
		string texteBoutonBiopack;
		if (!utiliserBiopack)
			texteBoutonBiopack = "Cliquez pour la version Biopack (30s d'attente)";
		else
			texteBoutonBiopack = "Cliquez pour la version normale";
		if (GUI.Button (new Rect (Screen.width * 0.5f, Screen.height * 0.8f, Screen.width * 0.5f, Screen.height * 0.2f), texteBoutonBiopack)) {
			utiliserBiopack=!utiliserBiopack;
		}

		//Tous les autres boutons
		int nombreDeScenes = nomsDesScenes.Count;
		if(nombreDeScenes==0) //Pas d'autres scènes !
			GUI.Label(new Rect (Screen.width * 0.4f, Screen.height * 0.4f, Screen.width * 0.2f, Screen.height * 0.2f), "Rien à afficher !", skin);

		//Positionnement
		for (int i=0; i<nombreDeScenes; i++) {
			float positionx=(int)(i%5)*Screen.width*0.2f;
			float positiony=(int)(i/5)*Screen.height*0.2f;
			if(nomsDesScenes[i]!="") {
				if (GUI.Button (new Rect (positionx, positiony, Screen.width * 0.2f, Screen.height * 0.2f), nomsDesScenes[i])) {
					nomDeLaScene=nomsDesScenes[i];
					StartCoroutine(Lancer());
					bypass=true;
				}
			}
		}
	}

	private void UtiliserOculus () {
		oculus = !oculus;
		ControlCenter.SetUtiliserOculus (oculus);
	}

	public IEnumerator Lancer () {
		if(utiliserBiopack) {
			Debug.Log("Début d'attente");
			tempsFinal=Time.time+tempsAttente;
			yield return new WaitForSeconds(tempsAttente);
		}
		else {
			yield return new WaitForSeconds(0.1f);
		}

		Application.LoadLevel(nomDeLaScene);
	}
}
