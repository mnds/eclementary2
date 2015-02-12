/**
 * \file      EnvoyerMessage.cs
 * \author    
 * \version   1.0
 * \date      12 février 2015
 * \brief     Envoie un message au joueur
 */

using UnityEngine;

public class EnvoyerMessage : Evenement
{
	public override void DeclencherEvenement (params Item[] items)
	{
		Messager messager = ControlCenter.GetMessager ();

		for (int i = 0; i < items.Length; i++) {
			if( items[i].GetNom().Equals( NomItem.Message ) )
				messager.EnvoyerMessage( items[i].GetNomItem() ); // nomItem est à un item ce que l'id est à un message
		}
	}

}


