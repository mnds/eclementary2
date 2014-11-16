/**
 * \file      ObjetLance.cs
 * \author    
 * \version   1.0
 * \date      19 octobre 2014
 * \brief     Script attaché aux objets lancés.
 *
 * \details   Ce script, placé sur un objet, indique que l'objet est lancé. Une fois lancé, un objet peut avoir plusieurs comportements ; il peut disparaître après un certain temps,
 *			  infliger des dégâts en touchant quelque chose, exploser... Tous ces comportements sont rassemblés dans ce script.
 *			  A noter : si l'objet est à la fois ramassable et lancable, le *meme* prefab doit contenir ObjetLance.cs et Pickable.cs.
 */

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class ObjetLance : MonoBehaviour {
	public bool bypass; //Si bI, rien ne se passe. Toutes les fonctions sont ignorées.

	public bool aUneDureeDeVie = true; //Si l'objet disparait après un certain temps
	public float dureeDeVie = 30.0f; //Temps avant la destruction
	
	public float damage = 5f; //Dégats max, au centre

	//si explosif. Utiliser disparitionPrefab pour l'explosion
	public bool doitExploser = false ;
	public float explosionRadius = 3f; //Zone d'impact
	public Vector3 detonationPoint; //L'endroit où ça explose sur l'objet

	//si l'objet disparait après avoir touché quelque chose
	public bool doitDisparaitreAprèsAvoirToucheQuelqueChose = false ;
	public GameObject disparitionPrefab; //Si un prefab est généré lors de la disparition

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (bypass) return;

		if(aUneDureeDeVie) { //Si l'objet ne reste qu'un certain temps, on vérifie s'il doit etre détruit
			dureeDeVie -= Time.deltaTime;
			if(dureeDeVie <= 0) {
				Disparaitre();
			}
		}
	}

	void OnCollisionEnter (Collision collision) {
		if (bypass) return;
		//Debug.Log("Touché " + collision.gameObject.name + " à " + Vector3.Distance(collision.transform.position,new Vector3(-4.9f,0f,-33f)));
		//Debug.Log (collision.gameObject.name);
		//Si dégats de zone/spéciaux, on les traite.
		if (doitExploser) {
			Exploser(collision.collider);
			return;
		}

		//Ensuite, on inflige des dégats à ce qu'on a touché.
		InfligerDegats (collision.gameObject);

		if (doitDisparaitreAprèsAvoirToucheQuelqueChose || doitExploser) {
			Disparaitre ();
			return;
		}
		else //L'objet ne disparait pas. On change le bypass : ObjetLance disparait, Pickable revient.
		{
			GameObject objet = gameObject; //On va parcourir les parents de gameObject pour trouver les scripts
			//On récupère sur cet objet les scripts Lancer, ObjetLance et Pickable, et on désactive tout sauf ObjetLancer.
			//Attention, objet est transformé dans le processus.
			Lancer lancer = objet.GetComponent<Lancer>();
			ObjetLance ol = objet.GetComponent<ObjetLance>();
			Pickable pickable = objet.GetComponent<Pickable>();
			//Tant que l'un de ces objets est null, on vérifie si ce n'est pas le parent qui a le script
			while((lancer == null||ol==null||pickable==null) && objet.transform.parent){
				objet=objet.transform.parent.gameObject;
				if(lancer==null)
					lancer = objet.GetComponent<Lancer>();
				if(ol==null)
					ol=objet.GetComponent<ObjetLance>();
				if(pickable==null)
					pickable=objet.GetComponent<Pickable>();
			}
			//Sinon, on est à la racine, donc on cherche dans les enfants.
			if(lancer == null)
				lancer = objet.GetComponentInChildren<Lancer>();
			if(ol==null)
				ol=objet.GetComponentInChildren<ObjetLance>();
			if(pickable==null)
				pickable=objet.GetComponentInChildren<Pickable>();
			
			if(lancer!=null)
				lancer.SetBypass(true); //Lancer désactivé
			if(ol!=null)
				ol.SetBypass(true); //ObjetLance désactivé
			if(pickable!=null)
				pickable.SetBypass(false); //Pickable actif 
		}
	}

	/**
	 * @brief Inflige des dégâts à un objet.
	 * @param objet L'objet touché.
	 *
	 * @details On cherche si l'objet (ou un de ses enfants) a un des points de vie. Si oui, on en retire, selon le champ damage de l'objet auquel ce script est attaché.
	 */
	void InfligerDegats(GameObject objet) {
		//enlever de la vie
		Transform objetAvecVie = objet.transform;
		Health health = objetAvecVie.GetComponent<Health>(); //Si le truc touché a des points de vie, on doit le blesser
		while(health == null && objetAvecVie.parent){
			objetAvecVie=objetAvecVie.parent;
			health = objetAvecVie.GetComponent<Health>();
		}
		if(health != null){
			health.SubirDegats(damage);
		}
	}

	/**
	 * @brief Gère la destruction de l'objet auquel ce script est attaché.
	 *
	 * @details On détruit l'objet. Si un effet de particules doit être lancé lors de sa destruction, on le fait.
	 */
	void Disparaitre () {
		Destroy (gameObject);
		if (disparitionPrefab != null) {
			Instantiate(disparitionPrefab,transform.position, Quaternion.identity);
		}
	}

	/**
	 * @brief Gère l'explosion de l'objet auquel ce script est attaché.
	 * @param objet Le collider de l'objet touché.
	 *
	 * @details On détruit l'objet auquel le script est attaché. En explosant, cet objet va infliger des dégâts dans un certain rayon, déterminé par explosionPoint et explosionRadius.
	 *          Les objets dans la zone de l'explosion vont subir les dégâts selon la distance à laquelle ils se situent du point d'impact de l'explosif.
	 */
	void Exploser(Collider objet) {
		Vector3 explosionPoint = transform.position + detonationPoint; //pour que l'explosion commence au niveau du bout du missile
		if (disparitionPrefab != null) { //Particule effect associé
			Instantiate(disparitionPrefab,explosionPoint,Quaternion.identity);
		}
		Destroy (gameObject);

		//Explosion: infliger des degats
		Collider[] collidersInRadius = Physics.OverlapSphere (explosionPoint, explosionRadius);
		foreach(Collider collider in collidersInRadius) {
			//enlever de la vie
			Transform objetAvecVie = collider.gameObject.transform;
			Health health = objetAvecVie.GetComponent<Health>(); //Si le truc touché a des points de vie, on doit le blesser
			while(health == null && objetAvecVie.parent){
				objetAvecVie=objetAvecVie.parent;
				health = objetAvecVie.GetComponent<Health>();
			}
			if(health != null){
				float dist = Vector3.Distance(explosionPoint,collider.transform.position);
				Debug.Log(dist);
				float damageRatio = Mathf.Clamp(1f - (dist/explosionRadius),0f,1f);
				Debug.Log (collider.gameObject+" subit "+damage*damageRatio+" degats");
				health.SubirDegats(damage*damageRatio);
			}
		}
	}

	public void SetBypass(bool bypass_) {
		bypass = bypass_;
	}
}
