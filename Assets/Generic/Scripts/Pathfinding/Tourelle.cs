/**
 * \file      Tourelle.cs
 * \author    
 * \version   1.0
 * \date      14 décembre 2014
 * \brief     Permet à un objet de tirer sur une cible dès que celle-ci s'approche trop. Seule la cible peut subir des dégats.
 * 
 * \details	  Hérite de Interactif.
 */
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))] //Pour le son du tir
public class Tourelle : MonoBehaviour {
	public bool estActivee = false; //Doit etre activée pour tirer

	GameObject cible; //La chose à toucher
	public float vitesseRotation = 2.0f; //Vitesse à laquelle la tourelle tourne. 0.5f est amplement suffisant pour garantir le suivi d'un perso qui marche.
	public float distanceMini = 30.0f;

	public float degatsParBalle = 1.0f; //degats d'une balle
	public float cooldown = 0.1f; //Temps entre deux balles
	float tempsDuDernierTir = 0; //Pour savoir si on peut tirer

	AudioSource audioSource; //Pour le tir
	public AudioClip clipTir; //Bruit du tir
	public GameObject etincellesTir; //Etincelles emises lors du tir

	// Use this for initialization
	void Start () {
		cible = ControlCenter.GetJoueurPrincipal ();
		audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(!estActivee) return; //Si la tourelle n'est pas activée elle ne fait rien.

		//Ajout de 0.75f à cause de la position du collider
		Vector3 directionVoulue = cible.transform.position+new Vector3(0f,0.75f) - transform.position;
		float distanceCible = Vector3.Magnitude (directionVoulue);
		float angleEntreDirections = Vector3.Angle (cible.transform.position, transform.position);
		if (distanceCible < distanceMini) {
			Quaternion rotationVoulue = Quaternion.LookRotation(directionVoulue);
			transform.rotation=Quaternion.Lerp(transform.rotation,rotationVoulue,Time.deltaTime*vitesseRotation/angleEntreDirections);
		}
		else {
			transform.Rotate(0,Time.deltaTime*vitesseRotation,0);
		}
		if (angleEntreDirections < 2f && Time.time>tempsDuDernierTir+cooldown) {
			Tir();
		}
	}

	/**
	 * @brief Tire tout droit.
	 * 
	 * @details Un bruit est déclenché à chaque tir. On vérifie si l'objet directement en face a un Health et que ça n'est pas le tireur, et on inflige des dégats.
	 */
	void Tir() {

		if(clipTir)
			audioSource.PlayOneShot(clipTir);
		if (etincellesTir)
			Instantiate (etincellesTir,gameObject.transform.position, Quaternion.identity);
		
		tempsDuDernierTir=Time.time;
		//On vérifie par Raycast qu'on a touché quelque chose, en partant
		Ray ray = new Ray (transform.position,transform.forward);
		RaycastHit hitInfo;
		
		if(Physics.Raycast(ray,out hitInfo, distanceMini)){
			GameObject go = hitInfo.collider.gameObject;
			if(go==cible){ //Le seul objet qui peut etre touché est la cible
				Vector3 hitPoint = hitInfo.point; //L'endroit qu'on touche
				Debug.Log("Hit Object : "+go.name);
				//Debug.Log("Hit Point : "+hitPoint);
				
				//enlever de la vie
				Transform objetAvecVie = go.transform;
				Health health = objetAvecVie.GetComponent<Health>(); //Si le truc touché a des points de vie, on doit le blesser
				while(health == null && objetAvecVie.parent){
					objetAvecVie=objetAvecVie.parent;
					health = objetAvecVie.GetComponent<Health>();
				}
				//On récupère le script Caracteristiques du joueur principal pour appliquer les modifications aux dégats
				Caracteristiques carac = GetComponent<Caracteristiques>();
				float degatsInfliges=degatsParBalle; //Initialement égal à la valeur "de base"
				if(carac) { //Formule de degats
					degatsInfliges = degatsParBalle+carac.GetAttaque(); ///FORMULE DE DEGATS
					Debug.Log ("Degats infligés après application de l'attaque du joueur : "+degatsInfliges);
				}
				if(health != null){
					health.SubirDegats(degatsInfliges);
				}
			}
		}

	}

	public void SetActive(bool active) {
		estActivee = active;
	}
}
