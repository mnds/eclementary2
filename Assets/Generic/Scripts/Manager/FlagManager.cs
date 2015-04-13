/**
 * \file      FlagManager.cs
 * \author    
 * \version   1.0
 * \date      11 decembre 2014
 * \brief     Contient les details des evenements du scenario et les lie entre eux pour qu'ils s'activent \u00E0 certains moments.
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

		//Le flag actifDebut dit si le flag est dej\u00E0 actif au debut. Si celui-ci est \u00E0 true, les evenements lies au flag ne seront pas declenches au demarrage du logiciel.
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

		// Convertit une liste de flags normaux en liste de flags réduits 
		public static List<FlagReduit> toFlagsReduits( List<Flag> flags ) {
			List<FlagReduit> flagsReduits = new List<FlagReduit>();

			foreach( Flag flag in flags )
				flagsReduits.Add( new FlagReduit( flag.id, flag.actif ) );

			return flagsReduits;
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
		 * On va appuyer sur un bouton pr\u00E8s de la boule pour l'enclencher et il faut la tuer.
		 * Une fois la boule tuee, elle donne un objet qui declenche la fin du jeu.
		 */
		/*flags.Add (new Flag (0, true, "DebutDuJeu", null));
		flags.Add (new Flag (1, false, "PremierBouton", new List<int> (){0}));
		flags.Add (new Flag (2, false, "DeuxiemeBouton", new List<int> (){0}));
		flags.Add (new Flag (3, false, "TroisiemeBouton", new List<int> (){0}));
		flags.Add (new Flag(4,false,"Activation de la BOULE", new List<int> () {1,2,3}));
		flags.Add (new Flag(5,false,"Mort de la boule",new List<int>() {4}));*/

		//Flags de difficult\u00E9 du jeu
		flags.Add (new Flag(9, false, "Flag écran titre", new List<Evenement>(){new DesactiverInventaire()}) );
		flags.Add (new Flag(1,false,"Difficult\u00E9 facile",new List<Evenement>(){new ChangerDifficulte(new Item(NomItem.NomDifficulte,ControlCenter.Difficulte.Facile))}));
		flags.Add (new Flag(2,false,"Difficult\u00E9 normale",new List<Evenement>(){new ChangerDifficulte(new Item(NomItem.NomDifficulte,ControlCenter.Difficulte.Normale))}));
		flags.Add (new Flag(3,false,"Difficult\u00E9 difficile",new List<Evenement>(){new ChangerDifficulte(new Item(NomItem.NomDifficulte,ControlCenter.Difficulte.Difficile))}));
		flags.Add (new Flag(4,false,"Difficult\u00E9 tr\u00E8s difficile",new List<Evenement>(){new ChangerDifficulte(new Item(NomItem.NomDifficulte,ControlCenter.Difficulte.TresDifficile))}));
		//Flags du scenario
		flags.Add (new Flag(10,false,"Debut du jeu",new List<int>(){},new List<Evenement>(){new ChangerSpawnPoint(new Item(NomItem.SpawnPoint,"SpawnPointPresWeiman",ControlCenter.nomDeLaSceneDuCampus,true))}));
		flags.Add (new Flag(20,false,"Thermobus affiche",new List<int>(){10},new List<Evenement>(){})); //Activ\u00E9 en parlant au Weiman
		flags.Add (new Flag(30,false,"Bus choisi",new List<int>(){20},new List<Evenement>(){})); //Activ\u00E9 en choisissant un bus
		flags.Add (new Flag(40,false,"Retour du WEI",new List<int>(){30},new List<Evenement>(){new ChangerSpawnPoint(new Item(NomItem.SpawnPoint,"SpawnPointMilieuTerrain",ControlCenter.nomDeLaSceneDuCampus,true)),
																								new EnvoyerMessage(new Item(NomItem.Message,"40")),
																								new RemplirEndurance()})); //Activ\u00E9 en parlant de nouveau au Weiman
		flags.Add (new Flag(42,false,"Choix du batiment",new List<int>(){40},new List<int>(){42})); //Activ\u00E9 en rentrant \u00E0 Adoma
		flags.Add (new Flag(44,false,"Lit apres le WEI",new List<int>(){42},new List<int>(){44},new List<Evenement>(){})); //Activ\u00E9 en \u00E9tudiant le lit
		flags.Add (new Flag(45,false,"Appui porte après lit",new List<int>(){44},new List<int>(){45},new List<Evenement>(){new EnvoyerMessage(new Item(NomItem.Message,"44"))})); //Activ\u00E9 en \u00E9tudiant le lit
		flags.Add (new Flag(50,false,"Papier du cadavre obtenu",new List<int>(){45},new List<int>(){50},new List<Evenement>(){}));
		flags.Add (new Flag(60,false,"Discussion avec le personnage devant le foyer",new List<int>(){44},new List<Evenement>(){new EnvoyerMessage(new Item(NomItem.Message,"60"))})); //Activ\u00E9 en parlant au GarsFoyer
		flags.Add (new Flag(70,true,"Recuperation de la belle boite",new List<int>(){60},new List<Evenement>(){})); //Activ\u00E9 en allant dans la chambreFoyer (Comparat)
		flags.Add (new Flag(80,true,"Recuperation du plan de Centrale",new List<int>(){60},new List<Evenement>(){})); //Activ\u00E9 en allant dans la chambreFoyer
		flags.Add (new Flag(90,false,"Recuperation du message du personnage du foyer dans sa chambre",new List<int>(){60},new List<Evenement>(){new EnvoyerMessage(new Item(NomItem.Message,"90"))})); //Activ\u00E9 en allant dans la chambreFoyer
		flags.Add (new Flag(100,false,"Entree au LIRIS",new List<int>(){90},new List<Evenement>(){})); //Activ\u00E9 en traversant le portail vers le LIRIS
		flags.Add (new Flag(110,false,"Debut quete recuperation composants electroniques",new List<int>(){100},new List<int>(){115},new List<Evenement>(){new EnvoyerMessage(new Item(NomItem.Message,"110"))})); //Activ\u00E9 en parlant \u00E0 ProfLIRIS
		flags.Add (new Flag(115,false,"Tous les composants recuperes",new List<int>(){110},new List<Evenement>(){})); //Activ\u00E9 en ramassant un des composants \u00E9lectroniques
		flags.Add (new Flag(120,false,"Acquisition inventaire",new List<int>(){115},new List<Evenement>(){})); //Activ\u00E9 en parlant \u00E0 ProfLIRIS
		flags.Add (new Flag(130,false,"Fin quete recuperation composants electroniques",new List<int>(){120},new List<int>(){130},new List<Evenement>(){new ActiverInventaire(),new EnvoyerMessage(new Item(NomItem.Message,"130"))})); //Activ\u00E9 en parlant \u00E0 profLiris
		flags.Add (new Flag(140,false,"Debut discours trez club BD pres du foyer",new List<int>(){130},new List<int>(){150},new List<Evenement>(){}));
		flags.Add (new Flag(150,false,"Entree club BD",new List<int>(){130},new List<Evenement>(){})); //Activ\u00E9 en traversant le portail ClubBD
		flags.Add (new Flag(151,false,"PrezBD explique pourquoi foyer barricade",new List<int>(){150})); //Dialogue interm\u00E9diaire
		flags.Add (new Flag(152,false,"PrezBD explique pourquoi ils sont la",new List<int>(){150}));
		flags.Add (new Flag(153,false,"PrezBD explique ce que sont les monstres",new List<int>(){150}));
		flags.Add (new Flag(160,false,"Debut quete recuperation codebar",new List<int>(){150},new List<int>(){160,210},new List<Evenement>(){new EnvoyerMessage(new Item(NomItem.Message,"160"))})); //Activ\u00E9 en parlant au PrezBD
		flags.Add (new Flag(170,false,"Codebar recuperee",new List<int>(){160},new List<Evenement>(){})); //Activ\u00E9 en ramassant la carte sur le terrain
		flags.Add (new Flag(171,false,"Remarque prez club BD - intermediaire dialogue",new List<int>(){170},new List<Evenement>(){}));
		flags.Add (new Flag(175,false,"Indication du prez club BD sur le club Serial Gamers donn\u00E9e",new List<int>(){170},new List<int>(){180}));
		flags.Add (new Flag(180,false,"Acquisition part pizza du prez Serial Gamers",new List<int>(){170},new List<Evenement>(){})); //Parler au prez serial gamers
		flags.Add (new Flag(190,false,"Systeme d'evolution du personnage acquis",new List<int>(){170},new List<Evenement>(){})); //Dialogue prez serial gamers
		flags.Add (new Flag(200,false,"Recuperation montre",new List<int>(){170},new List<Evenement>(){})); //dialogue prez serial gamers
		flags.Add (new Flag(210,false,"Fin quete recuperation codebar",new List<int>(){190,200},new List<Evenement>(){new EnvoyerMessage(new Item(NomItem.Message,"210"))})); //Dialogue prez serial gamers
		flags.Add (new Flag(220,false,"Retour dans la chambre apres recuperation codebar",new List<int>(){210},new List<Evenement>(){})); //Retour dans la chambre
		flags.Add (new Flag(230,false,"Dormir dans le lit apres recuperation codebar",new List<int>(){220},new List<Evenement>(){})); //Lit
		flags.Add (new Flag(240,false,"Message chambre apres sommeil",new List<int>(){230},new List<Evenement>(){new EnvoyerMessage(new Item(NomItem.Message,"240"))}));
		flags.Add (new Flag(250,false,"Debut quete prof eco",new List<int>(){230},new List<Evenement>(){new EnvoyerMessage(new Item(NomItem.Message,"250"))})); //Dialogue PrezClubBD
		flags.Add (new Flag(260,false,"Apparition objets a vendre sur le campus",new List<int>(){250},new List<Evenement>(){}));//Dialogue Prof Eco
		flags.Add (new Flag(270,false,"Fin quete prof eco",new List<int>(){250},new List<Evenement>(){})); //Dialogue ProfEco
		flags.Add (new Flag(280,false,"Debut quete choix laboratoire",new List<int>(){270},new List<Evenement>(){new EnvoyerMessage(new Item(NomItem.Message,"280"))})); //Dialogue Serial Gamers
		flags.Add (new Flag(290,false,"Fin quete choix laboratoire",new List<int>(){280},new List<Evenement>(){})); //Dialogue profLiris
		flags.Add (new Flag(300,false,"Debut quete DSI",new List<int>(){290},new List<Evenement>(){new EnvoyerMessage(new Item(NomItem.Message,"300"))})); //SerialGamers
		flags.Add (new Flag(310,false,"Ramassage cle DSI",new List<int>(){300},new List<Evenement>(){})); //Activ\u00E9 en examinant une fois la cl\u00E9 devant la DSI
		flags.Add (new Flag(320,false,"Fin quete DSI",new List<int>(){310},new List<Evenement>(){})); //Activ\u00E9 en examinant 2 fois la cl\u00E9 devant la DSI
		flags.Add (new Flag(330,false,"Debut quete foret",new List<int>(){320},new List<int>(){350},new List<Evenement>(){new EnvoyerMessage(new Item(NomItem.Message,"330"))})); //Activ\u00E9 en examinant 3 fois la cl\u00E9 devant la DSI
		flags.Add (new Flag(340,false,"Recuperation cle gymnase",new List<int>(){330},new List<Evenement>(){})); //Activ\u00E9 en parlant au PrezBDS
		flags.Add (new Flag(350,false,"Fin quete foret",new List<int>(){340},new List<Evenement>(){new EnvoyerMessage(new Item(NomItem.Message,"350"))})); //Activ\u00E9 en ramassant la cl\u00E9 qui spawn devant le Prez
		flags.Add (new Flag(360,false,"Ouverture gymnase",new List<int>(){340,350},new List<Evenement>(){})); //Activ\u00E9 en arrivant dans la sc\u00E8ne Gymnase
		flags.Add (new Flag(370,false,"Debut combat gymnase",new List<int>(){360},new List<int>(){380},new List<Evenement>(){})); //Activ\u00E9 en parlant \u00E0 Cotinaud
		flags.Add (new Flag(380,false,"Fin combat gymnase",new List<int>(){370},new List<Evenement>(){})); //Activ\u00E9 en battant Cotinaud
		flags.Add (new Flag(390,false,"Debut quete scolarite",new List<int>(){380},new List<Evenement>(){new EnvoyerMessage(new Item(NomItem.Message,"390"))})); //Activ\u00E9 par dialogue ProfSport
		flags.Add (new Flag(400,false,"Fin quete scolarite",new List<int>(){390},new List<Evenement>(){})); //Activ\u00E9 par MusyBassot
		flags.Add (new Flag(410,false,"Entree labyrinthe",new List<int>(){400},new List<Evenement>(){new EnvoyerMessage(new Item(NomItem.Message,"410"))})); //Activ\u00E9 en traversant le portail
		flags.Add (new Flag(420,false,"Sortie labyrinthe",new List<int>(){410},new List<Evenement>(){})); //Activ\u00E9 en passant le mur de fin du labyrinthe
		flags.Add (new Flag(425,false,"Fin discours directeur",new List<int>(){420},new List<Evenement>(){new EnvoyerMessage(new Item(NomItem.Message,"430"))})); //Dialogue Directeur
		flags.Add (new Flag(430,false,"Entree amphi 2",new List<int>(){425},new List<Evenement>(){})); //Portail amphi2
		flags.Add (new Flag(431,false,"Debut boss final",new List<int>(){430},new List<Evenement>(){})); //Dialogue boss final
		flags.Add (new Flag(435,false,"Mort boss final1",new List<int>(){431},new List<Evenement>(){new EvenementActiverFlag(new Item(NomItem.Flag,"437"))})); //Mort du boss
		flags.Add (new Flag(436,false,"Mort boss final2",new List<int>(){431},new List<Evenement>(){new EvenementActiverFlag(new Item(NomItem.Flag,"437"))})); //Mort du boss
		flags.Add (new Flag(437,false,"Mort boss final",new List<int>(){435,436},new List<Evenement>(){new EnvoyerMessage(new Item(NomItem.Message,"437"))})); //Mort des boss
		flags.Add (new Flag(440,false,"Fin amphi 2",new List<int>(){435,436},new List<Evenement>(){})); //Sortir par la porte apr\u00E8s avoir tu\u00E9 le boss final
		flags.Add (new Flag(450,false,"Fin du jeu",new List<int>(){440},new List<Evenement>(){ new ActiverEcranFin() })); //Au centre du terrain

		//Quetes secondaires
		//Quete de la boule
		flags.Add (new Flag (441, true, "PremierBouton", new List<int> (){0}));
		flags.Add (new Flag (442, true, "DeuxiemeBouton", new List<int> (){0}));
		flags.Add (new Flag (443, true, "TroisiemeBouton", new List<int> (){0}));
		flags.Add (new Flag(444,false,"Activation de la BOULE", new List<int> () {435,441,442,443}));
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
	 * @param id Numero du flag \u00E0 activer.
	 *
	 * @return Renvoie true si le flag est actif, sans tenir compte de son etat avant l'appel \u00E0 cette fonction. Renvoie false pour un flag inexistant.
	 */
	static public bool ActiverFlag(int id) {
		return ActiverFlag (ChercherFlagParId (id));
	}

	/**
	 * @brief Active un flag si possible, et renvoie true s'il est active.
	 * @param f Reference du flag \u00E0 activer.
	 *
	 * @return Renvoie true si le flag est \u00E0 true avant ou apr\u00E8s avoir essay\u00E9 de l'activer. Renvoie false pour un flag inexistant.
	 * @details Si on a un changement de flag on v\u00E9rifie quels objets sont \u00E0 activer d\u00E9sactiver
	 */
	static public bool ActiverFlag(Flag f) {
		Debug.Log ("Tentative d'activation du flag d'id : "+f.id);
		if(f==null) return false;
		if(f.actif) return true; //Flag d\u00E9j\u00E0 actif.

		//On verifie si le flag est activable
		foreach(int id in f.predecesseurs) {//Si l'un est inactif on ne peut pas activer ce Flag
			Debug.Log ("Etude du predecesseur d'id "+id);
			if(!ChercherFlagParId (id).actif) {
				Debug.Log("Flag id : "+f.id+" pas activable");
				f.actif=false; //Au cas o\u00F9 il y ait eu des mauvais positifs
				ControlCenter.VerifierObjetsFlags();
				return false;
			}
		}

		foreach(int id in f.bloquants) {//Si l'un est inactif on ne peut pas activer ce Flag
			Debug.Log ("Etude du bloquant d'id "+id);
			if(ChercherFlagParId (id).actif) {
				Debug.Log("Flag id : "+f.id+" pas activable");
				f.actif=false; //Au cas o\u00F9 il y ait eu des mauvais positifs
				ControlCenter.VerifierObjetsFlags();
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
		ControlCenter.VerifierObjetsFlags();
		return f.actif; //Renvoie si le flag est actif.
	}

	/**
	 * @brief Renvoie true ou false selon si les listes donn\u00E9es en arguments contiennent des flags dans les \u00E9tats souhait\u00E9s.
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

	public static List<Flag> GetFlags() {
		return flags;
	}

	public static void SetFlags( List<Flag> flags_ ) {
		flags = flags_;
	}
}
