/**
 * \file      Scoremananger.cs
 * \author    Zepeng LI
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

public class ScoreManager : MonoBehaviour {

	public int score=0;

	// get/set
	public int GetScore ()
	{
		return score;
	}
	public void SetScore(int _score)
	{
		score=_score;
	}
	public void AjouteScore(int _score)
	{
		score += _score;
	}

	void OnGUI()
	{
		GUI.Label (new Rect (0, Screen.height - 50, 100, 50), "Score : " + score);
	}
}
