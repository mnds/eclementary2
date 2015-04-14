/**
 * \file      GameData.cs
 * \author    
 * \version   1.0
 * \date      14 décembre 2014
 * \brief     Stocke les données sur le joueur et son avancement actuel
 */

using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class GameData {

	private float pointsDeVieSauves;
	private int pointsExperience;
	private ControlCenter.Difficulte difficulte;
	private int niveau;
	private string nomSpawnPointSauve;
	private int idScenePrecedente;
	private string nomEtatSauve;
	private List<FlagReduit> flagsReduits;
	private List<int> listeMunitions; //Match listeObjetsRecoltables. On doit en garder une trace pour d'éventuels changements de scène ou autres traitements
	private string idMessageReveil; // Message affiché lorsque le joueur charge la partie, pour le diriger dans sa quête

	public string IdMessageReveil {
		get { return idMessageReveil; }
		set { idMessageReveil = value; }
	}

	public float PointsDeVieSauves {
		get { return pointsDeVieSauves; }
		set { pointsDeVieSauves = value; }
	}

	public int PointsExperience {
		get { return pointsExperience; }
		set { pointsExperience = value; }
	}

	public ControlCenter.Difficulte Difficulte {
		get { return difficulte; }
		set { difficulte = value; }
	}

	public string NomSpawnPointSauve {
		get { return nomSpawnPointSauve; }
		set { nomSpawnPointSauve = value; }
	}

	public int IdScenePrecedente {
		get { return idScenePrecedente; }
		set { idScenePrecedente = value; }
	}

	public string NomEtatSauve {
		get { return nomEtatSauve; }
		set { nomEtatSauve = value; }
	}

	public List<FlagReduit> FlagsReduits {
		get { return flagsReduits; }
		set { flagsReduits = value; }
	}

	public List<int> ListeMunitions {
		get { return listeMunitions; }
		set { listeMunitions = value; }
	}

	public int Niveau {
		get { return niveau; }
		set { niveau = value;}
	}

	private void ChargerFlagsReduits() {
		List<FlagManager.Flag> flags = FlagManager.GetFlags();
		flagsReduits = FlagManager.Flag.toFlagsReduits( flags );
	}

	private void ChargerListeMunitions() {
		GameObject joueur = ControlCenter.GetJoueurPrincipal();
		Inventaire inventaire = joueur.GetComponent<Inventaire> ();
		if (inventaire != null)
			listeMunitions = inventaire.quantiteObjets;
		else
			Debug.Log ("Problème lors du chargement de la liste des munitions");
	}

	// Méthode qui enregistre le gameData dans le fichier gamedata.bin
	public void SauvegarderGameData() {
		GameObject joueur = ControlCenter.GetJoueurPrincipal();
		HealthPlayer healthPlayer = joueur.GetComponent<HealthPlayer> ();
		Caracteristiques carac = joueur.GetComponent<Caracteristiques> ();
		Inventaire inventaire = joueur.GetComponent<Inventaire> ();

		if (healthPlayer != null)
			this.PointsDeVieSauves = healthPlayer.GetPointsDeVieActuels ();
		if (carac != null)
			this.PointsExperience = carac.experience;
		ChargerFlagsReduits ();
		ChargerListeMunitions ();
		this.Difficulte = ControlCenter.difficulteActuelle;
		this.NomSpawnPointSauve = ControlCenter.GetNomSpawnPointActuel ();
		this.IdScenePrecedente = ControlCenter.idScenePrecedente;
		this.NomEtatSauve = StateManager.getInstance ().etatActif.GetType ().Name;
		this.Niveau = carac.niveau;
		
		FileStream stream = File.Create( ControlCenter.nomFichierSauvegarde );
		BinaryFormatter formatter = new BinaryFormatter();
		Debug.Log( "Sérialisation du gameData" );
		formatter.Serialize( stream, this );
		stream.Close();
	}

	// Charge les valeurs sauvegardées et met à jour les objets concernés
	public void ChargerSauvegarde() {
		if (!File.Exists (ControlCenter.nomFichierSauvegarde)) { // Vérification de l'existence du fichier
			Debug.Log ("Aucune sauvegarde trouvée");
			return;
		}

		FileStream stream = File.OpenRead( ControlCenter.nomFichierSauvegarde ); // Ouverture du fichier
		BinaryFormatter formatter = new BinaryFormatter();
		Debug.Log ("Desérialisation du gameData");
		GameData gameData = (GameData)formatter.Deserialize( stream ); // Désérialisation de l'objet
		stream.Close(); /// Fermeture du fichier

		GameObject joueur = ControlCenter.GetJoueurPrincipal();
		HealthPlayer healthPlayer = joueur.GetComponent<HealthPlayer> ();
		Caracteristiques carac = joueur.GetComponent<Caracteristiques> ();
		Inventaire inventaire = joueur.GetComponent<Inventaire> ();
		
		// Remplacement des caractéristiques
		if( healthPlayer != null )
			healthPlayer.SetPointsDeVie ( gameData.PointsDeVieSauves );
		if (carac != null) {
			carac.experience = gameData.PointsExperience;
			carac.niveau = gameData.Niveau;
		}
		if (inventaire != null) {
			inventaire.quantiteObjets = gameData.ListeMunitions;
			inventaire.InitialiserObjets();
		}
		ControlCenter.difficulteActuelle = gameData.Difficulte;
		ControlCenter.SetNomSpawnPointActuel ( gameData.NomSpawnPointSauve );
		ControlCenter.idScenePrecedente = gameData.IdScenePrecedente;
		RemplacerFlags ( gameData );
		ChargerEtat( gameData.NomEtatSauve ); // Chargement de l'état auquel le joueur avait fait sa sauvegarde
		new EnvoyerMessage (new Item (NomItem.Message, gameData.IdMessageReveil)).DeclencherEvenement (); // Affichage du dernier message affiché

		/*Debug.Log ("Points de vie chargés" + gameData.PointsDeVieSauves );
		Debug.Log ("Points xp:" + gameData.PointsExperience);
		Debug.Log ("Difficulte: " + gameData.Difficulte);
		Debug.Log ("nom spawnpoint: " + gameData.NomSpawnPointSauve);
		Debug.Log ("id scene prec: "+ gameData.idScenePrecedente);
		Debug.Log ("nom etat sauvé: "+ gameData.NomEtatSauve);
		Debug.Log ("flags reduits:"+ gameData.FlagsReduits);
		Debug.Log ("liste munitions: "+ gameData.ListeMunitions);*/
	}


	private void RemplacerFlags( GameData gameData ) {
		List<FlagManager.Flag> flags = FlagManager.GetFlags();
		for( int i = 0; i < flags.Count; i++ ) {
			if( flags[i].id == gameData.FlagsReduits[i].id )
				flags[i].actif = gameData.FlagsReduits[i].actif;
			else
				Debug.Log ("Id flags non correspondants");
		}
	}

	private void ChargerEtat( string nomEtatSauve ) {
		Type typeEtat = Type.GetType( nomEtatSauve ); // récupération du type de l'état

		if (typeEtat != null) {
			System.Object[] arguments = { StateManager.getInstance() }; // argument à donner au constructeur
			Etat etatSauve = (Etat)Activator.CreateInstance( typeEtat, arguments ); // Création d'une instance de l'état auquel le joueur avait fait sa sauvegarde
			StateManager.getInstance().BasculerEtat( etatSauve );
			Debug.Log ("Nom de la scène de l'état créé par réflexion: " + etatSauve.getSceneCorrespondante());
		} else
			Debug.Log ("Type de l'état non trouvé");
	}

}
