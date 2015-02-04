/**
 * \file      ControlCenter.cs
 * \author    
 * \version   1.0
 * \date      21 janvier 2015
 * \brief     Contient des variables globales liées au bon déroulement du jeu.
 *
 * \details   Quand une cinématique est lancée, cinematiqueEnCours passe à true. Les appels à getCEC renvoient true et permettent d'empecher des actions de se produire.
 * 			  ControlCenter contient toutes les valeurs qui peuvent etre amenées à changer dans les vues Hierarchy (nom du Joueur, des scènes...)
 */

/*
 * Utilisé dans Inventaire, MoveCamera , Attaquer, Lancer , SetJoueurPrincipal
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

static public class ControlCenter {
	public enum Difficulte {
		Facile,
		Normale,
		Difficile,
		TresDifficile
	}

	public enum Scenes { //En donner le nom explicite plus bas. Dans StateManager, ajouter dans ChargerEtatSelonScene l'etat correspondant.
		Titre,
		Mort,
		Campus,
		LIRIS,
		Chambre,
		ChambreGarsFoyer,
		ClubBD,
		Gymnase,
		Scolarite,
		BureauDebouck,
		Labyrinthe,
		Amphi2,
		Fin
	}

	[System.Serializable]
	public class ObjetActivableSelonFlags {
		public ActivationSelonFlags asf;
		public string nomScene;
		Scenes machin;
		public ObjetActivableSelonFlags(ActivationSelonFlags asf_, string ns) {
			asf=asf_;
			nomScene=ns;
		}
	}

	//Variables globales permettant de définir les noms de certains éléments du jeu amenés à changer
	static public string nomDuJoueurPrincipal = "Joueur";
	static public string nomDeLaSceneDepart = "Ecran titre";
	static public string nomDeLaSceneDuCampus = "Campus";
	static public string nomDeLaSceneDeMort = "Ecran Mort";
	static public string nomDeLaSceneLIRIS = "LaboLIRIS";
	static public string nomDeLaSceneDeLaChambre = "Chambre";
	static public string nomDeLaSceneDeLaChambreGarsFoyer = "ChambreGarsFoyer";
	static public string nomDeLaSceneDuClubBD = "ClubBD";
	static public string nomDeLaSceneDuGymnase = "Gymnase";
	static public string nomDeLaSceneScolarite = "Scolarite";
	static public string nomDeLaSceneDebouck = "BureauDebouck";
	static public string nomDeLaSceneLabyrinthe = "Labyrinthe";
	static public string nomDeLaSceneAmphi2 = "Amphi2";
	static public string nomDeLaSceneDeFin = "Ecran Fin";


	static bool cinematiqueEnCours = false; //Les interactions doivent s'arreter si on est en cinématique
	static bool afficherBarreDeVieJoueur = true;
	static bool afficherBarreEnduranceJoueur = true;
	static private bool abdvjAncien; //Pour les changements
	static private bool abejAncien;
	static private bool utiliserOculus = false; //Dans FPCClassic
	static private bool pause = false;
	static public bool inventaireOuvert = false;

	static GameObject joueurPrincipal;
	static Camera cameraPrincipale;

	static GUIText texteInteraction; //Texte affiché à l'écran
	static bool texteEstPrioritaire; //Le texte vient d'une interaction ou d'un raycast ? Si raycast, le supprimer si rien en raycast, sinon attendre le cooldown.
	static public float tempsAffichageTexteInteraction = 1f;

	//Garde en mémoire les scènes
	static int idScenePrecedente;
	static public ModesJeu mode = ModesJeu.Normal; //Mode Debug ou Normal. Utilisé dans DebugManager.
	static public string nomSpawnPointActuel;

	//Recupération de tous les objets qui peuvent s'activer / se désactiver en fonction des flags
	static public List<ObjetActivableSelonFlags> lesOASFs = new List<ObjetActivableSelonFlags>(){};
	
	//Parametres
	static public Difficulte difficulteActuelle = Difficulte.Normale;

	static ControlCenter () {
		joueurPrincipal = GameObject.Find ("Joueur"); //au cas où aucun gameObject n'ait déclaré au ControlCenter qu'il est JoueurPrincipal
		texteInteraction = GameObject.Find ("Texte").GetComponent<GUIText>(); //au cas où aucun gO n'ait déclaré au CC qu'il est le texte sur lequel afficher les messages
	}

	static public void SetTexte (GUIText texte) { //Appelé dans SetTexte, présent dans le prefab Texte de Interface
		texteInteraction = texte;
	}

	static public GUIText GetTexte () {
		return texteInteraction;
	}

	static public bool GetAfficherBarreDeVieJoueur () {
		return afficherBarreDeVieJoueur;
	}

	static public bool GetAfficherBarreEnduranceJoueur () {
		return afficherBarreEnduranceJoueur;
	}

	static public bool GetCinematiqueEnCours () {
		return cinematiqueEnCours;
	}

	static public void SetCinematiqueEnCours (bool cec) {
		Debug.Log ("Control Center : cinematique "+cec);
		cinematiqueEnCours = cec;
		//L'affichage dépend de cinematique
		if(cec) {
			abdvjAncien = afficherBarreDeVieJoueur;
			abejAncien = afficherBarreEnduranceJoueur;
			afficherBarreDeVieJoueur = cec;
			afficherBarreEnduranceJoueur = cec;
		}
		else
		{
			afficherBarreDeVieJoueur = abdvjAncien;
			afficherBarreEnduranceJoueur = abejAncien;
		}


	}

	static public void SetAfficherBarreDeVieJoueur (bool abdvj) {
		Debug.Log ("Control Center : affichage vie joueur "+abdvj);
		afficherBarreDeVieJoueur = abdvj;
	}

	static public void SetAfficherBarreEnduranceJoueur (bool abdej) {
		Debug.Log ("Control Center : affichage stamina joueur "+abdej);
		afficherBarreEnduranceJoueur = abdej;
	}

	static public void SetJoueurPrincipal (GameObject go) {
		joueurPrincipal = go;
	}

	static public GameObject GetJoueurPrincipal () {
		return joueurPrincipal;
	}

	static public void SetCameraPrincipale (Camera cp) {
		cameraPrincipale = cp;
	}
	
	static public Camera GetCameraPrincipale () {
		return cameraPrincipale;
	}

	static public void SetUtiliserOculus (bool utiliserOculus_) {
		utiliserOculus = utiliserOculus_;
	}

	static public bool GetUtiliserOculus () {
		return utiliserOculus;
	}

	static public void SetPause (bool pause_) {
		pause=pause_;
		if(pause) //Le control center se charge d'arreter le temps
			Time.timeScale=0;
		else
			Time.timeScale=1f;
	}

	static public bool GetPause () {
		return pause;
	}

	static public void SetTexteEstPrioritaire (bool texteEstPrioritaire_) {
		texteEstPrioritaire=texteEstPrioritaire_;
	}
	
	static public bool GetTexteEstPrioritaire () {
		return texteEstPrioritaire;
	}

	static public void SetNomSpawnPointActuel (string nomSpawnPointActuel_) {
		Debug.Log ("Nouveau point de spawn : "+nomSpawnPointActuel_);
		nomSpawnPointActuel=nomSpawnPointActuel_;
	}
	
	static public string GetNomSpawnPointActuel () {
		return nomSpawnPointActuel;
	}


  ///Méthodes liées à l'activation d'objets dans le décor selon l'état des flags

	/**
	 * @brief Ajout d'un objet dont l'activation dépend des flags.
	 * @param asf Le script qui détermine si l'objet doit etre active ou nom
	 * @param nomScene_ Le nom de la scene dans laquelle se trouve l'objet
	 **/
	static public void AddObjetActivableSelonFlags(ActivationSelonFlags asf, string nomScene_) {
		lesOASFs.Add(new ObjetActivableSelonFlags(asf,nomScene_));
	}

	/**
	 * @brief Enlève tous les oasf qui ne sont pas dans la bonne scène donc qu'on ne doit pas étudier, et on vérifie si les objets correspondants doivent se désactiver ou pas.
	 * @details Appelée notamment lorsqu'un flag est activé/désactivé.
	 **/
	static public void VerifierLesOASFs () {
		//Debug.Log ("Vérification des OASF");
		string nomSceneActuelle = Application.loadedLevelName;
		List<ObjetActivableSelonFlags> resultat = new List<ObjetActivableSelonFlags>(){};
		//On vérifie quels oasf ne sont pas dans la scène considérée
		foreach(ObjetActivableSelonFlags oasf in lesOASFs) {
			if(oasf.nomScene==nomSceneActuelle) { //Si la bonne scène
				resultat.Add (oasf);
				oasf.asf.Verifier(); //On vérifie ce asf
			}
		}
		//On remplace la liste par la nouvelle
		lesOASFs=resultat;
	}

	/**
	 * @brief Formule de dégat
	 * @param degatsDeBase Degats initiaux à infliger
	 * @param caracAttaquant Script Caracteristiques de l'attaquant, par défaut à null
	 * @param caracDefenseur Script Caracteristiques de l'attaquant, par défaut à null
	 * @return Degats subis
	 **/
	static public float FormuleDeDegats(float degatsDeBase, Caracteristiques caracAttaquant=null, Caracteristiques caracDefenseur=null) {
		float caracAttaque = 0; 
		if(caracAttaquant)
			caracAttaque = caracAttaquant.GetAttaque();
		float caracDefense = 0;
		caracDefense = caracDefenseur.GetDefense ();
		return Mathf.Max(0,(degatsDeBase)*(1f-(caracDefense-caracAttaque)/100f));
	}
}
