/**
 * \file      MazeEnemy.cs
 * \author    
 * \version   1.0
 * \date      9 novembre 2014
 * \brief     S'occupe des ennemis présents dans le labyrinthe.
 *
 * \details   Si l'ennemi est assez proche de la cible, on déclenche la mort de celle-ci, soit par un screamer, soit par une chute.
 * 			  Ne fait pas bouger les ennemis.
 */

/*
 * Utilisé dans MazeManager
 */
using UnityEngine;
using System.Collections;

public class MazeEnemy : MonoBehaviour {
	GameObject cible; //la cible
	GameObject cameraScreamer; //la caméra à activer quand on arrive trop près
	Vector3 positionCible;
	MazeManager mazeManager;
	bool labyrintheEnCoursDeRegeneration = false; //Lorsque le labyrinthe change ses murs, il ne faut pas lancer la coroutine

	public float tempsChuteInduit = 0.3f;
	public float tempsReveilInduit = 1.5f;

	private float hauteurMurs;

	// Use this for initialization
	void Start () {
		cameraScreamer = GameObject.Find ("Camera Screamer");
	}
	
	// Update is called once per frame
	void Update () {
		positionCible.x = cible.transform.position.x; // use target's x and z
		positionCible.z = cible.transform.position.z;
		positionCible.y = transform.position.y; // use this object's y value
		//face the target
		transform.LookAt(positionCible);
		transform.Rotate (0, transform.rotation.y, 0, Space.World); //Le -90 est du bricolage ici. Prendre garde à ce que les objets soient bien orientés avec la tete face à z.
		//see if the player is close enough
		RaycastHit hit; //Information sur ce qui a été touché
		Vector3 direction = positionCible - transform.position;
		if (Physics.Raycast(transform.position+new Vector3(0,1f,0),direction,out hit,3f)) { // check for a hit within 3 meters
			Debug.DrawLine(transform.position+new Vector3(0,1f,0), hit.point);
			if(hit.collider==cible.collider)
			{
				Debug.Log ("Attrapé !");
				if(!labyrintheEnCoursDeRegeneration && !cible.GetComponent<AnimationChute>().enChute)
					//StartCoroutine(ActiverCamera());
					StartCoroutine(Chute());
			}
		}
	}

	public void SetMazeManager (MazeManager mw_) {
		mazeManager = mw_;
	}

	public void SetCible (GameObject cible_) {
		cible = cible_;
	}

	public void SetHauteurMurs (float hm) {
		hauteurMurs = hm;
	}

	public void SetLabyrintheEnCoursDeRegeneration (bool labyrintheEnCoursDeRegeneration_) {
		labyrintheEnCoursDeRegeneration = labyrintheEnCoursDeRegeneration_;
	}

	public IEnumerator ActiverCamera() {
		cameraScreamer.camera.enabled=true;
		cameraScreamer.GetComponent<MazeCameraScreamer> ().Activer();
		yield return new WaitForSeconds(2.0F);
		cameraScreamer.GetComponent<MazeCameraScreamer> ().Desactiver();
		cameraScreamer.camera.enabled=false;
		mazeManager.PlacerPersonnages();
	}

	public IEnumerator Chute () {
		AnimationChute ac = cible.GetComponent<AnimationChute>();
		ac.Chuter (tempsChuteInduit, tempsReveilInduit);
		yield return new WaitForSeconds((tempsChuteInduit+tempsReveilInduit)/2);
		mazeManager.PlacerPersonnages ();
	}
}
