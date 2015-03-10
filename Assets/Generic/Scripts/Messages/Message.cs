/**
 * \file      Message.cs
 * \author    
 * \version   1.0
 * \date      11 février 2015
 * \brief     Encapsule un message à afficher à l'écran
 */

using UnityEngine;

[System.Serializable]
public class Message
{
	private string id;
	private string texte; // Texte du message
	private string expediteur; // Auteur du message
	private float dureeAffichage; // Durée d'affichage du texte à l'écran

	public Message() {

	}

	public Message ( string idMessage, string text, string sender,float displayTime = 5 )
	{
		SetId (idMessage);
		SetTexte (text);
		SetExpediteur(sender);
		SetDureeAffichage (displayTime);
	}

	public string GetId() {
		return id;
	}

	public void SetId( string idMessage ) {
		id = idMessage;
	}

	public string GetTexte() {
		return texte;
	}

	public void SetTexte( string text ) {
		texte = text;
	}

	public string GetExpediteur() {
		return expediteur;
	}

	public void SetExpediteur( string sender ) {
		expediteur = sender;
	}

	public float GetDureeAffichage() {
		return dureeAffichage;
	}

	public void SetDureeAffichage( float displayTime ) {
		dureeAffichage = displayTime;
	}
}


