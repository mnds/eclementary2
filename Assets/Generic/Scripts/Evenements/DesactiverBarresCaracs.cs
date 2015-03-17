/**
 * \file      DesactiverBarresCaracs.cs
 * \author    
 * \version   1.0
 * \date      17 Mars 2015
 * \brief     Désactive les barres de vies pour l'écran titre
 *
 * \details   
 */

using UnityEngine;

public class DesactiverBarresCaracs : Evenement {


	public override void DeclencherEvenement( params Item[] items ) {
		GameObject barres = GameObject.Find ("Barres");
		GestionBarre scriptCaracs;
		if(barres)
		{
			scriptCaracs = barres.GetComponent<GestionBarre>();
			scriptCaracs.setEnabled(false);
		}
	}
}
