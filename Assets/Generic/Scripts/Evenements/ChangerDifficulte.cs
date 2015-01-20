/**
 * \file      ChangerDifficulte.cs
 * \author    
 * \version   1.0
 * \date      20 janvier 2015
 * \brief     Evénement permettant de changer la difficulté du jeu.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChangerDifficulte : Evenement {
	public override void DeclencherEvenement( params Item[] items ) {
		//Le premier item contient la difficulte
		Item d = items[0];
		ControlCenter.Difficulte diff=d.GetNomDifficulte();
		switch(d.GetNomDifficulte()) {
		case(ControlCenter.Difficulte.Facile):
			ControlCenter.difficulteActuelle=diff;
			FlagManager.ChercherFlagParId(1).actif=true;
			FlagManager.ChercherFlagParId(2).actif=false;
			FlagManager.ChercherFlagParId(3).actif=false;
			FlagManager.ChercherFlagParId(4).actif=false;
			break;
		case(ControlCenter.Difficulte.Normale):
			ControlCenter.difficulteActuelle=diff;
			FlagManager.ChercherFlagParId(1).actif=false;
			FlagManager.ChercherFlagParId(2).actif=true;
			FlagManager.ChercherFlagParId(3).actif=false;
			FlagManager.ChercherFlagParId(4).actif=false;
			break;
		case(ControlCenter.Difficulte.Difficile):
			ControlCenter.difficulteActuelle=diff;
			FlagManager.ChercherFlagParId(1).actif=false;
			FlagManager.ChercherFlagParId(2).actif=false;
			FlagManager.ChercherFlagParId(3).actif=true;
			FlagManager.ChercherFlagParId(4).actif=false;
			break;
		case(ControlCenter.Difficulte.TresDifficile):
			ControlCenter.difficulteActuelle=diff;
			FlagManager.ChercherFlagParId(1).actif=false;
			FlagManager.ChercherFlagParId(2).actif=false;
			FlagManager.ChercherFlagParId(3).actif=false;
			FlagManager.ChercherFlagParId(4).actif=true;
			break;
		}
	}
}
