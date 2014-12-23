﻿/**
 * \file      StateManager.cs
 * \author    
 * \version   1.0
 * \date      12 décembre 2014
 * \brief     Script gestionnaire des états, se charge entre autres du passage d'un état à un autre
 */

using UnityEngine;
using System.Collections;

public class StateManager : MonoBehaviour {

	private Etat etatActif; // Etat qui actif en l'instant
	private static StateManager instanceActive; // Instance de StateManager qui est active, utilisée pour implémenter un singleton
	public static string sceneDebut; // Scène par laquelle le jeu commence

	[HideInInspector]
	public GameData dataRef;

	void Awake() {
		// Création d'une nouvelle instance ssi une autre n'a pas déjà été créée
		if (instanceActive == null) {
			instanceActive = this;
			DontDestroyOnLoad( gameObject ); // Le gameObject parent, et le script avec, sont conservés lors des changements de scène
		} 
		else {
			DestroyImmediate( gameObject ); // Si une instance du script existait déjà, alors l'instance nouvellement créée est supprimée sur le champ
		}
	}

	// Use this for initialization
	void Start () {
		sceneDebut = "CampusExterieurLie";
		dataRef = GetComponent<GameData> ();
		etatActif = new EtatDepart ( this );
	}
	
	// Update is called once per frame
	void Update () {
		if (etatActif != null)
			etatActif.UpdateEtat ();
	}

	void OnGUI() {
		if (etatActif != null)
			etatActif.AfficherRendu ();
	}

	public static StateManager getInstance() {
		return instanceActive;
	}

	// Permet le changement de l'état actif à l'état donné en paramètre
	public void BasculerEtat( Etat etat ) {
		etatActif = etat;
	}

	public void Restart() {
		Destroy (gameObject); // Destruction du gameObject parent
		Application.LoadLevel ( sceneDebut ); // Chargement de la scène de début
	}
}