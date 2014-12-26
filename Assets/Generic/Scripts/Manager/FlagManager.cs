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
#define li = new List<int>(){}

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
