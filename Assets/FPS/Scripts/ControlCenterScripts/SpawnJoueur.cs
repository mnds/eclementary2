using UnityEngine;
using System.Collections;

public class SpawnJoueur : MonoBehaviour {
	public GameObject pointDeDepartJoueur;
	public GameObject joueur;
	// Use this for initialization
	void Start () {
		Instantiate (joueur, pointDeDepartJoueur.transform.position, pointDeDepartJoueur.transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
