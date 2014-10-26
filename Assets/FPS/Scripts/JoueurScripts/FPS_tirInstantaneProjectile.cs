using UnityEngine;
using System.Collections;

public class FPS_tirInstantaneProjectile : MonoBehaviour {
	public float delaiEntreDeuxTirs = 1f;
	float delaiRestant = 0; //Contient le temps à attendre avant de pouvoir retirer

	public GameObject projectilePrefab; //Le prefab correspondant à ce qui tombe de l'endroit qu'on vise

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

		if (Input.GetButton("Fire1") && delaiRestant <= 0) { //Bouton de tir
			delaiRestant = delaiEntreDeuxTirs; //On doit attendre

			Instantiate(projectilePrefab,cameraTransform.position+cameraTransform.forward,cameraTransform.rotation);
		}                       
	}
}
