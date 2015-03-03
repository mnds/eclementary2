using UnityEngine;
using System.Collections;

[System.Serializable]
public class Niveau  {

	public int niveau;
	public int experience;
	public float pointDeVie;
	public float endurance;
	public float attaque;
	public float defense;
	public int scoreZombie; // propriété que pour les zombie
	// Update is called once per frame
	public Niveau (int _niveau, int _experience, float _pointDeVie, float _endurance, float _attaque, float _defense) {
	
		niveau = _niveau;
		experience = _experience;
		pointDeVie = _pointDeVie;
		endurance = _endurance;
		attaque = _attaque;
		defense = _defense;
	}

	public Niveau (int _niveau, int _experience, float _pointDeVie, float _endurance,
	               float _attaque, float _defense, int _scoreZombie) {
		
		niveau = _niveau;
		experience = 0;
		pointDeVie = _pointDeVie;
		endurance = 0;
		attaque = _attaque;
		defense = _defense;
		scoreZombie = _scoreZombie;
	}
}
