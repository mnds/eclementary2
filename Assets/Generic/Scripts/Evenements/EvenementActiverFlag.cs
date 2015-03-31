/**
 * \file      EvenementActiverFlag.cs
 * \author    
 * \version   1.0
 * \date      31 Mars 2015
 * \brief     Active les flags donnés en paramètres
 *
 * \details   
 */
using System;
using UnityEngine;

public class EvenementActiverFlag:Evenement
{
	Item[] tabItems;

	public EvenementActiverFlag( params Item[] items ) {
		tabItems = items;
	}

	public override void DeclencherEvenement( params Item[] items ) {
		foreach(Item flag in tabItems) {
			if( flag.GetNom() == NomItem.Flag ) {
				FlagManager.Flag flagCorrespondant = FlagManager.ChercherFlagParId( Convert.ToInt32( flag.GetNomItem() ) );
				FlagManager.ActiverFlag(flagCorrespondant.id);
			}

		}
	}
}

