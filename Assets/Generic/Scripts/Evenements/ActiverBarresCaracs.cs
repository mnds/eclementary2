/**
 * \file      ActiverBarresCaracs.cs
 * \author    
 * \version   1.0
 * \date      17 Mars 2015
 * \brief     Active les barres de vies pour les états jouables
 *
 * \details   
 */

using UnityEngine;
using System.Collections;

public class ActiverBarresCaracs : Evenement {
	
	
	public override void DeclencherEvenement( params Item[] items ) {
		GameObject barres = GameObject.Find ("Barres");
		GestionBarre scriptCaracs;
		if(barres)
		{
			scriptCaracs = barres.GetComponent<GestionBarre>();
			scriptCaracs.setEnabled(true);
		}
	}
}
