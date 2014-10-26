using UnityEngine;
using System.Collections;

//Destruction de l'objet après un certain temps
public class Autodestruction : MonoBehaviour {
	public float dureeDeVie = 1.0f;
	// Use this for initialization
	void Update () {
		dureeDeVie -= Time.deltaTime;
		if (dureeDeVie <= 0) {
			Destroy (gameObject);
		}
	}
}
