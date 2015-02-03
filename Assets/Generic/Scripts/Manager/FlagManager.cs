/**
 * \file      FlagManager.cs
 * \author    
 * \version   1.0
 * \date      11 decembre 2014
 * \brief     Contient les details des evenements du scenario et les lie entre eux pour qu'ils s'activent à certains moments.
 * 
 * \details	  Les flags sont crees dans ce script manuellement. Une methode renvoie l'etat d'un flag une fois qu'on a tente de l'activer.
 * 			  L'activation et la verification des conditions associees sont faites dans ce script.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

static public class FlagManager {
	[System.Serializable]
	public class Flag {
		public int id;
		public bool actif;
		public string description;
		public List<int> predecesseurs; //Pour verifier si un flag est bien activable
		public List<int> bloquants; //Pour verifier si un flag est bien activable
		public List<Evenement> evenementsDeclenches;

		//Le flag actifDebut dit si le flag est dejà actif au debut. Si celui-ci est à true, les evenements lies au flag ne seront pas declenches au demarrage du logiciel.
		public Flag(int id_,bool actifDebut, string description_, List<int> predecesseurs_, List<int> bloquants_,List<Evenement> evenements_) {
			id=id_;
			actif=actifDebut;
			description=description_;
			predecesseurs=predecesseurs_;
			bloquants=bloquants_;
			evenementsDeclenches=evenements_;
		}

		public Flag(int id_,bool actifDebut, string description_) {
			id=id_;
			actif=actifDebut;
			description=description_;
			predecesseurs=new List<int>(){};
			bloquants=new List<int>(){};
			evenementsDeclenches=new List<Evenement>(){};
		}

		public Flag(int id_,bool actifDebut, string description_, List<Evenement> evenements_) {
			id=id_;
			actif=actifDebut;
			description=description_;
			predecesseurs=new List<int>(){};
			bloquants=new List<int>(){};
			evenementsDeclenches=evenements_;
		}

		public Flag(int id_,bool actifDebut, string description_, List<int> predecesseurs_,List<Evenement> evenements_) {
			id=id_;
			actif=actifDebut;
			description=description_;
			predecesseurs=predecesseurs_;
			bloquants=new List<int>(){};
			evenementsDeclenches=evenements_;
		}

		public Flag(int id_,bool actifDebut, string description_, List<int> predecesseurs_, List<int> bloquants_) {
			id=id_;
			actif=actifDebut;
			description=description_;
			predecesseurs=predecesseurs_;
			bloquants=bloquants_;
			evenementsDeclenches=new List<Evenement>(){};
		}

		public Flag(int id_,bool actifDebut, string description_, List<int> predecesseurs_) {
			id=id_;
			actif=actifDebut;
			description=description_;
			predecesseurs=predecesseurs_;
			bloquants=new List<int>(){};
			evenementsDeclenches=new List<Evenement>(){};
		}
	}
	

	static private List<Flag> flags = new List<Flag>{};

	static FlagManager () {
		//if(ControlCenter.DebutDuJeu)
		RemplirDefaut();
	}

	/**
	 * @brief Remplissage par defaut des flags. Seul le flag 0, d'initialisation, est actif.
	 */
	static private void RemplirDefaut () {
		//Redaction des differents flags
		/* Version de test pour l'edition decembre 2014.
		 * On commence sans rien, on va chercher le numerisator sur le campus
		 * On va appuyer sur un bouton près de la boule pour l'enclencher et il faut la tuer.
		 * Une fois la boule tuee, elle donne un objet qui declenche la fin du jeu.
		 */
		/*flags.Add (new Flag (0, true, "DebutDuJeu", null));
		flags.Add (new Flag (1, false, "PremierBouton", new List<int> (){0}));
		flags.Add (new Flag (2, false, "DeuxiemeBouton", new List<int> (){0}));
		flags.Add (new Flag (3, false, "TroisiemeBouton", new List<int> (){0}));
		flags.Add (new Flag(4,false,"Activation de la BOULE", new List<int> () {1,2,3}));
		flags.Add (new Flag(5,false,"Mort de la boule",new List<int>() {4}));*/

		//Flags de difficulté du jeu
		flags.Add (new Flag(1,false,"Difficulté facile",new List<Evenement>(){new ChangerSpawnPoint(new Item(NomItem.NomDifficulte,ControlCenter.Difficulte.Facile))}));
		flags.Add (new Flag(2,false,"Difficulté normale",new List<Evenement>(){new ChangerSpawnPoint(new Item(NomItem.NomDifficulte,ControlCenter.Difficulte.Normale))}));
		flags.Add (new Flag(3,false,"Difficulté difficile",new List<Evenement>(){new ChangerSpawnPoint(new Item(NomItem.NomDifficulte,ControlCenter.Difficulte.Difficile))}));
		flags.Add (new Flag(4,false,"Difficulté très difficile",new List<Evenement>(){new ChangerSpawnPoint(new Item(NomItem.NomDifficulte,ControlCenter.Difficulte.TresDifficile))}));
		//Flags du scenario
		flags.Add (new Flag(10,false,"Debut du jeu",new List<int>(){},new List<Evenement>(){new ChangerSpawnPoint(new Item(NomItem.SpawnPoint,"SpawnPointPresWeiman",ControlCenter.nomDeLaSceneDuCampus,true))}));
		flags.Add (new Flag(20,false,"Thermobus affiche",new List<int>(){10},new List<Evenement>(){})); //Activé en parlant au Weiman
		flags.Add (new Flag(30,false,"Bus choisi",new List<int>(){20},new List<Evenement>(){})); //Activé en choisissant un bus
		flags.Add (new Flag(40,false,"Retour du WEI",new List<int>(){30},new List<Evenement>(){new ChangerSpawnPoint(new Item(NomItem.SpawnPoint,"SpawnPointMilieuTerrain",ControlCenter.nomDeLaSceneDuCampus,true))})); //Activé en parlant de nouveau au Weiman
		flags.Add (new Flag(42,false,"Choix du batiment",new List<int>(){40},new List<int>(){42})); //Activé en rentrant à Adoma
		flags.Add (new Flag(44,false,"Lit apres le WEI",new List<int>(){42},new List<int>(){44})); //Activé en étudiant le lit
		flags.Add (new Flag(50,false,"Papier du cadavre obtenu",new List<int>(){44},new List<int>(){50},new List<Evenement>(){}));
		flags.Add (new Flag(60,false,"Discussion avec le personnage devant le foyer",new List<int>(){44},new List<Evenement>(){})); //Activé en parlant au GarsFoyer
		flags.Add (new Flag(70,false,"Recuperation de la belle boite",new List<int>(){60},new List<Evenement>(){})); //Activé en allant dans la chambreFoyer (Comparat)
		flags.Add (new Flag(80,false,"Recuperation du plan de Centrale",new List<int>(){60},new List<Evenement>(){})); //Activé en allant dans la chambreFoyer
		flags.Add (new Flag(90,false,"Recuperation du message du personnage du foyer dans sa chambre",new List<int>(){60},new List<Evenement>(){})); //Activé en allant dans la chambreFoyer
		flags.Add (new Flag(100,false,"Entree au LIRIS",new List<int>(){90},new List<Evenement>(){})); //Activé en traversant le portail vers le LIRIS
		flags.Add (new Flag(110,false,"Debut quete recuperation composants electroniques",new List<int>(){100},new List<int>(){115},new List<Evenement>(){})); //Activé en parlant à ProfLIRIS
		flags.Add (new Flag(115,false,"Tous les composants recuperes",new List<int>(){110},new List<Evenement>(){})); //Activé en ramassant un des composants électroniques
		flags.Add (new Flag(120,false,"Fin quete recuperation composants electroniques",new List<int>(){115},new List<Evenement>(){})); //Activé en parlant à ProfLIRIS
		flags.Add (new Flag(130,false,"Acquisition inventaire",new List<int>(){120},new List<int>(){130},new List<Evenement>(){})); //Activé en parlant à profLiris
		flags.Add (new Flag(140,false,"Debut discours trez club BD pres du foyer",new List<int>(){130},new List<int>(){150},new List<Evenement>(){}));
		flags.Add (new Flag(150,false,"Entree club BD",new List<int>(){130},new List<Evenement>(){})); //Activé en traversant le portail ClubBD
		flags.Add (new Flag(151,false,"PrezBD explique pourquoi foyer barricade",new List<int>(){150})); //Dialogue intermédiaire
		flags.Add (new Flag(152,false,"PrezBD explique pourquoi ils sont la",new List<int>(){150}));
		flags.Add (new Flag(153,false,"PrezBD explique ce que sont les monstres",new List<int>(){150}));
		flags.Add (new Flag(160,false,"Debut quete recuperation codebar",new List<int>(){150},new List<int>(){160,210},new List<Evenement>(){})); //Activé en parlant au PrezBD
		flags.Add (new Flag(170,false,"Codebar recuperee",new List<int>(){160},new List<Evenement>(){})); //Activé en ramassant la carte sur le terrain
		flags.Add (new Flag(171,false,"Remarque prez club BD - intermediaire dialogue",new List<int>(){170},new List<Evenement>(){}));
		flags.Add (new Flag(175,false,"Indication du prez club BD sur le club Serial Gamers donnée",new List<int>(){170},new List<int>(){180}));
		flags.Add (new Flag(180,false,"Acquisition part pizza du prez Serial Gamers",new List<int>(){170},new List<Evenement>(){})); //Parler au prez serial gamers
		flags.Add (new Flag(190,false,"Systeme d'evolution du personnage acquis",new List<int>(){170},new List<Evenement>(){})); //Dialogue prez serial gamers
		flags.Add (new Flag(200,false,"Recuperation montre",new List<int>(){170},new List<Evenement>(){})); //dialogue prez serial gamers
		flags.Add (new Flag(210,false,"Fin quete recuperation codebar",new List<int>(){190,200},new List<Evenement>(){})); //Dialogue prez serial gamers
		flags.Add (new Flag(220,false,"Retour dans la chambre apres recuperation codebar",new List<int>(){210},new List<Evenement>(){})); //Retour dans la chambre
		flags.Add (new Flag(230,false,"Dormir dans le lit apres recuperation codebar",new List<int>(){220},new List<Evenement>(){})); //Lit
		flags.Add (new Flag(240,false,"Message chambre apres sommeil",new List<int>(){230},new List<Evenement>(){}));
		flags.Add (new Flag(250,false,"Debut quete prof eco",new List<int>(){230},new List<Evenement>(){})); //Dialogue PrezClubBD
		flags.Add (new Flag(260,false,"Apparition objets a vendre sur le campus",new List<int>(){250},new List<Evenement>(){}));//Dialogue Prof Eco
		flags.Add (new Flag(270,false,"Fin quete prof eco",new List<int>(){250},new List<Evenement>(){})); //Dialogue ProfEco
		flags.Add (new Flag(280,false,"Debut quete choix laboratoire",new List<int>(){270},new List<Evenement>(){})); //Dialogue Serial Gamers
		flags.Add (new Flag(290,false,"Fin quete choix laboratoire",new List<int>(){280},new List<Evenement>(){})); //Dialogue profLiris
		flags.Add (new Flag(300,false,"Debut quete DSI",new List<int>(){290},new List<Evenement>(){})); //SerialGamers
		flags.Add (new Flag(310,false,"Ramassage cle DSI",new List<int>(){300},new List<Evenement>(){})); //Activé en examinant une fois la clé devant la DSI
		flags.Add (new Flag(320,false,"Fin quete DSI",new List<int>(){310},new List<Evenement>(){})); //Activé en examinant 2 fois la clé devant la DSI
		flags.Add (new Flag(330,false,"Debut quete foret",new List<int>(){320},new List<int>(){350},new List<Evenement>(){})); //Activé en examinant 3 fois la clé devant la DSI
		flags.Add (new Flag(340,false,"Recuperation cle gymnase",new List<int>(){330},new List<Evenement>(){})); //Activé en parlant au PrezBDS
		flags.Add (new Flag(350,false,"Fin quete foret",new List<int>(){340},new List<Evenement>(){})); //Activé en ramassant la clé qui spawn devant le Prez
		flags.Add (new Flag(360,false,"Ouverture gymnase",new List<int>(){340,350},new List<Evenement>(){})); //Activé en arrivant dans la scène Gymnase
		flags.Add (new Flag(370,false,"Debut combat gymnase",new List<int>(){360},new List<int>(){380},new List<Evenement>(){})); //Activé en parlant à Cotinaud
		flags.Add (new Flag(380,false,"Fin combat gymnase",new List<int>(){370},new List<Evenement>(){})); //Activé en battant Cotinaud
		flags.Add (new Flag(390,false,"Debut quete scolarite",new List<int>(){380},new List<Evenement>(){})); //Activé par dialogue ProfSport
		flags.Add (new Flag(400,false,"Fin quete scolarite",new List<int>(){390},new List<Evenement>(){})); //Activé par MusyBassot
		flags.Add (new Flag(410,false,"Entree labyrinthe",new List<int>(){400},new List<Evenement>(){})); //Activé en traversant le portail
		flags.Add (new Flag(420,false,"Sortie labyrinthe",new List<int>(){410},new List<Evenement>(){})); //Activé en passant le mur de fin du labyrinthe
		flags.Add (new Flag(425,false,"Fin discours directeur",new List<int>(){420},new List<Evenement>(){})); //Dialogue Debouck
		flags.Add (new Flag(430,false,"Entree amphi 2",new List<int>(){425},new List<Evenement>(){})); //Portail amphi2
		flags.Add (new Flag(431,false,"Debut boss final",new List<int>(){430},new List<Evenement>(){})); //Dialogue boss final
		flags.Add (new Flag(435,false,"Mort boss final",new List<int>(){431},new List<Evenement>(){})); //Mort du boss
		flags.Add (new Flag(440,false,"Fin amphi 2",new List<int>(){435},new List<Evenement>(){})); //Sortir par la porte après avoir tué le boss final
		flags.Add (new Flag(450,false,"Fin du jeu",new List<int>(){440},new List<Evenement>(){ new ActiverEcranFin() })); //Au centre du terrain

		//Quetes secondaires
		//Quete de la boule
		flags.Add (new Flag (441, false, "PremierBouton", new List<int> (){0}));
		flags.Add (new Flag (442, false, "DeuxiemeBouton", new List<int> (){0}));
		flags.Add (new Flag (443, false, "TroisiemeBouton", new List<int> (){0}));
		flags.Add (new Flag(444,false,"Activation de la BOULE", new List<int> () {441,442,443}));
		flags.Add (new Flag(445,false,"Mort de la boule",new List<int>() {444}));
		//Armes
		flags.Add (new Flag(446,false,"RecuperationNumerisator",new List<int>(){}));
	}

	static public Flag ChercherFlagParId(int id) {
		foreach (Flag f in flags) {
			if(id==f.id)
				return f;
		}
		return null;
	}

	/**
	 * @brief Active un flag si possible, et renvoie true s'il est active.
	 * @param id Numero du flag à activer.
	 *
	 * @return Renvoie true si le flag est actif, sans tenir compte de son etat avant l'appel à cette fonction. Renvoie false pour un flag inexistant.
	 */
	static public bool ActiverFlag(int id) {
		return ActiverFlag (ChercherFlagParId (id));
	}

	/**
	 * @brief Active un flag si possible, et renvoie true s'il est active.
	 * @param f Reference du flag à activer.
	 *
	 * @return Renvoie true si le flag est à true avant ou après avoir essayé de l'activer. Renvoie false pour un flag inexistant.
	 * @details Si on a un changement de flag on vérifie quels objets sont à activer désactiver
	 */
	static public bool ActiverFlag(Flag f) {
		Debug.Log ("Tentative d'activation du flag d'id : "+f.id);
		if(f==null) return false;
		if(f.actif) return true; //Flag déjà actif.

		//On verifie si le flag est activable
		foreach(int id in f.predecesseurs) {//Si l'un est inactif on ne peut pas activer ce Flag
			Debug.Log ("Etude du predecesseur d'id "+id);
			if(!ChercherFlagParId (id).actif) {
				Debug.Log("Flag id : "+f.id+" pas activable");
				f.actif=false; //Au cas où il y ait eu des mauvais positifs
				ControlCenter.VerifierLesOASFs();
				return false;
			}
		}

		foreach(int id in f.bloquants) {//Si l'un est inactif on ne peut pas activer ce Flag
			Debug.Log ("Etude du bloquant d'id "+id);
			if(ChercherFlagParId (id).actif) {
				Debug.Log("Flag id : "+f.id+" pas activable");
				f.actif=false; //Au cas où il y ait eu des mauvais positifs
				ControlCenter.VerifierLesOASFs();
				return false;
			}
		}

		Debug.Log ("Activation du flag id : "+f.id+" => "+f.description);

		//Le flag est activable : on l'active et on effectue les evenements lies
		f.actif = true;
		foreach(Evenement e in f.evenementsDeclenches) {
			Debug.Log ("Activation evenement");
			e.DeclencherEvenement();
		}
		ControlCenter.VerifierLesOASFs();
		return f.actif; //Renvoie si le flag est actif.
	}

	/**
	 * @brief Renvoie true ou false selon si les listes données en arguments contiennent des flags dans les états souhaités.
	 */
	static public bool VerifierFlags(List<int> flagsDevantEtreActives, List<int> flagsDevantEtreDesactives) {
		foreach (int i in flagsDevantEtreActives) {
			if (!ChercherFlagParId (i).actif) {
				return false;
			}
		}
		foreach (int j in flagsDevantEtreDesactives) {
			if (ChercherFlagParId (j).actif) {
				return false;
			}
		}
		return true;
	}
}
