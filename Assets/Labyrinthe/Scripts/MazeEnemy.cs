using UnityEngine;
using System.Collections;

public class MazeEnemy : MonoBehaviour {
	GameObject cible; //la cible
	GameObject cameraScreamer; //la caméra à activer quand on arrive trop près
	Vector3 positionCible;
	MazeManager mazeManager;
	bool labyrintheEnCoursDeRegeneration = false; //Lorsque le labyrinthe change ses murs, il ne faut pas lancer la coroutine

	// Use this for initialization
	void Start () {
		cible = GameObject.Find ("First Person Controller");
		cameraScreamer = GameObject.Find ("Camera Screamer");
	}
	
	// Update is called once per frame
	void Update () {
		positionCible.x = cible.transform.position.x; // use target's x and z
		positionCible.z = cible.transform.position.z;
		positionCible.y = transform.position.y; // use this object's y value
		//face the target
		transform.LookAt(positionCible);
		transform.Rotate (0, transform.rotation.y-90, 0, Space.World); //Le -90 est du bricolage ici. Prendre garde à ce que les objets soient bien orientés avec la tete face à z.
		//see if the player is close enough
		RaycastHit hit; //Information sur ce qui a été touché
		Vector3 direction = cible.transform.position - transform.position;
		if (Physics.Raycast(transform.position,direction,out hit,3.0f)) { // check for a hit within 30 meters
			//Debug.DrawLine(transform.position, hit.point);
			if(hit.collider==cible.collider && cameraScreamer.camera.enabled==false)
			{
				if(!labyrintheEnCoursDeRegeneration)
					StartCoroutine(ActiverCamera());
			}
		}
	}

	public void SetMazeManager (MazeManager mw_) {
		mazeManager = mw_;
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

}
