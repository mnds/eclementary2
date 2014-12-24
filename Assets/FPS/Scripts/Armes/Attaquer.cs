/**
 * \file      Attaquer.cs
 * \author    
 * \version   1.0
 * \date      9 novembre 2014
 * \brief     Indique qu'un objet peut être utilisé pour attaquer.
 *
 * \details   Ce script, placé sur un objet, indique que le bouton Fire1 lui permet d'etre utilisé pour attaquer. Il faut manuellement définir une position initiale et finale pour le coup.
 * 			  Le trajet initial->final est le seul à infliger des dégats. Il est nécessaire de manuelle repérer ces deux positions. Ce script doit etre placé sur le graphique de l'objet, qui contient un collider.
 */

/*
 * Utilisé dans Inventaire
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Note : lorsqu'on attaque, il est possible de jeter l'objet pendant l'animation de l'attaque.
public class Attaquer : MonoBehaviour {
	public bool bypass;//Si bI, rien ne se passe. Toutes les fonctions sont ignorées.
	
	public float degatsParCoup = 10.0f; //Degats initiaux, changés pour chaque obje
	public bool infligeDesDegatsAPlusieursEnnemis = true; //Si l'arme "traverse" les ennemis
	
	public bool enTrainDAttaquer = false; //false pendant un coup
	Vector3 positionInitiale;
	Quaternion rotationInitiale;
	Vector3 positionFinale;
	Quaternion rotationFinale;
	
	public Texture2D vignette; // utilisée pour la représentation de l'objet sur l'écran d'inventaire
	
	public float tempsInitialVersFinal = 1f; //Temps initial->final
	public float tempsFinalVersInitial = 1f; //Temps final->initial
	float tFVISiCollision; //Si retour anticipé
	
	public float avancementAnim = 0; //variable pour savoir où on en est dans une animation
	public bool enCoursDeRetour = false; //false si on doit aller de initial->final, true si final->initial
	bool infligerDegats; //true si on peut infliger des degats
	bool affecteParCaracteristiques = true; //true si les degats de l'arme sont influencés par la caractéristique Attaque du joueur
	List<Health> objetsTouchesLorsDeCetteAttaque; //Pour éviter de taper plusieurs fois les memes objets
	
	Lancer lancerGameObject; //Script de lancé pour empecher d'attaquer et lancer en meme temps
	
	void Start() {
		objetsTouchesLorsDeCetteAttaque = new List<Health> (){}; //Initialisation
		tFVISiCollision = tempsFinalVersInitial;
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

		if(!bypass) {
			positionInitiale = new Vector3 (0.5f, 0, 0.6f);
			rotationInitiale = new Quaternion (0, 0, .5f, .9f);
			positionFinale = new Vector3 (-0.9f, -1f, .9f);
			rotationFinale = new Quaternion (.4f, -.8f, .4f, .2f);
			transform.localPosition = positionInitiale;
			transform.localRotation = rotationInitiale;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(bypass) return;
		if (ControlCenter.inventaireOuvert) return;

		if (ControlCenter.GetCinematiqueEnCours()) return; //Rien pendant une cinématique
		//Debug.Log ("Rotation : " + transform.localRotation + " Position : " + transform.localPosition);

		//Si on demande d'attaquer, qu'on n'est pas déjà en train d'attaquer, et qu'on n'est pas en train de lancer
		if(Input.GetButtonDown("Fire1") && !enTrainDAttaquer &&
		   (lancerGameObject==null || !lancerGameObject.GetEstEnTrainDeLancer()))
		{
			gameObject.GetComponent<Collider>().isTrigger=true;
			enTrainDAttaquer=true;
			infligerDegats = true;
		}
		//Si on est en train d'attaquer, on continue
		if(enTrainDAttaquer)
		{
			if (avancementAnim < 1f) {
				if(!enCoursDeRetour) //On fait l'attaque
				{
					transform.localPosition = Vector3.Lerp(positionInitiale, positionFinale, avancementAnim);
					transform.localRotation = Quaternion.Lerp(rotationInitiale,rotationFinale,avancementAnim);
					avancementAnim = Mathf.Min (1f,avancementAnim + Time.deltaTime/tempsInitialVersFinal);
				}
				else //On revient au début
				{
					transform.localPosition = Vector3.Lerp(positionFinale, positionInitiale, avancementAnim);
					transform.localRotation = Quaternion.Lerp(rotationFinale,rotationInitiale,avancementAnim);
					avancementAnim = Mathf.Min (1f,avancementAnim + Time.deltaTime/tFVISiCollision);
				}
			}
			else //On a fini
			{
				avancementAnim=0; //On remet l'avancement à 0
				if(!enCoursDeRetour) //On a fini l'animation de coup, on signifie qu'on veut retourner au début
				{
					//On remet bien au bout de l'animation
					transform.localPosition=positionFinale;
					transform.localRotation=rotationFinale;
					enCoursDeRetour = true;
				}
				else //On est revenu au départ, on dit qu'on n'attaque plus.
				{
					transform.localPosition=positionInitiale;
					transform.localRotation=rotationInitiale;
					enCoursDeRetour = false;
					enTrainDAttaquer=false;
					objetsTouchesLorsDeCetteAttaque=new List<Health>(){}; //On remet à 0 les objets touchés.
				}
			}
		}
	}
	
	void OnTriggerEnter (Collider objet) {
		Debug.Log ("OnTriggerEnter");
		if(enCoursDeRetour) return;
		if(bypass) return;
		if (ControlCenter.inventaireOuvert) return;
		Debug.Log ("Pas de bypass");
		
		GameObject go = objet.gameObject;
		Transform objetAvecVie = go.transform;
		
		// La cible ne reçoit des dégâts que si le joueur l'attaque (la toucher ne suffit pas)
		if (enTrainDAttaquer && !enCoursDeRetour && infligerDegats) {
			Debug.Log ("Recevoir des degats");
			if(!infligeDesDegatsAPlusieursEnnemis)
				infligerDegats = false; //Plus de dégats
			//On cherche si l'objet ou un de ses parents a de la vie
			Health health = objetAvecVie.GetComponent<Health> (); //Si le truc touché a des points de vie, on doit le blesser
			while (health == null && objetAvecVie.parent) {
				objetAvecVie = objetAvecVie.parent;
				health = objetAvecVie.GetComponent<Health> ();
			}
			//On regarde si c'est le joueur lui-meme. Si oui, on ignore
			Attaquer[] attaquers = objetAvecVie.GetComponentsInChildren<Attaquer>();
			foreach(Attaquer a in attaquers)
			{
				if(a==this) {//Si on se tape soi-meme, on laisse tomber
					Debug.Log ("Cible ignorée car c'est l'attaquant");
					return;
				}
			}
			if (health != null) {
				Debug.Log ("Health non nul");
				//On vérifie qu'on ne l'a pas déjà touche
				foreach(Health h in objetsTouchesLorsDeCetteAttaque) {
					if(h==health) {
						Debug.Log("Objet déjà touché");
						return;
					}
				}
				Debug.Log ("Touché");
				
				//On récupère le script Caracteristiques du joueur principal pour appliquer les modifications aux dégats
				Caracteristiques carac = ControlCenter.GetJoueurPrincipal().GetComponent<Caracteristiques>();
				float degatsInfliges=degatsParCoup; //Initialement égal à la valeur "de base"
				if(carac && affecteParCaracteristiques) { //Formule de degats
					degatsInfliges = degatsParCoup+carac.GetAttaque(); ///FORMULE DE DEGATS
				}
				
				
				Debug.Log ("Degats infligés après application de l'attaque du joueur : "+degatsInfliges);
				
				health.SubirDegats (degatsInfliges);
				objetsTouchesLorsDeCetteAttaque.Add(health);
			}
		}
	}
	
	public bool GetEnTrainDAttaquer () {
		return enTrainDAttaquer;
	}
	public void SetEnTrainDAttaquer (bool a) {
		enTrainDAttaquer=a;
	}
	public void SetEnCoursDeRetour (bool a) {
		enCoursDeRetour=a;
	}
	public void SetBypass(bool bypass_) {
		bypass=bypass_;
	}
}
