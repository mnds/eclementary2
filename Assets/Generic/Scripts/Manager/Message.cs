/**
 * \file      Message.cs
 * \author    
 * \version   1.0
 * \date      11 février 2015
 * \brief     Encapsule un message à afficher à l'écran
 */

using UnityEngine;

public class Message
{
	private int id;
	private string texte; // Texte du message
	private float dureeAffichage; // Durée d'affichage du texte à l'écran

	public Message ( int idMessage, string text, float displayTime = 5 )
	{
		id = idMessage;
		texte = text;
		dureeAffichage = displayTime;
	}

	public int GetId() {
		return id;
	}

	public void SetId( int idMessage ) {
		id = idMessage;
	}

	public string GetTexte() {
		return texte;
	}

	public void SetTexte( string text ) {
		texte = text;
	}
}


