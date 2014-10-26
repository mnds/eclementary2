using UnityEngine;
using System.Collections;

public class Balle : MonoBehaviour {

	float dureeDeVie = 3.0f;
	public GameObject particuleEffect; //On va mettre l'ennemi en feu sans le détruire

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		dureeDeVie -= Time.deltaTime;

		if(dureeDeVie <= 0) {
			Exploser();
		}
	}

	void OnCollisionEnter (Collision collision) {
		if(collision.gameObject.tag == "Ennemi") {
			Destroy (gameObject); //Destruction de l'objet dès qu'il touche quelque chose
			collision.gameObject.tag = "Untagged"; //Pour ne plus qu'il soit considéré comme un ennemi. On pourrait utiliser un autre tag si on voulait en faire autre chose
			Instantiate(particuleEffect,collision.transform.position, Quaternion.identity);
		}
	}

	void Exploser () {
		Destroy (gameObject);
	}
}
