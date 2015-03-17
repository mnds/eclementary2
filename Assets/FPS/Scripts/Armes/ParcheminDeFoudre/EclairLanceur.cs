/**
 * \file      EclairLanceur.cs
 * \author    Thomas Martin
 * \version   1.0
 * \date      27 février 2015
 * \brief     Indique qu'un objet peut être utilisé pour lancer des éclairs avec Tir 1 et des "bobines Tesla" avec Tir 2.
*/

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))] //Pour le son de l'éclair
public class EclairLanceur : MonoBehaviour {

	public bool bypass;//Si bI, rien ne se passe. Toutes les fonctions sont ignorées.
	public float cooldown = 1f; // Temps entre deux tirs d'éclairs;
	float tempsDuDernierTir = 0f; // Pour savoir si on peut tirer
	private float vitesseBobine = 10f; // la vitesse de la bobine lorsqu'elle part
	private Camera mainCamera; // pour lancer l'objet depuis la position de la caméra
	float rangeEclair = 20f; // au-delà de la range, le tir ne part pas 
	public float degatsEclair = 25f; // suffisant pour oneshot un zombie
	private GameObject target; 

	//pour la génération d'élcairs
	public EclairRender eclairPrefab;
	public GameObject lensFlare;


	public GameObject bobineTesla;

	public float manaEclair = 1f;
	public float manaBobine = 1f;

	AudioSource audioSource; //Pour l'éclair
	public AudioClip clipTir; //Bruit de l'éclair

	// Use this for initialization
	void Start () {
		mainCamera = ControlCenter.GetCameraPrincipale ();
		audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(bypass) return;
		if (ControlCenter.GetCinematiqueEnCours()) return; //Rien pendant une cinématique

		//Si on demande d'attaquer, et que le cooldown est terminé, on lance une bobine
		if(Input.GetButton("Fire2") && Time.time>tempsDuDernierTir+cooldown)
		{
			if(!ControlCenter.GetJoueurPrincipal().GetComponent<Health>().SubirDegatsMana(manaBobine))
				return; //on enlève le mana nécessaire au sort

			tempsDuDernierTir=Time.time;
			//On lance une bobine de type Rigidbody avec une vitesse 25
			GameObject cloneBobine;
			cloneBobine = (GameObject)Instantiate (bobineTesla, mainCamera.transform.position+mainCamera.transform.forward, mainCamera.transform.rotation);
			cloneBobine.rigidbody.AddForce(vitesseBobine*mainCamera.transform.forward,ForceMode.Impulse);
			
		}

		//Si on demande d'attaquer, et que le cooldown est terminé, on tire un éclair
		if(Input.GetButton("Fire1") && Time.time>tempsDuDernierTir+cooldown)
		{

			tempsDuDernierTir=Time.time;
			//On vérifie par Raycast qu'on a touché quelque chose, en partant
			Ray ray = new Ray (mainCamera.transform.position,mainCamera.transform.forward);
			RaycastHit hitInfo;
			
			if(Physics.Raycast(ray,out hitInfo, rangeEclair)){
				if(!ControlCenter.GetJoueurPrincipal().GetComponent<Health>().SubirDegatsMana(manaEclair))
					return; //on enlève le mana nécessaire au sort

				target = hitInfo.collider.gameObject;
				if(target!=ControlCenter.GetJoueurPrincipal()){ //Si l'objet touché n'est pas le tireur
					Vector3 hitPoint = hitInfo.point; //L'endroit qu'on touche
					Debug.Log("Hit Object : "+target.name);

					if(clipTir)
						audioSource.PlayOneShot(clipTir);
					
					//enlever de la vie
					Transform objetAvecVie = target.transform;
					Health health = objetAvecVie.GetComponent<Health>(); //Si le truc touché a des points de vie, on doit le blesser
					while(health == null && objetAvecVie.parent){
						objetAvecVie=objetAvecVie.parent;
						health = objetAvecVie.GetComponent<Health>();
					}

					float degatsInfliges=degatsEclair; //Initialement égal à la valeur "de base"
					if(health != null){
						health.SubirDegats(degatsInfliges);
					}
				}
				    // pour le rendu de l'éclair on instancie trois LineRenderer et lens flare (Michael BAY !!)
					Instantiate (eclairPrefab, mainCamera.transform.position+mainCamera.transform.forward, Quaternion.identity);
					Instantiate (eclairPrefab, mainCamera.transform.position+mainCamera.transform.forward, Quaternion.identity);
					Instantiate (eclairPrefab, mainCamera.transform.position+mainCamera.transform.forward, Quaternion.identity);
					GameObject cloneLensFlare;
				    // on fait apparaitre un Lens Flare de manière à ce qu'il soit visible à l'écran, cad éloigné du point de collision (d'ou le 0.9)
					cloneLensFlare = Instantiate (lensFlare, mainCamera.transform.position+ (hitInfo.point-mainCamera.transform.position)*0.9f, Quaternion.identity) as GameObject;

					Destroy(cloneLensFlare,0.3f);
				}
			}
			}
	
	public void SetBypass(bool bypass_) {
		bypass=bypass_;
	}

	public GameObject GetTarget () {
		return target;
	}
}
