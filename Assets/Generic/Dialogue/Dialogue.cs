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

public class Dialogue : MonoBehaviour {
	public List<Replique> repliquesObjet; //Liste de toutes les repliques de l'objet
	List<Replique> repliquesAccessibles;
	Replique repliqueActuelle; //Replique en train d'etre lue par l'objet

	void Start () {
		repliquesAccessibles = new List<Replique> ();
		foreach (Replique replique in repliquesObjet) {
			replique.SetDialogue (this);
			if(replique.GetAccessible()) //Si la replique est accessible
				AddRepliqueAccessible(replique);
		}
	}

	//Ajout de repliques à l'objet par code
	public void AddReplique (Replique replique) {
		repliquesObjet.Add (replique);
		if (replique.GetDialogue () != null) //Si la réplique avait un dialogue avant
						replique.GetDialogue ().RemoveReplique (replique); //On lui enlève
		replique.SetDialogue (this); //Et on change le dialogue
	}

	//Appelé par AddReplique. Donc rien d'autre à faire que d'enlever la réplique.
	public void RemoveReplique (Replique replique) {
		repliquesObjet.Remove (replique);
	}

	//Ajout de repliques accessibles
	public void AddRepliqueAccessible (Replique replique) {
		replique.SetAccessible (true);
		repliquesAccessibles.Add (replique);
	}
	
	public void RemoveRepliqueAccessible (Replique replique) {
		replique.SetAccessible (false);
		repliquesAccessibles.Remove (replique);
	}


	public void ArreterRepliqueActuelle () {
		if(!repliqueActuelle) return; //S'il n'y a pas de replique actuelle, on n'a rien à faire
		repliqueActuelle.ArreterLecture (); //On arrete la lecture
		repliqueActuelle = null; //Plus de réplique actuelle
	}

	public void LancerReplique(Replique replique) {
		if(repliqueActuelle) return; //Si une replique est déjà en cours pour cet objet, on abandonne.
		if(!replique.GetAccessible()) return; //Si la replique n'est pas accessible, on ne peut pas la lancer

		repliqueActuelle = replique;
		replique.Lire ();
	}

	//Setters/Getters
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
 