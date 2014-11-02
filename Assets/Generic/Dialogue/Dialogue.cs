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

[RequireComponent(typeof(AudioSource))]
public class Dialogue : MonoBehaviour {
	bool interactionPossible; //True si on peut interagir avec l'objet
	public float distanceMinimaleInteraction = 4.0f; //La distance à laquelle on doit etre pour pouvoir interagir avec l'objet

	public List<Replique> repliquesObjet; //Liste de toutes les repliques de l'objet
	List<Replique> repliquesAccessibles;
	Replique repliqueActuelle; //Replique en train d'etre lue par l'objet
	List<Replique> repliquesPrecedentes; //Les répliques possibles du joueur. Attention, ne pas mettre à la fois une réplique dans repSuivante et repPrecedentes ! Lorsqu'on clique sur un objet, les répliques à choisir sont celles-ci. Agir en conséquence !
	
	void Start () {
		RemplirRepliquesAccessibles ();
		RemplirRepliquesPrecedentes ();
	}

	/**
	 * @brief Sert à lancer un dialogue.
	 * @param interaction Le script interaction qui a demandé à ce que le dialogue s'enclenche.
	 *
	 * @details On cherche à savoir quelles sont les répliques précédentes pour les afficher pour que le joueur les choisisse. S'il n'y en a pas, on remplit par null.
	 */
	public void Trigger (Interaction interaction) {
		RemplirRepliquesPrecedentes (); //On récupère toutes les repliques précédentes
		if(repliquesPrecedentes.Count==0 && repliquesAccessibles.Count>0) {//S'il n'y a pas de repliques precedentes, et qu'il y a des repliques accessibles, on en lit une au hasard
			Replique repliqueLancee = repliquesAccessibles[Random.Range(0,repliquesAccessibles.Count)]; //On en prend ici une au hasard et on la lance
			LancerReplique(repliqueLancee);
		}
		else
		{
			//Afficher les trucs
			//Pour l'instant, on prend une replique precedente au hasard et on la jette
			Replique repliqueLancee; //Contient la replique à lancer
			int random = Random.Range (0,repliquesPrecedentes.Count);
			repliquesPrecedentes[random].Lire ();
		}
	}
	
	public void RemplirRepliquesAccessibles () {
		repliquesAccessibles = new List<Replique> ();
		foreach (Replique replique in repliquesObjet) {
			replique.SetDialogue (this);
			if(replique.GetAccessible()) //Si la replique est accessible
				AddRepliqueAccessible(replique);
		}
		//On regarde si des repliques sont accessibles. Selon la reponse, interactionPossible change
		interactionPossible = repliquesAccessibles.Count > 0;
	}

	public void RemplirRepliquesPrecedentes () {
		repliquesPrecedentes = new List<Replique> ();
		foreach (Replique replique in repliquesAccessibles) {
			foreach(Replique rep in replique.GetRepliquesPrecedentes())
				repliquesPrecedentes.Add (rep);
		}
	}

	public void ArreterRepliqueActuelle () {
		if(!repliqueActuelle) return; //S'il n'y a pas de replique actuelle, on n'a rien à faire
		repliqueActuelle.ArreterLecture (); //On arrete la lecture
		RemplirRepliquesPrecedentes (); //La lecture est arretée, on regarde quelles sont les repliques precedentes
	}

	public void LancerReplique(Replique replique) {
		if(repliqueActuelle) return; //Si une replique est déjà en cours pour cet objet, on abandonne.
		if(!replique.GetAccessible()) return; //Si la replique n'est pas accessible, on ne peut pas la lancer

		repliqueActuelle = replique;
		replique.Lire ();
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
		repliquesAccessibles.Add (replique);
		interactionPossible = repliquesAccessibles.Count > 0; //Si au moins une phrase est accessible, on peut interagir
	}
	
	public void RemoveRepliqueAccessible (Replique replique) {
		repliquesAccessibles.Remove (replique);
		interactionPossible = repliquesAccessibles.Count > 0; //Si aucune phrase n'est accessible, impossible d'interagir
	}
	
	public void AddRepliquePrecedente (Replique replique) {
		repliquesPrecedentes.Add (replique);
	}
	
	public void RemoveRepliquePrecedente (Replique replique) {
		repliquesPrecedentes.Remove (replique);
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

	public void SetInteractionPossible (bool interactionPossible_) {
		interactionPossible = interactionPossible_;
	}
	
	public bool GetInteractionPossible () {
		return interactionPossible;
	}

	public void SetDistanceMinimaleInteraction (float distanceMinimaleInteraction_) {
		distanceMinimaleInteraction = distanceMinimaleInteraction_;
	}
	
	public float GetDistanceMinimaleInteraction () {
		return distanceMinimaleInteraction;
	}

	public void SetRepliquesPrecedentes (List<Replique> repliquesPrecedentes_) {
		repliquesPrecedentes = repliquesPrecedentes_;
	}
	
	public List<Replique> GetRepliquesPrecedentes () {
		return repliquesPrecedentes;
	}

}
 