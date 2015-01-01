/**
 * \file      AffichageTexteRaycast.cs
 * \author    
 * \version   1.0
 * \date      1 janvier 2015
 * \brief     Script indiquant que l'objet en question demande à afficher à l'écran un certain texte.
 */

using UnityEngine;
using System.Collections;

public class AffichageTexteRaycast : AffichageTexteEcran {
	public string texteAffiche="";

	public void ChangerTexte () {
		RemplacerTexteNonPrioritaire(texteAffiche);
	}
}
