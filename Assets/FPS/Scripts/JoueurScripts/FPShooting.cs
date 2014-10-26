using UnityEngine;
using System.Collections;

public class FPShooting : MonoBehaviour {
	public float delaiEntreDeuxTirs = 0.5f;
	float tempsAvantProchainTir; //Contient la valeur de Time.time à dépasser pour pouvoir tirer

	public GameObject ballePrefab; //Le prefab correspondant à la balle tirée
	float vitesseBalle = 25.0f;
	Camera camera; //Camera utilisée

	// Use this for initialization
	void Start () {
		tempsAvantProchainTir = Time.time;
		camera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Fire1") && Time.time>=tempsAvantProchainTir) { //Bouton de tir
			tempsAvantProchainTir = Time.time+delaiEntreDeuxTirs;
			GameObject laBalle = (GameObject)Instantiate(ballePrefab,camera.transform.position+camera.transform.forward,camera.transform.rotation);
			laBalle.rigidbody.AddForce(camera.transform.forward*vitesseBalle,ForceMode.Impulse);
		}                       
	}
}
