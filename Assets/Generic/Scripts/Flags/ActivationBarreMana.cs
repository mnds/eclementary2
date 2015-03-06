/**
 * \file      ActivationInterfaceGraphique.cs
 * \author    
 * \version   1.0
 * \date      6 mars 2015
 * \brief     Permet de controler l'affichage des différentes barres en fonction des flags.
 * \details   Sert à afficher/Désactiver la barre de mana
 */


using UnityEngine;
using System.Collections;

public class ActivationBarreMana : ActionSelonFlags {
	override protected void ActionSiBonFlag () {
		Debug.Log ("Bons flags");
		ControlCenter.SetAfficherBarreDeManaJoueur(true);
	}

	override protected void ActionSiMauvaisFlag () {
		Debug.Log ("MAuvais flags");
		ControlCenter.SetAfficherBarreDeManaJoueur(false);
	}
}
