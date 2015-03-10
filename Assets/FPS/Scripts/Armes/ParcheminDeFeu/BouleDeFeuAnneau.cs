/**
 * \file      BouleDeFeuAnneau.cs
 * \author    
 * \version   1.0
 * \date      27 février 2015
 * \brief     Gère l'anneau de feu créé par BouleDeFeuLanceur
 */


using UnityEngine;
using System.Collections;

public class BouleDeFeuAnneau : MonoBehaviour {

	float tempsRefresh = 0f;
	public float rayonAura = 5f; // zone d'effet en mètres
	public float degatsAura = 2f; // dégats toutes les demi-secondes
	public float dureeRing = 10f; // duree de  l'effet (le cooldown se règle dans BouleDeFeuLanceur)


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > tempsRefresh + 0.5) { // les dégats sont appliqués toutes les 0.5 secondes

			tempsRefresh=Time.time;
			GameObject playerObject = GameObject.Find("Joueur");
			Vector3 pos = playerObject.transform.position; // on prend la position du joueur
			
			Collider[] hitColliders = Physics.OverlapSphere(pos, rayonAura); // on prend tous ceux touchés par la boule centrée sur le joueur
			foreach (Collider collid in hitColliders){
				
				//enlever de la vie SEULEMENT AUX ENNEMIS
				if (collid.gameObject.tag == ("Ennemi")) {
				Transform c=collid.gameObject.transform;
				Health health = c.GetComponent<Health>(); //Si le truc touché a des points de vie, on doit le blesser
				while(health == null && c.transform.parent){
					c=c.transform.parent;
					health = c.GetComponent<Health>();
				}
				
								
				float degatsInfliges=degatsAura; //Initialement égal à la valeur "de base"
				if(health != null){
					health.SubirDegats(degatsInfliges);
				}
			}
			}
				}
	
		Destroy(gameObject,dureeRing); //l'anneau expire au bout de 10 secondes
	}

}
