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

		public Flag(int id_,bool actifDebut, string description_, List<int> predecesseurs_,List<Evenement> evenements_) {
			id=id_;
			actif=actifDebut;
			description=description_;
			predecesseurs=predecesseurs_;
			bloquants=new List<int>(){};
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

		//Flags du scenario
		flags.Add (new Flag(10,false,"Debut du jeu",new List<int>(){},new List<Evenement>(){}));
		flags.Add (new Flag(20,false,"Thermobus affiche",new List<int>(){10},new List<Evenement>(){}));
		flags.Add (new Flag(30,false,"Bus choisi",new List<int>(){20},new List<Evenement>(){}));
		flags.Add (new Flag(40,false,"Retour du WEI",new List<int>(){30},new List<Evenement>(){}));
		flags.Add (new Flag(50,false,"Papier du cadavre obtenu",new List<int>(){40},new List<int>(){50},new List<Evenement>(){}));
		flags.Add (new Flag(60,false,"Discussion avec le personnage devant le foyer",new List<int>(){40},new List<Evenement>(){}));
		flags.Add (new Flag(70,false,"Recuperation de la belle boite",new List<int>(){60},new List<Evenement>(){}));
		flags.Add (new Flag(80,false,"Recuperation du plan de Centrale",new List<int>(){60},new List<Evenement>(){}));
		flags.Add (new Flag(90,false,"Recuperation du message du personnage du foyer dans sa chambre",new List<int>(){60},new List<Evenement>(){}));
		flags.Add (new Flag(100,false,"Entree au LIRIS",new List<int>(){90},new List<Evenement>(){}));
		flags.Add (new Flag(110,false,"Debut quete recuperation composants electroniques",new List<int>(){100},new List<Evenement>(){}));
		flags.Add (new Flag(115,false,"Tous les composants recuperes",new List<int>(){110},new List<Evenement>(){}));
		flags.Add (new Flag(120,false,"Fin quete recuperation composants electroniques",new List<int>(){115},new List<Evenement>(){}));
		flags.Add (new Flag(130,false,"Acquisition inventaire",new List<int>(){120},new List<Evenement>(){}));
		flags.Add (new Flag(140,false,"Debut discours trez club BD pres du foyer",new List<int>(){130},new List<Evenement>(){}));
		flags.Add (new Flag(150,false,"Entree club BD",new List<int>(){130},new List<Evenement>(){}));
		flags.Add (new Flag(160,false,"Debut quete recuperation codebar",new List<int>(){150},new List<Evenement>(){}));
		flags.Add (new Flag(170,false,"Codebar recuperee",new List<int>(){160},new List<Evenement>(){}));
		flags.Add (new Flag(180,false,"Acquisition part pizza du prez du club BD",new List<int>(){170},new List<Evenement>(){}));
		flags.Add (new Flag(190,false,"Systeme d'evolution du personnage acquis",new List<int>(){170},new List<Evenement>(){}));
		flags.Add (new Flag(200,false,"Recuperation montre",new List<int>(){170},new List<Evenement>(){}));
		flags.Add (new Flag(210,false,"Fin quete recuperation codebar",new List<int>(){190,200},new List<Evenement>(){}));
		flags.Add (new Flag(220,false,"Retour dans la chambre apres recuperation codebar",new List<int>(){210},new List<Evenement>(){}));
		flags.Add (new Flag(230,false,"Dormir dans le lit apres recuperation codebar",new List<int>(){220},new List<Evenement>(){}));
		flags.Add (new Flag(240,false,"Message chambre apres sommeil",new List<int>(){230},new List<Evenement>(){}));
		flags.Add (new Flag(250,false,"Debut quete prof eco",new List<int>(){240},new List<Evenement>(){}));
		flags.Add (new Flag(260,false,"Apparition objets a vendre sur le campus",new List<int>(){250},new List<Evenement>(){}));
		flags.Add (new Flag(270,false,"Fin quete prof eco",new List<int>(){240},new List<Evenement>(){}));
		flags.Add (new Flag(280,false,"Debut quete choix laboratoire",new List<int>(){270},new List<Evenement>(){}));
		flags.Add (new Flag(290,false,"Fin quete choix laboratoire",new List<int>(){280},new List<Evenement>(){}));
		flags.Add (new Flag(300,false,"Debut quete DSI",new List<int>(){290},new List<Evenement>(){}));
		flags.Add (new Flag(310,false,"Ramassage cle DSI",new List<int>(){300},new List<Evenement>(){}));
		flags.Add (new Flag(320,false,"Fin quete DSI",new List<int>(){310},new List<Evenement>(){}));
		flags.Add (new Flag(330,false,"Debut quete foret",new List<int>(){320},new List<Evenement>(){}));
		flags.Add (new Flag(340,false,"Recuperation cle gymnase",new List<int>(){330},new List<Evenement>(){}));
		flags.Add (new Flag(350,false,"Fin quete foret",new List<int>(){340},new List<Evenement>(){}));
		flags.Add (new Flag(360,false,"Ouverture gymnase",new List<int>(){340,350},new List<Evenement>(){}));
		flags.Add (new Flag(370,false,"Debut combat gymnase",new List<int>(){360},new List<Evenement>(){}));
		flags.Add (new Flag(380,false,"Fin combat gymnase",new List<int>(){370},new List<Evenement>(){}));
		flags.Add (new Flag(390,false,"Debut quete scolarite",new List<int>(){380},new List<Evenement>(){}));
		flags.Add (new Flag(400,false,"Fin quete scolarite",new List<int>(){390},new List<Evenement>(){}));
		flags.Add (new Flag(410,false,"Entree labyrinthe",new List<int>(){400},new List<Evenement>(){}));
		flags.Add (new Flag(420,false,"Sortie labyrinthe",new List<int>(){410},new List<Evenement>(){}));
		flags.Add (new Flag(430,false,"Entree amphi 2",new List<int>(){420},new List<Evenement>(){}));
		flags.Add (new Flag(440,false,"Fin amphi 2",new List<int>(){430},new List<Evenement>(){}));
		flags.Add (new Flag(450,false,"Fin du jeu",new List<int>(){440},new List<Evenement>(){}));
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
	 * @return Renvoie true si le flag est actif, sans tenir compte de son etat avant l'appel à cette fonction. Renvoie false pour un flag inexistant.
	 */
	static public bool ActiverFlag(Flag f) {
		if(f==null) return false;
		//On verifie si le flag est activable
		foreach(int id in f.predecesseurs) {//Si l'un est inactif on ne peut pas activer ce Flag
			Debug.Log ("Etude du predecesseur d'id "+id);
			if(!ChercherFlagParId (id).actif) {
				Debug.Log("Flag id : "+f.id+" pas activable");
				f.actif=false; //Au cas où il y ait eu des mauvais positifs
				return false;
			}
		}

		foreach(int id in f.bloquants) {//Si l'un est inactif on ne peut pas activer ce Flag
			Debug.Log ("Etude du bloquant d'id "+id);
			if(ChercherFlagParId (id).actif) {
				Debug.Log("Flag id : "+f.id+" pas activable");
				f.actif=false; //Au cas où il y ait eu des mauvais positifs
				return false;
			}
		}

		Debug.Log ("Activation du flag id : "+f.id+" => "+f.description);

		//Le flag est activable : on l'active et on effectue les evenements lies
		f.actif = true;
		foreach(Evenement e in f.evenementsDeclenches) {
			e.DeclencherEvenement();
		}

		return f.actif; //Renvoie si le flag est actif.
	}
}
