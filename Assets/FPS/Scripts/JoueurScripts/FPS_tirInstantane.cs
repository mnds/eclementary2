using UnityEngine;
using System.Collections;

public class FPS_tirInstantane : MonoBehaviour {
	public float range = 100.0f;

	public float delaiEntreDeuxTirs = 0.5f;
	float delaiRestant = 0; //Contient le temps à attendre avant de pouvoir retirer

	public float degatsParBalle = 0.5f;
	public GameObject debrisPrefab; //Le prefab correspondant à ce qui tombe de l'endroit qu'on vise

	GameObject parent; //Joueur
	Camera camera; //Camera utilisée
	Transform cameraTransform;

	// Use this for initialization
	void Start () {
		parent = this.gameObject;
		camera = Camera.main;
		cameraTransform = camera.transform;
	}
	
	// Update is called once per frame
	void Update () {
		delaiRestant -= Time.deltaTime;

		if (Input.GetButton("Fire2") && delaiRestant <= 0) { //Bouton de tir
			delaiRestant = delaiEntreDeuxTirs; //On doit attendre

			//On vérifie par Raycast qu'on a touché quelque chose, en partant
			Ray ray = new Ray (cameraTransform.position,cameraTransform.forward);
			RaycastHit hitInfo;

			if(Physics.Raycast(ray,out hitInfo, range)){
				GameObject go = hitInfo.collider.gameObject;
				if(go!=parent){ //Si l'objet touché n'est pas le tireur (le parent)
					Vector3 hitPoint = hitInfo.point; //L'endroit qu'on touche
					Debug.Log("Hit Object : "+go.name);
					Debug.Log("Hit Point : "+hitPoint);

					//enlever de la vie
					Transform objetAvecVie = go.transform;
					Health health = objetAvecVie.GetComponent<Health>(); //Si le truc touché a des points de vie, on doit le blesser
					while(health == null && objetAvecVie.parent){
						objetAvecVie=objetAvecVie.parent;
						health = objetAvecVie.GetComponent<Health>();
					}
					if(health != null){
						health.SubirDegats(degatsParBalle);
					}

					if(debrisPrefab != null){ //On met les débris
						Instantiate(debrisPrefab,hitPoint,Quaternion.identity);
					}
				}
			}
		}                       
	}
}
