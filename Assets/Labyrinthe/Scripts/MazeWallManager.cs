using UnityEngine;
using System.Collections.Generic;

public class MazeWallManager : MonoBehaviour {
	private List<int> anglesPossibles;
	public int angleDeRotation = 0; //le mur est droit au départ
	[HideInInspector] public Vector3 axeRotation; //point par lequel passe l'axe de rotation
	public int i; //Position i selon MazeManager
	public int j; //Position j selon MazeManager
	//Pour les entree/sortie du labyrinthe
	private bool estMurEntree = false;
	private bool estMurSortie = false;
	private GameObject fpc;

	public void SetFpc(GameObject fpc_) {
		fpc = fpc_;
	}

	public void SetAxeRotation () {
		axeRotation = transform.position - new Vector3 (2.5f, 0.0f, 0.0f);
		anglesPossibles = new List<int> () {0,90,90,180,180,270,270};
		int nombreRandom = Random.Range (0,anglesPossibles.Count-1); //nombre aléatoire
		int angle = anglesPossibles[nombreRandom]; //angle aléatoire dans la liste proposée
		Tourner (angle);
	}

	public void Tourner (int angle) {
		//Fait tourner le mur selon un axe vertical situé au centre d'une de ses largeurs
		transform.RotateAround(axeRotation,Vector3.up,
		                       angle-angleDeRotation); //retirer l'angle de rotation initial et rajouter le nouveau
		angleDeRotation = angle; //actualiser la valeur de l'angle de rotation
	}

	public void SetMurEntree () {
		estMurEntree = true;
		estMurSortie = false;
		collider.isTrigger = true; //pouvoir passer à travers
		gameObject.renderer.enabled = false; //voir à travers
	}

	public void SetMurSortie () {
		estMurEntree = false;
		estMurSortie = true;
		collider.isTrigger = true; //pouvoir passer à travers
		gameObject.renderer.enabled = false; //voir à travers
	}

	void OnTriggerEnter (Collider collider) {
		if(collider.gameObject==fpc) //Si le fpc rentre
		{
			if(estMurEntree) {Application.LoadLevel("Labyrinthe");}
			if(estMurSortie) {Application.LoadLevel("Labyrinthe");}
		}
	}

	public void SetCoordinates (int i_,int j_) {
		i = i_;
		j = j_;
	}
}