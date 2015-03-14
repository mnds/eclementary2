﻿/**
 * \file      Dialogue.cs
 * \author    
 * \version   1.0
 * \date      5 novembre 2014
 * \brief     Contient toutes les répliques possibles d'un dialoque.
 *
 * \details   Se charge d'instancier toutes les répliques lors d'un dialogue,
 * 			  de créer les relations entre elles ainsi que de dérouler le dialogue
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SimpleJSON;
using System.IO;

public class Dialogue : MonoBehaviour, Interactif {

	public string nomFichierRepliques; // Nom du fichier contenant les répliques
	List<Replique> repliques = new List<Replique>(); // Liste de toute les répliques du dialogue
	Replique repliqueChoisie; // Réplique dernièrement sélectionnée par l'utilisateur
	List<Replique> repliquesSuivantes; // Réplique suivant repliqueChosie
	bool interactionDeclenchee = false; // Informe de l'etat du dialogue

	ControllerJoueur mouvementJoueur ; // Référence vers le script de déplacement du joueur

	public float distanceMinimaleInteraction = 4.0f; //La distance à laquelle on doit etre pour pouvoir interagir avec l'objet

	// Use this for initialization
	void Start () {
		string contenu = ContenuFichierRepliques ();
		JSONNode json = JSON.Parse ( contenu ); // parsing du json

		RemplirRepliques( json );
		LierRepliques( json );

		GameObject joueur = ControlCenter.GetJoueurPrincipal(); // Récupération du joueur principal
		if (joueur != null)
			mouvementJoueur = joueur.GetComponent<ControllerJoueur> ();

	}
	
	void OnGUI() {
		if( interactionDeclenchee ) {
			if (repliquesSuivantes == null || repliquesSuivantes.Count == 0) { // Arrêt de l'interaction si l'interaction choisie n'a pas de répliques suivantes
				if(repliquesSuivantes==null) Debug.Log ("Repliques suivantes nulles");
				else Debug.Log ("Pas de repliques suivantes");
				ArreterInteraction ();
			}
			else{ // Affichage des répliques si l'intéraction a été déclenché
				if( (repliquesSuivantes.Count > 0) && (repliquesSuivantes[0] != null) )
					AfficherInfosPersonnage( repliquesSuivantes[0] ); // Affichage de l'avatar et du nom du personnage parlant

				int y = 10;
				for (int i = 0; i < repliquesSuivantes.Count; i++) {
					if (repliquesSuivantes [i] != null) { // Test de l'existence en mémoire de la réplique, au cas où il y aurait une erreur dans le fichier des répliques
						// Calcul de la longueur du texte pour ajustement à l'affichage
						int longueurZoneTexte = repliquesSuivantes [i].GetTexte ().Length > 25 ? repliquesSuivantes [i].GetTexte ().Length*9 : repliquesSuivantes [i].GetTexte ().Length*10;
						if( longueurZoneTexte > Screen.width/2 )
							longueurZoneTexte = Mathf.FloorToInt( Screen.width/2 );
						GUIStyle guiStyle = new GUIStyle(GUI.skin.GetStyle("Button")); // Style des "boutons de dialogue"
						guiStyle.alignment = TextAnchor.UpperLeft; // Texte aligné à gauche
						guiStyle.fontSize = 18;

						if (GUI.Button (new Rect (100, y, longueurZoneTexte, 70), repliquesSuivantes [i].GetTexte (), guiStyle) ) {
							SelectionnerReplique (repliquesSuivantes [i]); // Sélection de la prochaine réplique
						}
						y += 75; // Incrémentation de y pour afficher la réplique soeur un cran en dessous
					} 
					else
						Debug.Log ("La réplique indiquée n'existe pas. Veuillez vérifier le contenu du fichier");
				}	
			}
		}
	}

	// Permet l'affichage d'infos sur le personnage prononçant la réplique (avatar, nom...)
	void AfficherInfosPersonnage( Replique replique ) {
		GUIStyle guiStyle = new GUIStyle(GUI.skin.GetStyle("Button")); // Style des "boutons de dialogue"
		guiStyle.alignment = TextAnchor.UpperLeft; // Texte aligné à gauche
		guiStyle.fontSize = 18;

		// Affichage de l'avatar du joueur
		Personnage perso = (Personnage)replique.GetGoAssocie().GetComponent<Personnage>();
		if( perso != null )
			GUI.Label( new Rect(0, 10, 100, 80), perso.avatar );
		// Affichage du nom du joueur
		GUI.Label ( new Rect(0, 90, 100, 30), replique.GetGoAssocie().name, guiStyle); 
	}

	// Effectue le changement de la réplique actuelle
	void SelectionnerReplique( Replique repliqueSelectionnee ) {
		repliqueChoisie = repliqueSelectionnee;
		//Activation des flags
		foreach(int idFlag in repliqueChoisie.GetFlagsActives ()) {
			if(FlagManager.ActiverFlag (idFlag))
				Debug.Log ("Activation du flag "+idFlag+" : "+FlagManager.ChercherFlagParId (idFlag).description);
			else
				Debug.Log ("Impossible d'activer le flag "+idFlag);
		}
		//On récupère les répliques suivantes APRES avoir activé les flags
		repliquesSuivantes = repliqueSelectionnee.GetRepliquesSuivantes ();
		if(repliquesSuivantes.Count==0) Debug.Log ("Pas de réplique suivante");

	}

	/* Renseigne les champs des objets Repliques
	 * json: noeud json issu de la lecture de nomFichierRepliques
	 */
	void RemplirRepliques( JSONNode json ) {
		for( int i = 0; i < json["repliques"].Count; i++ ) {
			Replique replique = new Replique();
			replique.SetId( json["repliques"][i]["id"].AsInt ); // Conversion de l'id en int avant remplissage du champ
			replique.SetText( json["repliques"][i]["texte"].Value );

			// Association avec le gameObject qui prononcera la réplique
			string nomGoAssocie = json["repliques"][i]["goAssocie"].Value;
			if( nomGoAssocie.CompareTo("") != 0 ){
				GameObject go = GameObject.Find ( nomGoAssocie );
				if( go )
					replique.SetGoAssocie( go );
				else
					Debug.Log ("Le gameObject " + nomGoAssocie + "n'a pu être trouvé");
			}

			replique.SetFlagsRequis( json["repliques"][i]["flagsRequis"] );
			replique.SetFlagsBloquants( json["repliques"][i]["flagsBloquants"] );
			replique.SetFlagsActives( json["repliques"][i]["flagsActives"] );

			repliques.Add( replique ); // Ajout à la liste des répliques gérées par le dialogue
		}
	}

	/* Permet de lier les répliques entre elles, de façon à constituer "l'arbre du dialogue"
	 * json: noeud json issu de la lecture de nomFichierRepliques
	 */
	void LierRepliques( JSONNode json ) {
		for( int i = 0; i < json["repliques"].Count; i++) {
			if( json["repliques"][i]["repSuivantes"] != null ) { // si la réplique est suivie d'au moins une réplique
				for( int j = 0; j < json["repliques"][i]["repSuivantes"].Count; j++ ) { // parcours des répliques suivant la réplique courante
					Replique repliqueSuivante = TrouverReplique( json["repliques"][i]["repSuivantes"][j].AsInt );
					if( repliqueSuivante != null )
						repliques[i].AjouterRepliqueSuivante( repliqueSuivante ); // Ajout à la liste des répliques suivantes
				}
			}
		}
	}

	/*
	 * Cherche la réplique identifiée par son id
	 * id: id de la réplique cherchée
	 * @return: retourne l'adresse de la réplique cherchée, ou null si elle n'a pas été trouvée
	 */
	Replique TrouverReplique( int id ) {
		bool trouve = false;
		int i = 0;
		Replique repliqueCherchee = null;

		while (!trouve && (i < repliques.Count)) { // on boucle tant qu'on n'a pas trouvé la réplique correspondante, dans les limites de la liste
			trouve = (repliques[i].GetId() == id);
			i++;		
		}

		if (trouve) // Mise à jour de repliqueCherchee, si la réplique en question a été trouvée
			repliqueCherchee = repliques [i - 1]; // i sort de la boucle avec la position de la réplique incrémentée, d'où la décrémentation

		return repliqueCherchee;
	}

	// Renvoie le contenu du fichier des repliques
	string ContenuFichierRepliques() {
		StreamReader sr = new StreamReader (Application.dataPath + "/" +nomFichierRepliques); // Flux du fichier
		string contenu = sr.ReadToEnd (); // Lecture du fichier jusqu'à sa fin
		return contenu;
	}


	// _______________________________REALISATION DE L'INTERFACE________________________
	public void DemarrerInteraction() {

		// Mise à jour de la variable d'état des interactions
		new MajEtatInteraction (true).DeclencherEvenement ();

		if (mouvementJoueur != null) { // Blocage des mouvements du joueur pendant le déroulement de l'interaction
			mouvementJoueur.SetFreeze( true );
		} 
		else
			Debug.Log ("Mouvements du joueur non accessibles");
		new DesactiverInventaire().DeclencherEvenement(); // Désactivation de l'inventaire
		// Démarrage du dialogue
		repliqueChoisie = repliques[0]; // Pointage de repliquesActuelles à la tête de repliques (qui est une replique sans contenu)
		repliquesSuivantes = repliqueChoisie.GetRepliquesSuivantes ();
		Debug.Log ("Replique choisie : "+repliqueChoisie.GetTexte());
		interactionDeclenchee = true;
	}
	
	public void ArreterInteraction() {
		// Mise à jour de la variable d'état des interactions
		new MajEtatInteraction (false).DeclencherEvenement ();
		Debug.Log ("Fin du dialogue");
		repliqueChoisie = null;
		repliquesSuivantes = null;
		
		interactionDeclenchee = false;

		if (mouvementJoueur != null) {// Déblocage des mouvements du joueur
			mouvementJoueur.SetFreeze( false ); // Déblocage des mouvements du joueur
		}
		new ActiverInventaire ().DeclencherEvenement (); // Réactivation de l'inventaire
	}

	//Si le dialogue est désactivé alors qu'on a une interaction enclenché, ça va etre tres problématique.
	public void OnDisable () {
		if(interactionDeclenchee)
			ArreterInteraction();
	}

	public void SetDistanceMinimaleInteraction (float distanceMinimaleInteraction_) {
		distanceMinimaleInteraction = distanceMinimaleInteraction_;
	}
	
	public float GetDistanceMinimaleInteraction () {
		return distanceMinimaleInteraction;
	}
}
