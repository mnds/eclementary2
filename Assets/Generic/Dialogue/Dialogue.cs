/**
 * \file      Dialogue.cs
 * \author    
 * \version   1.0
 * \date      31 octobre 2014
 * \brief     Contient toutes les répliques possibles d'un objet.
 *
 * \details   Toutes les répliques sont stockées ici. Les interactions entre les différentes répliques (lesquelles s'activent avant lesquelles) sont en partie
 * 			  faites ici.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dialogue {
	public List<Replique> repliquesObjet; //Liste de toutes les repliques de l'objet
	List<Replique> repliquesAccessibles;
	Replique repliqueActuelle; //Replique en train d'etre lue par l'objet

	void AddReplique (Replique replique) {
		repliquesObjet.Add (replique);
	}



	public void SetRepliquesObjet (List<Replique> repliquesObjet_) {
		repliquesObjet = repliquesObjet_;
	}
	
	public List<Replique> GetRepliquesObjet () {
		return repliquesObjet;
	}
	
	public void SetRepliquesAccessibles (List<Replique> repliquesAccessibles_) {
		repliquesAccessibles = repliquesAccessibles_;
	}
	
	public List<Replique> GetRepliquesAccessibles () {
		return repliquesAccessibles;
	}

	public void SetRepliqueActuelle (Replique repliqueActuelle_) {
		repliqueActuelle = repliqueActuelle_;
	}
	
	public Replique GetRepliqueActuelle () {
		return repliqueActuelle;
	}

}
