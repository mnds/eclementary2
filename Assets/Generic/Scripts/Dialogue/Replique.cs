/**
 * \file      Dialogue.cs
 * \author    
 * \version   1.0
 * \date      5 novembre 2014
 * \brief     Réplique d'un dialogue
 *
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Replique {

	/* sert à identifier de manière unique la réplique, 
	 * l'id 0 correspondant à la première réplique (qui en fait ne contient pas de texte )
	 * et l'id -1 correspondant à la dernière réplique
	 */
	int id;
	string texte; // Texte affiché à l'écran lorsque la réplique est sélectionnée
	List<Replique> repliquesSuivantes = new List<Replique>();
	List<int> flagsRequis, flagsBloquants, flagsActives;
	GameObject goAssocie; // GameObject qui prononcera la réplique

	// Permet de chainer une réplique suivante à la réplique courante
	public void AjouterRepliqueSuivante ( Replique replique ) {
		repliquesSuivantes.Add (replique);
	}

	// ___________________________Getters & Setters____________________________
	public int GetId() {
		return id;
	}

	public void SetId( int _id ) {
		id = _id;
	}

	public string GetTexte() {
		return texte;
	}

	public List<int> GetFlagsRequis() {
		return flagsRequis;
	}

	public List<int> GetFlagsBloquants() {
		return flagsBloquants;
	}

	public List<int> GetFlagsActives() {
		return flagsActives;
	}

	public void SetText( string _texte ) {
		texte = _texte;
	}

	public GameObject GetGoAssocie() {
		return goAssocie;
	}

	public void SetGoAssocie( GameObject go ) {
		goAssocie = go;
	}

	public List<Replique> GetRepliquesSuivantes(bool prendreEnCompteLesFlags=true) {
		if(prendreEnCompteLesFlags) {
			List<Replique> reponse = new List<Replique>(){};
			foreach(Replique r in repliquesSuivantes) { //On regarde toutes les répliques suivantes et on vérifie si elles sont accessibles au niveau de leurs flags
				bool estRepliqueSuivante=true;
				foreach(int flag in r.flagsRequis) { //On vérifie les flags requis
					if(!estRepliqueSuivante)
						break;
					if (!FlagManager.ChercherFlagParId (flag).actif) {
						estRepliqueSuivante=false;
					}
				}
				if(!estRepliqueSuivante)
					break;
				foreach(int flag in r.flagsBloquants) { //On vérifie les flags bloquants
					if(!estRepliqueSuivante)
						break;
					if (FlagManager.ChercherFlagParId (flag).actif) {
						estRepliqueSuivante=false;
					}
				}
				if(estRepliqueSuivante) //On ajoute si les flags sont bons
					reponse.Add (r);
			}
			return reponse;
		}
		else
			return repliquesSuivantes;
	}

	public void SetFlagsRequis( JSONNode rFlags ) {
		flagsRequis = new List<int>();
		for (int i = 0; i < rFlags.Count; i++) {
			flagsRequis.Add( int.Parse(rFlags[i].Value) );
		}
	}

	public void SetFlagsBloquants( JSONNode bFlags ) {
		flagsBloquants = new List<int>();
		for (int i = 0; i < bFlags.Count; i++) {
			flagsBloquants.Add( int.Parse(bFlags[i].Value) );
		}	
	}

	public void SetFlagsActives( JSONNode eFlags ) {
		flagsActives = new List<int>();
		for (int i = 0; i < eFlags.Count; i++) {
			flagsActives.Add( int.Parse(eFlags[i].Value) );
		}	
	}
}
