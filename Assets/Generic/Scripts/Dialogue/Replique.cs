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

	public List<Replique> GetRepliquesSuivantes() {
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
