/**
 * \file      Replique.cs
 * \author    
 * \version   1.0
 * \date      31 octobre 2014
 * \brief     Contient une réplique d'un objet.
 *
 * \details   Contient le texte d'une réplique, un son qui va avec, quelles autres répliques sont enclenchées par l'activation de cette réplique,
 * 		      et quels éléments sont affectés par cette réplique.
 * 			  La réplique ne tient pas en compte de quelle réplique l'a déclenchée. La réplique du personnage joué et la réponse d'un objet sont deux répliques différentes.
 * 			  Les répliques disponibles pour le joueur sont intégralement décidées dans DialogueManager.cs.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class Replique : MonoBehaviour {
	public bool enCours; //Si la replique est en train d'etre utilisee
	public string texteReplique; //Le texte associé à la réplique
	AudioSource sourceDeLaReplique; //D'où le son produit par la réplique va venir
	public List<AudioClip> sonReplique; //Le son associé à la réplique. C'est une liste au cas où on voudrait mettre par exemple plusieurs intonations.
	AudioClip sonRepliqueActuel;

	public bool desactivationAutomatique = false; //true si la réplique est désactivée dès que le son est arrete. Attention, il faut un clip sonore associé !
	float tempsDepuisLecture = 0;

	public bool accessibleDebut; //Au début, est-ce que la réplique est accessible
	bool accessible; //Si la replique a le droit d'etre utilisée
	public List<Replique> repliquesRenduesAccessibles; //Après activation de la replique, des repliques sont rendues accessibles
	public List<Replique> repliquesRenduesInaccessibles; //Après activation de la replique, des repliques sont rendues inaccessibles
	Dialogue dialogue; //Le dialogue auquel est associé la réplique. Est obligatoire.

	//Actions speciales
	public Replique repliqueSuivante; //Si une réplique doit etre declenchee juste apres, c'est ici qu'elle est stockée

	
	void Awake () {
		accessible = accessibleDebut; //Si accessible, Dialogue::start se charge de le mettre dans le dialogue associé
	}

	void Start () {
		sourceDeLaReplique = gameObject.GetComponent<AudioSource> ();
	}

	void Update () {
		if (!accessible) return; //Si ce n'est pas accessible, on laisse tomber
		//Si le son est en cours, on désactive le tout une fois que le son est terminé
		if(desactivationAutomatique && sonRepliqueActuel && enCours)
		{
			tempsDepuisLecture+=Time.deltaTime;
			if(tempsDepuisLecture>sonRepliqueActuel.length)
				dialogue.ArreterRepliqueActuelle();
		}
	}

	public void Lire () {
		if (!accessible) return; //Si ce n'est pas accessible, on laisse tomber

		enCours = true; //Son en cours
		dialogue.SetRepliqueActuelle (this); //On dit au dialogue qu'on est la réplique actuelle
		tempsDepuisLecture = 0; //Remise à 0
		int sonLu = Random.Range (0, sonReplique.Count); //On choisir un son associé à la réplique
		sonRepliqueActuel = sonReplique [sonLu]; //On le stocke
		if(sonRepliqueActuel && sourceDeLaReplique) //Si le son et la source existent, on le joue
			sourceDeLaReplique.PlayOneShot (sonRepliqueActuel);
	}

	//Arrete la lecture de l'objet ; le son associé est coupé, et on effectue les actions nécessaires à continuer le dialogue.
	public void ArreterLecture () {
		if (!accessible) return; //Si ce n'est pas accessible, on laisse tomber

		if(sourceDeLaReplique) //Le son est arreté
			sourceDeLaReplique.Stop ();
		enCours = false; //La réplique n'est plus en cours
		dialogue.SetRepliqueActuelle (null); //Plus de réplique actuelle

		FinirReplique (); //On fait tout ce qui est associé à la réplique
	}

	public void FinirReplique () {
		//Changer les flags des répliques associées
		RendreRepliquesAccessibles ();
		RendreRepliquesInaccessibles ();
		//Si une replique doit etre lue juste apres celle-ci, on la lance
		if (repliqueSuivante)
			repliqueSuivante.Lire ();
	}

	public void AddRepliqueAccessible (Replique replique) {
		repliquesRenduesAccessibles.Add (replique);
	}

	public void RemoveRepliqueAccessible (Replique replique) {
		repliquesRenduesAccessibles.Remove (replique);
	}

	public void AddRepliqueInaccessible (Replique replique) {
		repliquesRenduesInaccessibles.Add (replique);
	}

	public void RemoveRepliqueInaccessible (Replique replique) {
		repliquesRenduesInaccessibles.Remove (replique);
	}

	public void RendreRepliquesAccessibles () {
		foreach (Replique replique in repliquesRenduesAccessibles)
		{
			replique.SetAccessible(true);
			replique.GetDialogue().AddRepliqueAccessible(replique);
		}
	}

	public void RendreRepliquesInaccessibles () {
		foreach (Replique replique in repliquesRenduesInaccessibles)
		{
			replique.SetAccessible(false);
			replique.GetDialogue().RemoveRepliqueAccessible(replique);
		}
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

	public void SetDialogue (Dialogue dialogue_) {
		dialogue = dialogue_;
	}
	
	public Dialogue GetDialogue () {
		return dialogue;
	}

	public void SetDesactivationAutomatique (bool desactivationAutomatique_) {
		desactivationAutomatique = desactivationAutomatique_;
	}
	
	public bool GetDesactivationAutomatique () {
		return desactivationAutomatique;
	}

	public void SetRepliqueSuivante (Replique repliqueSuivante_) {
		repliqueSuivante = repliqueSuivante_;
	}
	
	public Replique GetRepliqueSuivante () {
		return repliqueSuivante;
	}
}
