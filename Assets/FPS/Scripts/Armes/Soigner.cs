/**
 * \file      Soigner.cs
 * \author    
 * \version   1.0
 * \date      19 octobre 2014
 * \brief     Indique qu'un objet soigne son utilisateur.
 *
 * \details   Ce script, placé sur un objet, indique que le bouton Fire1 lui permettra de soigner son utilisateur.
 */

using UnityEngine;
using System.Collections;

public class Soigner : MonoBehaviour {
	public bool bypass;//Si bI, rien ne se passe. Toutes les fonctions sont ignorées.
	bool estEnTrainDeSoigner = false; //true quand on lance. Dicté par tempsAvantProchainTir

	public float delaiEntreDeuxSoins = 0.1f;
	float tempsAvantProchainTir; //Contient la valeur de Time.time à dépasser pour pouvoir tirer
	public int munitions = 0; //Nombre de fois où l'objet peut etre lancé
	Inventaire inventaire ; //Il est nécessaire de dire à un inventaire que les munitions changent

	public float pointsDeVieSoignes = 5.0f;

	Lancer lancerGameObject; //Script de lancé pour empecher de soigner et lancer en meme temps

	void Start () {
		//Initialisation de lancer
		GameObject objet = gameObject; //On va parcourir les parents de gameObject pour trouver les scripts
		lancerGameObject = objet.GetComponent<Lancer>();
		//Tant que lancer est null, on vérifie si ce n'est pas le parent qui a le script
		while(lancerGameObject == null && objet.transform.parent){
			objet=objet.transform.parent.gameObject;
			lancerGameObject = objet.GetComponent<Lancer>();
		}
		//Sinon, on est à la racine, donc on cherche dans les enfants.
		if(lancerGameObject==null)
			lancerGameObject=objet.GetComponentInChildren<Lancer>();
	}

	// Update is called once per frame
	void Update () {
		if(bypass) return;
		if (ControlCenter.GetCinematiqueEnCours()) return; //Rien pendant une cinématique
		if(Time.time<tempsAvantProchainTir) return;

		//Si on demande de soigner, qu'on n'est pas déjà en train de soigner, et qu'on n'est pas en train de lancer
		if(Input.GetButtonDown("Fire1") && !estEnTrainDeSoigner &&
		   (lancerGameObject==null || !lancerGameObject.GetEstEnTrainDeLancer()))
		{
			//On récupère le script Health
			Health health; //Script qui contient les points de vie à rajouter
			health=this.GetComponentInParent<Health>();
			if(health==null)
				health=this.GetComponentInChildren<Health>();

			//On regarde si le script existe, et si le soin a fonctionné
			if(health) {
				if(health.Soigner(pointsDeVieSoignes)) { //Si oui on fait ce qu'il faut
					tempsAvantProchainTir = Time.time+delaiEntreDeuxSoins;
					//Traitements d'inventaire
					if(inventaire!=null)
						inventaire.ChangerMunitions (gameObject, munitions-1); //On enlève une munition de l'arme
					else
						Debug.Log ("Pas d'inventaire !");
					//On teste si l'objet a encore des munitions
					if(munitions<=0) { //S'il n'y a plus de munitions, on désactive l'objet. Pas de problème pour eETDL, car quand on réactivera l'objet, la dernière ligne de Update sera lue et le mettra comme il faut
						gameObject.SetActive(false); //On désactive l'objet pour l'instant
					}
				}
			}
		}
		estEnTrainDeSoigner = Time.time < tempsAvantProchainTir; //Savoir si on est en train de lancer
		//On teste si l'objet a encore des munitions
		if(munitions<=0) { //S'il n'y a plus de munitions, on désactive l'objet. Pas de problème pour eETDL, car quand on réactivera l'objet, la dernière ligne de Update sera lue et le mettra comme il faut
			gameObject.SetActive(false); //On désactive l'objet pour l'instant
		}

	}

	public bool GetEstEnTrainDeSoigner () {
		return estEnTrainDeSoigner;
	}

	public void SetBypass(bool bypass_) {
		bypass = bypass_;
	}

	public void SetMunitions (int munitions_) {
		munitions = Mathf.Max (0,munitions_);
	}

	public void SetMunitionsSimple (int munitions_) {
		munitions = Mathf.Max (0,munitions_);
	}

	public void SetInventaire (Inventaire inventaire_) {
		inventaire = inventaire_;
	}
}
