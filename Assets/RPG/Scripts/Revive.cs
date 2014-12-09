/**
 * \file      Revive.cs
 * \author    
 * \version   1.0
 * \date      7 décembre 2014
 * \brief     Permet de quitter l'écran de mort en revenant à la dernière scène.
 */

using UnityEngine;
using System.Collections;

public class Revive : MonoBehaviour {
	void Update () {
		if(Input.GetKeyDown (KeyCode.Backspace)) {
			ScenarioManager.ActiverEvenement(1);
		}
	}
}
