using UnityEngine;
using System.Collections;

//Note : lorsqu'on attaque, il est possible de jeter l'objet pendant l'animation de l'attaque.
public class Attaquer : MonoBehaviour {
	public bool bypass;//Si bI, rien ne se passe. Toutes les fonctions sont ignorées.
	
	public float degatsParCoup = 10.0f; //Degats initiaux, changés pour chaque obje
	
	public bool enTrainDAttaquer = false; //false pendant un coup
	public Transform placementInitial; //GameObject placé à l'endroit où commence l'objet
	Vector3 positionInitiale;
	Quaternion rotationInitiale;
	public Transform placementFinal;
	Vector3 positionFinale;
	Quaternion rotationFinale;

	public Texture2D vignette; // utilisée pour la représentation de l'objet sur l'écran d'inventaire

	public float tempsInitialVersFinal = 1f; //Temps initial->final
	public float tempsFinalVersInitial = 1f; //Temps final->initial
	float avancementAnim = 0; //variable pour savoir où on en est dans une animation
	bool enCoursDeRetour = false; //false si on doit aller de initial->final, true si final->initial
	
	Lancer lancerGameObject; //Script de lancé pour empecher d'attaquer et lancer en meme temps
	
	void Start() {
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
		//Si on demande d'attaquer, qu'on n'est pas déjà en train d'attaquer, et qu'on n'est pas en train de lancer
		if(Input.GetButton("Fire1") && !enTrainDAttaquer && (lancerGameObject==null || !lancerGameObject.GetEstEnTrainDeLancer()))
		{
			gameObject.GetComponent<Collider>().isTrigger=true;
			enTrainDAttaquer=true;
		}
		if(enTrainDAttaquer)
		{
			//A réactualiser à chaque fois
			positionInitiale = placementInitial.position;
			rotationInitiale = placementInitial.rotation;
			positionFinale = placementFinal.position;
			rotationFinale = placementFinal.rotation;
			
			if (avancementAnim < 1f) {
				if(!enCoursDeRetour) //On fait l'attaque
				{
					transform.position = Vector3.Lerp(positionInitiale, positionFinale, avancementAnim);
					transform.rotation = Quaternion.Lerp(rotationInitiale,rotationFinale,avancementAnim);
					avancementAnim = avancementAnim + Time.deltaTime*tempsInitialVersFinal;
				}
				else //On revient au début
				{
					transform.position = Vector3.Lerp(positionFinale, positionInitiale, avancementAnim);
					transform.rotation = Quaternion.Lerp(rotationFinale,rotationInitiale,avancementAnim);
					avancementAnim = avancementAnim + Time.deltaTime*tempsFinalVersInitial;
				}
			}
			else //On a fini
			{
				avancementAnim=0; //On remet l'avancement à 0
				if(!enCoursDeRetour) //On a fini l'animation de coup, on signifie qu'on veut retourner au début
				{
					enCoursDeRetour = true;
				}
				else //On est revenu au départ, on dit qu'on n'attaque plus.
				{
					enCoursDeRetour = false;
					enTrainDAttaquer=false;
				}
			}
		}
	}
	
	void OnTriggerEnter (Collider objet) {
		if(enCoursDeRetour) return;
		if(bypass) return;
		
		GameObject go = objet.gameObject;
		Transform objetAvecVie = go.transform;
		
		// La cible ne reçoit des dégâts que si le joueur l'attaque (la toucher ne suffit pas)
		if (enTrainDAttaquer && !enCoursDeRetour) {
			//On cherche si l'objet ou un de ses parents a de la vie
			Health health = objetAvecVie.GetComponent<Health> (); //Si le truc touché a des points de vie, on doit le blesser
			while (health == null && objetAvecVie.parent) {
				objetAvecVie = objetAvecVie.parent;
				health = objetAvecVie.GetComponent<Health> ();
			}
			if (health != null) {
				health.SubirDegats (degatsParCoup);
			}
		}
	}
	
	public bool GetEnTrainDAttaquer () {
		return enTrainDAttaquer;
	}
	
	public void SetBypass(bool bypass_) {
		bypass=bypass_;
	}
}
