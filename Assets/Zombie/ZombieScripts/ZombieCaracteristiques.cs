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

using UnityEngine;
using System.Collections;

public class ZombieCaracteristiques : Caracteristiques {
	public int scoreZombie = 10;

	public void SetScoreZombie (int scoreZombie_)
	{
		scoreZombie = scoreZombie_;
	}
	public int GetScoreZombie()
	{
		return scoreZombie;
	}
}
