/**
 * \file      Messager.cs
 * \author    
 * \version   1.0
 * \date      11 février 2015
 * \brief     Gère l'affichage des messages envoyés au joueur pour le guider tout au long du jeu
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Messager:MonoBehaviour
{
	public Texture2D iconeEnveloppe; // icone de l'enveloppe
	private List<Message> tousLesMessages; // Liste de tous les messages qui peuvent être envoyés dans le jeu
	private Queue<Message> fileMessages; // File des messages en attente de lecture par le joueur
	private GUIText ecran;

	void Start()
	{
		ecran = gameObject.GetComponent<GUIText>();
		fileMessages = new Queue<Message> ();
		fileMessages.Enqueue ( new Message( "YOo", "Ceci est un message test", "boss", 5.0f ) );
	}

	void OnGUI() {
		if (fileMessages.Count != 0) { // s'il y a au moins un message dans la file
			GUI.Label( new Rect( Screen.width/2, Screen.height/10, 100, 100 ), iconeEnveloppe ); // Notification de l'arrivée d'un message au joueur
			if( Input.GetKeyDown("l") ) { // Si le bouton de lecture est appuyé
				Debug.Log("Lecture d'un message");
				StartCoroutine("LireMessageSuivant");// Lecture du message
			}
		}
	}

	// Gère l'affichage des messages à l'écran
	public IEnumerator LireMessageSuivant() {
		Message message = fileMessages.Dequeue (); // Récupération du message en début de file
		string texteAAfficher = "Expéditeur: " + message.GetExpediteur() + "\n" +
						"Texte: " + message.GetTexte();
		ecran.text = texteAAfficher;
		yield return new WaitForSeconds (message.GetDureeAffichage());

		ecran.text = "";
	}

	public void EnvoyerMessage( int idMessage ) {
		bool found = false;
		int i = 0;
		// Recherche du message avec l'id donné en paramètre
		while ( i < tousLesMessages.Count && !found) {
			found = tousLesMessages[i].GetId().Equals( idMessage );
			i++;
		}

		if (found) // Envoi du message s'il a été trouvé
			EnvoyerMessage (tousLesMessages [i - 1]);
		else
			Debug.Log ("Message " +idMessage+ " non trouvé");
	}

	public void EnvoyerMessage( Message message ) {
		fileMessages.Enqueue ( message );
	}
}


