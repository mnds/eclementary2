/**
 * \file      Replique.cs
 * \author    
 * \version   1.0
 * \date      31 octobre 2014
 * \brief     Contient une réplique d'un objet.
 *
 * \details   Contient le texte d'une réplique, un son qui va avec, quelles autres répliques sont enclenchées par l'activation de cette réplique,
 * 		      et quels éléments sont affectés par cette réplique.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Replique : MonoBehaviour {
	bool accessible; //Si la replique a le droit d'etre utilisée
	bool enCours; //Si la replique est en train d'etre utilisee
	public string texteReplique; //Le texte associé à la réplique
	public AudioSource sourceDeLaReplique; //D'où le son produit par la réplique va venir
	public List<AudioClip> sonReplique; //Le son associé à la réplique. C'est une liste au cas où on voudrait mettre par exemple plusieurs intonations.
	AudioClip sonRepliqueActuel;

	bool desactivationAutomatique = false; //true si la réplique est désactivée dès que le son est arrete. Attention, il faut un clip sonore associé !
	float tempsDepuisLecture = 0;



	void Update () {
		if (!accessible) return; //Si ce n'est pas accessible, on laisse tomber
		//Si le son est en cours, on désactive le tout une fois que le son est terminé
		if(desactivationAutomatique && sonRepliqueActuel)
		{
			tempsDepuisLecture+=Time.deltaTime;
			if(tempsDepuisLecture>sonRepliqueActuel.length)
				ArreterLecture();
		}
	}

	public void Lire () {
		if (!accessible) return; //Si ce n'est pas accessible, on laisse tomber

		enCours = true;
		tempsDepuisLecture = 0;
		int sonLu = Random.Range (0, sonReplique.Count);
		sonRepliqueActuel = sonReplique [sonLu];
		if(sonRepliqueActuel && sourceDeLaReplique) //Si le son et la source existent, on le joue
			sourceDeLaReplique.PlayOneShot (sonRepliqueActuel);
	}

	public void ArreterLecture () {
		if (!accessible) return; //Si ce n'est pas accessible, on laisse tomber

		if(sourceDeLaReplique)
			sourceDeLaReplique.Stop ();
		enCours = false;
	}

	//Setters/Getters

	public void SetAccessible (bool accessible_) {
		accessible = accessible_;
	}
	
	public bool GetAccessible () {
		return accessible;
	}
	
	public void SetEnCours (bool enCours_) {
		enCours = enCours_;
	}
	
	public bool GetEnCours () {
		return enCours;
	}

	public void SetTexteReplique (string texteReplique_) {
		texteReplique = texteReplique_;
	}
	
	public string GetTexteReplique () {
		return texteReplique;
	}

	public void SetSourceDeLaReplique (AudioSource sourceDeLaReplique_) {
		sourceDeLaReplique = sourceDeLaReplique_;
	}
	
	public AudioSource GetSourceDeLaReplique () {
		return sourceDeLaReplique;
	}

	public void SetSonReplique (List<AudioClip> sonReplique_) {
		sonReplique = sonReplique_;
	}
	
	public List<AudioClip> GetSonReplique () {
		return sonReplique;
	}

}
