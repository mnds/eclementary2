/**
 * \file      FlagManager.cs
 * \author    
 * \version   1.0
 * \date      11 décembre 2014
 * \brief     Contient les détails des événements du scénario et les lie entre eux pour qu'ils s'activent à certains moments.
 * 
 * \details	  Les flags sont créés dans ce script manuellement. Une méthode renvoie l'état d'un flag une fois qu'on a tenté de l'activer.
 * 			  L'activation et la vérification des conditions associées sont faites dans ce script.
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
		public List<int> predecesseurs; //Pour vérifier si un flag est bien activable
		public List<Evenement> evenementsDeclenches;

		//Le flag actifDebut dit si le flag est déjà actif au début. Si celui-ci est à true, les événements liés au flag ne seront pas déclenchés au démarrage du logiciel.
		public Flag(int id_,bool actifDebut, string description_, List<int> predecesseurs_,List<Evenement> evenements_) {
			id=id_;
			actif=actifDebut;
			description=description_;
			predecesseurs=predecesseurs_;
			evenementsDeclenches=evenements_;
		}

		public Flag(int id_,bool actifDebut, string description_, List<int> predecesseurs_) {
			id=id_;
			actif=actifDebut;
			description=description_;
			predecesseurs=predecesseurs_;
			evenementsDeclenches=new List<Evenement>(){};
		}
	}
	

	static private List<Flag> flags = new List<Flag>{};

	static FlagManager () {
		//if(ControlCenter.DebutDuJeu)
		RemplirDefaut();
	}

	/**
	 * @brief Remplissage par défaut des flags. Seul le flag 0, d'initialisation, est actif.
	 */
	static private void RemplirDefaut () {
		//Redaction des differents flags
		/* Version de test pour l'édition décembre 2014.
		 * On commence sans rien, on va chercher le numérisator sur le campus
		 * On va appuyer sur un bouton près de la boule pour l'enclencher et il faut la tuer.
		 * Une fois la boule tuée, elle donne un objet qui déclenche la fin du jeu.
		 */
		/*flags.Add (new Flag (0, true, "DebutDuJeu", null));
		flags.Add (new Flag (1, false, "PremierBouton", new List<int> (){0}));
		flags.Add (new Flag (2, false, "DeuxiemeBouton", new List<int> (){0}));
		flags.Add (new Flag (3, false, "TroisiemeBouton", new List<int> (){0}));
		flags.Add (new Flag(4,false,"Activation de la BOULE", new List<int> () {1,2,3}));
		flags.Add (new Flag(5,false,"Mort de la boule",new List<int>() {4}));*/

		//Flags du scénario
		flags.Add (new Flag(1,false,"Debut du jeu",new List<int>(){},new List<Evenement>(){}));
		flags.Add (new Flag(2,false,"Thermobus affich‚",new List<int>(){1},new List<Evenement>(){}));
		flags.Add (new Flag(3,false,"Bus choisi",new List<int>(){2},new List<Evenement>(){}));
		flags.Add (new Flag(4,false,"Retour du WEI",new List<int>(){3},new List<Evenement>(){}));
		flags.Add (new Flag(5,false,"Papier du cadavre obtenu",new List<int>(){4},new List<Evenement>(){}));
		flags.Add (new Flag(6,false,"Discussion avec le personnage devant le foyer",new List<int>(){4},new List<Evenement>(){}));
		flags.Add (new Flag(7,false,"Recuperation de la belle boite",new List<int>(){6},new List<Evenement>(){}));
		flags.Add (new Flag(8,false,"Recuperation du plan de Centrale",new List<int>(){6},new List<Evenement>(){}));
		flags.Add (new Flag(9,false,"Recuperation du message du personnage du foyer dans sa chambre",new List<int>(){6},new List<Evenement>(){}));
		flags.Add (new Flag(10,false,"Entree au LIRIS",new List<int>(){9},new List<Evenement>(){}));
		flags.Add (new Flag(11,false,"Debut quete recuperation composants electroniques",new List<int>(){10},new List<Evenement>(){}));
		flags.Add (new Flag(12,false,"Fin quete recuperation composants electroniques",new List<int>(){11},new List<Evenement>(){}));
		flags.Add (new Flag(13,false,"Acquisition inventaire",new List<int>(){12},new List<Evenement>(){}));
		flags.Add (new Flag(14,false,"Debut discours trez club BD pres du foyer",new List<int>(){13},new List<Evenement>(){}));
		flags.Add (new Flag(15,false,"Entree club BD",new List<int>(){13},new List<Evenement>(){}));
		flags.Add (new Flag(16,false,"Debut quete recuperation codebar",new List<int>(){15},new List<Evenement>(){}));
		flags.Add (new Flag(17,false,"Codebar recuperee",new List<int>(){16},new List<Evenement>(){}));
		flags.Add (new Flag(18,false,"Acquisition part pizza du prez du club BD",new List<int>(){17},new List<Evenement>(){}));
		flags.Add (new Flag(19,false,"Systeme d'evolution du personnage acquis",new List<int>(){17},new List<Evenement>(){}));
		flags.Add (new Flag(20,false,"Recuperation montre",new List<int>(){17},new List<Evenement>(){}));
		flags.Add (new Flag(21,false,"Fin quete recuperation codebar",new List<int>(){19,20},new List<Evenement>(){}));
		flags.Add (new Flag(22,false,"Retour dans la chambre apres recuperation codebar",new List<int>(){21},new List<Evenement>(){}));
		flags.Add (new Flag(23,false,"Dormir dans le lit apres recuperation codebar",new List<int>(){22},new List<Evenement>(){}));
		flags.Add (new Flag(24,false,"Message chambre apres sommeil",new List<int>(){23},new List<Evenement>(){}));
		flags.Add (new Flag(25,false,"Debut quete prof eco",new List<int>(){24},new List<Evenement>(){}));
		flags.Add (new Flag(26,false,"Apparition objets a vendre sur le campus",new List<int>(){25},new List<Evenement>(){}));
		flags.Add (new Flag(27,false,"Fin quete prof eco",new List<int>(){24},new List<Evenement>(){}));
		flags.Add (new Flag(28,false,"Debut quete choix laboratoire",new List<int>(){27},new List<Evenement>(){}));
		flags.Add (new Flag(29,false,"Fin quete choix laboratoire",new List<int>(){28},new List<Evenement>(){}));
		flags.Add (new Flag(30,false,"Debut quete DSI",new List<int>(){29},new List<Evenement>(){}));
		flags.Add (new Flag(31,false,"Ramassage cle DSI",new List<int>(){30},new List<Evenement>(){}));
		flags.Add (new Flag(32,false,"Fin quete DSI",new List<int>(){31},new List<Evenement>(){}));
		flags.Add (new Flag(33,false,"Debut quete foret",new List<int>(){32},new List<Evenement>(){}));
		flags.Add (new Flag(34,false,"Recuperation cle gymnase",new List<int>(){33},new List<Evenement>(){}));
		flags.Add (new Flag(35,false,"Fin quete foret",new List<int>(){34},new List<Evenement>(){}));
		flags.Add (new Flag(36,false,"Ouverture gymnase",new List<int>(){34,35},new List<Evenement>(){}));
		flags.Add (new Flag(37,false,"Debut combat gymnase",new List<int>(){36},new List<Evenement>(){}));
		flags.Add (new Flag(38,false,"Fin combat gymnase",new List<int>(){37},new List<Evenement>(){}));
		flags.Add (new Flag(39,false,"Debut quete scolarite",new List<int>(){38},new List<Evenement>(){}));
		flags.Add (new Flag(40,false,"Fin quete scolarite",new List<int>(){39},new List<Evenement>(){}));
		flags.Add (new Flag(41,false,"Entree labyrinthe",new List<int>(){40},new List<Evenement>(){}));
		flags.Add (new Flag(42,false,"Sortie labyrinthe",new List<int>(){41},new List<Evenement>(){}));
		flags.Add (new Flag(43,false,"Entree amphi 2",new List<int>(){42},new List<Evenement>(){}));
		flags.Add (new Flag(44,false,"Fin amphi 2",new List<int>(){43},new List<Evenement>(){}));
		flags.Add (new Flag(45,false,"Fin du jeu",new List<int>(){44},new List<Evenement>(){}));
	}

	static private Flag ChercherFlagParId(int id) {
		foreach (Flag f in flags) {
			if(id==f.id)
				return f;
		}
		return null;
	}

	/**
	 * @brief Active un flag si possible, et renvoie true s'il est activé.
	 * @param id Numéro du flag à activer.
	 *
	 * @return Renvoie true si le flag est actif, sans tenir compte de son état avant l'appel à cette fonction.
	 */
	static public bool ActiverFlag(int id) {
		return ActiverFlag (ChercherFlagParId (id));
	}

	/**
	 * @brief Active un flag si possible, et renvoie true s'il est activé.
	 * @param f Référence du flag à activer.
	 *
	 * @return Renvoie true si le flag est actif, sans tenir compte de son état avant l'appel à cette fonction.
	 */
	static public bool ActiverFlag(Flag f) {
		//On vérifie si le flag est activable
		foreach(int id in f.predecesseurs) {//Si l'un est inactif on ne peut pas activer ce Flag
			if(!ChercherFlagParId (id).actif) {
				Debug.Log("Flag id : "+f.id+" pas activable");
				f.actif=false; //Au cas où il y ait eu des mauvais positifs
				return false;
			}
		}
		Debug.Log ("Activation du flag id : "+f.id+" => "+f.description);

		//Le flag est activable : on l'active et on effectue les événements liés
		f.actif = true;
		foreach(Evenement e in f.evenementsDeclenches) {
			e.DeclencherEvenement();
		}

		return f.actif; //Renvoie si le flag est actif.
	}
}
