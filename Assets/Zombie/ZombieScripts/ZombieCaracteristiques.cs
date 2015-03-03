using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * \file      ZombieCaracteristiques.cs
 * \author    
 * \version   1.0
 * \date      24 decembre 2014
 * \brief     Contient les statistiques du zombie sur lequel ce script est attaché.
 *
 * \details   Herite de Caracteristiques, Contient les caractéristiques naturelles du zombie (bonus de vie, endurance, attaque, défense...)
 * 			  ainsi que son état (empoisonné, brulé...) qui peuvent affecter l'expérience de jeu.
 */

/*
 * Utilisé dans Attaquer , Health , ControllerJoueur
 */

public class ZombieCaracteristiques : Caracteristiques {
	public int scoreZombie = 10;
	public List<Niveau> listeNiveauZombie;

	public void SetScoreZombie (int scoreZombie_)
	{
		scoreZombie = scoreZombie_;
	}
	public int GetScoreZombie()
	{
		return scoreZombie;
	}

	void Start ()
	{
		// On ajoute les différents niveaux à la liste
		listeNiveauZombie.Add (new Niveau (1, 0,10,10,5,5,5));   
		listeNiveauZombie.Add (new Niveau (2, 20,20,20,10,10,10));
		listeNiveauZombie.Add (new Niveau (3, 30,30,30,15,15,15));
		SetCaracteristiques (niveau);   // On modifie les caractéristiques de la zombie
	}

    private void SetCaracteristiques (int _niveau)
	{   int i = listeNiveauZombie.Count;

		if (_niveau <= 0)    _niveau = 1; // Si on rentre un niveau qui n est pas compris dans la listeNieau, on fait l'adaptation
		if (_niveau > i)     _niveau = i;

		Niveau niveauZombie = listeNiveauZombie [_niveau - 1];
		SetPointsDeVie (niveauZombie.pointDeVie);
		SetPointsEndurance (niveauZombie.endurance);
		SetAttaque (niveauZombie.attaque);
		SetDefense (niveauZombie.defense);
		SetScoreZombie (niveauZombie.scoreZombie);
	}
}
