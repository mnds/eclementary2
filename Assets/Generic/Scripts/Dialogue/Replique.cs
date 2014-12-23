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

public class Replique {

	/* sert à identifier de manière unique la réplique, 
	 * l'id 0 correspondant à la première réplique (qui en fait ne contient pas de texte )
	 * et l'id -1 correspondant à la dernière réplique
	 */
	int id;
	string texte; // Texte affiché à l'écran lorsque la réplique est sélectionnée
	List<Replique> repliquesSuivantes = new List<Replique>();
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
}
