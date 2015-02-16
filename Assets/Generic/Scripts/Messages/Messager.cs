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


using SimpleJSON;
using System.IO;

public class Messager:MonoBehaviour
{
	public Texture2D iconeEnveloppe; // icone de l'enveloppe
	public string nomFichierMessages; // Nom du fichier dans lequel sont enregistrés les messages

	private List<Message> tousLesMessages; // Liste de tous les messages qui peuvent être envoyés dans le jeu
	private Queue<Message> fileMessages; // File des messages en attente de lecture par le joueur
	private GUIText ecran;
	private bool reading = false; // vérifie si un message est en cours de lecture

	void Start()
	{
		fileMessages = new Queue<Message> ();
		tousLesMessages = new List<Message> ();
		string contenu = ContenuFichierMessages ();
		JSONNode json = JSON.Parse ( contenu ); // parsing du json
		ChargerTousLesMessages (json); // Chargement des objets messages en mémoire

		ecran = gameObject.GetComponent<GUIText>();
	}

	void OnGUI() {
		if (fileMessages.Count != 0) { // s'il y a au moins un message dans la file
			GUI.Label( new Rect( Screen.width/2, Screen.height/10, 50, 50 ), iconeEnveloppe ); // Notification de l'arrivée d'un message au joueur
			if( !isReading() && Input.GetButtonDown("read") ) { // Si le bouton de lecture est appuyé
				Debug.Log("Lecture d'un message");
				reading = true;
				StartCoroutine("LireMessageSuivant");// Lecture du message
			}
		}

		// Test
		/*if (GUI.Button (new Rect (Screen.width / 3, Screen.height / 5, 300, 100), "Envoyer Message test")) {
			Evenement sendEvent = new global::EnvoyerMessage();
			Item message = new Item( NomItem.Message, "1" );
			sendEvent.DeclencherEvenement( message );
		}*/
	}

	private void ChargerTousLesMessages( JSONNode json ) {
		for( int i = 0; i < json["messages"].Count; i++ ) {
			Message message = new Message();
			JSONNode element = json["messages"][i];

			message.SetId( element["id"].Value );
			message.SetTexte( element["text"].Value );
			message.SetExpediteur( element["sender"].Value );
			message.SetDureeAffichage( element["displayTime"].AsFloat );

			tousLesMessages.Add( message );
		}
	}

	// Gère l'affichage des messages à l'écran
	private IEnumerator LireMessageSuivant() {
		Message message = fileMessages.Dequeue (); // Récupération du message en début de file
		string texteAAfficher = "Exp\u00e9diteur: " + message.GetExpediteur() + "\n" +
						"Texte: " + message.GetTexte();
		ecran.text = texteAAfficher;
		yield return new WaitForSeconds (message.GetDureeAffichage());

		ecran.text = "";
		reading = false; // Fin de la lecture du message
	}

	public void EnvoyerMessage( string idMessage ) {
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
			Debug.Log ("Message " +idMessage+ " non trouv\u00e9");
	}

	public void EnvoyerMessage( Message message ) {
		fileMessages.Enqueue ( message );
	}

	private string ContenuFichierMessages() {
		StreamReader sr = new StreamReader ( Application.dataPath + '/' + nomFichierMessages); // Flux du fichier
		Debug.Log (Application.dataPath + '/' + nomFichierMessages);
		string contenu = sr.ReadToEnd (); // Lecture du fichier jusqu'à sa fin
		return contenu;
	}
	

	public bool isReading() {
		return reading;
	}	
}


