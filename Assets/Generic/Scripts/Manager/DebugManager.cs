/**
 * \file      DebugManager.cs
 * \author    
 * \version   1.0
 * \date      9 novembre 2014
 * \brief     Permet l'accès à différentes fonctionnalités de Debug.
 * 
 * \details	  Permet de passer en mode debug en tapant une ligne de commande dans une scène qui contient un DebugManager.
 * 			  /load <niveau> permet de lancer la scène ayant pour nom "<niveau>".
 */

using UnityEngine;
using System.Collections;

public enum ModesJeu 
{
	Normal,
	Debug
}

public class DebugManager : MonoBehaviour {
	string commandeDebug = "/debug";
	string commandeNormal = "/normal";
	string commandeActuelle = "";

	public ModesJeu modeDepart;

	void Start () {
		ControlCenter.mode=modeDepart;
	}

	void Update () {
		//On récupère les touches entrées.
		foreach (char c in Input.inputString) {
			if (c == "\b"[0]) {//Si backspace
				if (commandeActuelle.Length != 0)
					commandeActuelle = commandeActuelle.Substring(0, commandeActuelle.Length - 1);
				return;
			}
			if (c == "\n"[0] || c == "\r"[0]) { //Si on fait entrée ou return
				Debug.Log("Commande entrée : " + commandeActuelle);
				VerifierCombinaison();
				commandeActuelle=""; //On remet à 0 la commande
				return;
			}
			commandeActuelle += c;
		}
	}

	void VerifierCombinaison() { //La combinaison entrée fait-elle quelque chose ?
		//On vérifie si la combinaison actuelle correspond à quelque chose
		switch (ControlCenter.mode) {
		case ModesJeu.Normal:
			if(commandeActuelle==commandeDebug) { //Si on demande à passer en debug
				ControlCenter.mode=ModesJeu.Debug;
				Debug.Log ("Mode Debug activé");
			}
			break;
		case ModesJeu.Debug:
			if(commandeActuelle==commandeNormal) { //Si on demande à passer en debug
				ControlCenter.mode=ModesJeu.Normal;
				Debug.Log ("Mode Debug désactivé");
			}
			if(commandeActuelle.Length>6 && commandeActuelle.Substring(0,6)=="/load ") {
				Application.LoadLevel(commandeActuelle.Substring(6));
			}
			break;
		}
	}

	void OnGUI () {
		if(ControlCenter.mode==ModesJeu.Debug) {
			GUI.Label (new Rect (0, 0, 100, 35), "DebugMode"); //On écrit qu'on est en mode debug
		}
	}

}
