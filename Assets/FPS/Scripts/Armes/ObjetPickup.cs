using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Object states.
/// Les états accessibles pour l'objet
/// </summary>
public enum ObjectStates
{
	//L'objet est inaccessible, par exemple parce qu'il n'y a plus de munition
	NotPickable,
	//L'objet est au sol, pret à etre pris en main
	Pickable,
	//L'objet est dans l'inventaire
	//InInventory,
	//L'objet est dans la main, pret a etre lancé
	InHand,
	//L'objet est utilisé pour frapper
	Attack,
	//L'objet est en train d'etre lancé
	Thrown
}

public class ObjetPickup : MonoBehaviour {
	//State Machine
	public ObjectStates StartState; //Contient une initialisation de l'état de l'objet
	ObjectStates currentObjectState; //L'état courant
	public string pickObjetInput = "Fire2"; //Touche à appuyer pour avoir l'objet
	public float pickDistance = 3.0f;
	public string attackObjetInput = "Fire1";
	public string throwObjetInput = "Fire2"; //Touche pour lancer l'objet


	//Transitions possibles entre les états
	public bool PickToInventory = true;
	public bool PickToInHand = false;
	public bool PickToNotPickable = false;
	public bool InventoryToHand = true;
	public bool HandToThrow = true;
	public bool HandToAttack = true;
	public bool ThrownToPickable = true;
	public bool ThrownToNP = false;

	//Tenir en main l'objet
	public float delaiEntreDeuxTirs = 0.5f;
	float tempsAvantProchainTir; //Contient la valeur de Time.time à dépasser pour pouvoir tirer
	public int munitions = 0; //Nombre de fois où l'objet peut etre lancé
	Inventaire inventaire ; //Il est nécessaire de dire à un inventaire que les munitions changent
	bool estEnTrainDeLancer = false; //true quand on lance. Dicté par tempsAvantProchainTir
	public GameObject objetReel; //L'objet qui sera lancé, celui qui agira sur les méchants
	public Transform departObjetReel; //D'où l'objet réel sera lancé
	public float vitesseDeLance; //La force appliquée sur l'objet réel

	//Attaquer
	public float degatsParCoup = 10.0f; //Degats initiaux, changés pour chaque obje
	public bool enTrainDAttaquer = false; //false pendant un coup
	public Transform placementInitial; //GameObject placé à l'endroit où commence l'objet
	Vector3 positionInitiale;
	Quaternion rotationInitiale;
	public Transform placementFinal;
	Vector3 positionFinale;
	Quaternion rotationFinale;
	public float tempsInitialVersFinal = 0.5f; //Temps initial->final
	public float tempsFinalVersInitial = 0.1f; //Temps final->initial
	float avancementAnim = 0; //variable pour savoir où on en est dans une animation
	bool enCoursDeRetour = false; //false si on doit aller de initial->final, true si final->initial

	//Thrown
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
		currentObjectState = StartState;
	}
	
	// Update is called once per frame
	void Update () {
		//Si en main
		switch (currentObjectState) {
			case ObjectStates.NotPickable:
				return; //L'objet ne peut pas etre sélectionné
				break;
			case ObjectStates.Pickable:
				//Si l'objet va dans l'inventaire
				if(PickToInventory) {
					
				}
			break;
//			case ObjectStates.InInventory:
//				break;
			case ObjectStates.Attack:
				//A réactualiser à chaque fois
				positionInitiale = placementInitial.position;
				rotationInitiale = placementInitial.rotation;
				positionFinale = placementFinal.position;
				rotationFinale = placementFinal.rotation;
				
				if (avancementAnim < 1f) {
					if(!enCoursDeRetour) //On fait l'attaque
					{
						transform.position = Vector3.Lerp(positionInitiale, positionFinale, avancementAnim);
						transform.rotation = Quaternion.Lerp(rotationInitiale,rotationFinale,avancementAnim);
						avancementAnim = avancementAnim + Time.deltaTime/tempsInitialVersFinal;
					}
					else //On revient au début
					{
						transform.position = Vector3.Lerp(positionFinale, positionInitiale, avancementAnim);
						transform.rotation = Quaternion.Lerp(rotationFinale,rotationInitiale,avancementAnim);
						avancementAnim = avancementAnim + Time.deltaTime/tempsFinalVersInitial;
					}
				}
				else //On a fini
				{
					avancementAnim=0; //On remet l'avancement à 0
					if(!enCoursDeRetour) //On a fini l'animation de coup, on signifie qu'on veut retourner au début
					{
						enCoursDeRetour = true;
					}
					else //On est revenu au départ, on dit qu'on n'attaque plus.
					{
						enCoursDeRetour = false;
						currentObjectState = ObjectStates.InHand;
					}
				}
			break;
			case ObjectStates.InHand:
				//On teste si l'objet a encore des munitions
				if(munitions<=0) { //S'il n'y a plus de munitions, on désactive l'objet. Pas de problème pour eETDL, car quand on réactivera l'objet, la dernière ligne de Update sera lue et le mettra comme il faut
					gameObject.SetActive(false); //On désactive l'objet pour l'instant
				}
				if (Input.GetButton(throwObjetInput) && Time.time>=tempsAvantProchainTir) { //Bouton de tir
					currentObjectState = ObjectStates.Thrown;
					tempsAvantProchainTir = Time.time+delaiEntreDeuxTirs;
					GameObject objet = (GameObject)Instantiate(objetReel,departObjetReel.position,departObjetReel.rotation); //On oriente selon la position initiale
					Vector3 forceAppliquee = new Vector3 (vitesseDeLance*objet.transform.forward.x,
					                                      vitesseDeLance*objet.transform.forward.y,
					                                      vitesseDeLance*objet.transform.forward.z);
					objet.rigidbody.AddForce(forceAppliquee,ForceMode.Impulse); //On lui donne une force
					SetMunitions(munitions-1); //On enlève une munition de l'arme
					//On teste si l'objet a encore des munitions
					if(munitions<=0) { //S'il n'y a plus de munitions, on désactive l'objet. Pas de problème pour eETDL, car quand on réactivera l'objet, la dernière ligne de Update sera lue et le mettra comme il faut
						gameObject.SetActive(false); //On désactive l'objet pour l'instant
					}
				}
				estEnTrainDeLancer = Time.time < tempsAvantProchainTir; //Savoir si on est en train de lancer
			break;
			case ObjectStates.Thrown:
				if(aUneDureeDeVie) { //Si l'objet ne reste qu'un certain temps, on vérifie s'il doit etre détruit
					dureeDeVie -= Time.deltaTime;
					if(dureeDeVie <= 0) {
						Disparaitre();
					}
				}
			break;
		}

	}

	void OnTriggerEnter (Collider objet) {
		switch (currentObjectState) {
			case ObjectStates.NotPickable:
				return; //L'objet ne peut pas etre sélectionné
				break;
			case ObjectStates.Pickable:
				break;
//			case ObjectStates.InInventory:
//				break;
			case ObjectStates.Attack:
				GameObject go = objet.gameObject;
				Transform objetAvecVie = go.transform;
				
				//On cherche si l'objet ou un de ses parents a de la vie
				Health health = objetAvecVie.GetComponent<Health>(); //Si le truc touché a des points de vie, on doit le blesser
				while(health == null && objetAvecVie.parent){
					objetAvecVie=objetAvecVie.parent;
					health = objetAvecVie.GetComponent<Health>();
				}
				if(health != null){
					health.SubirDegats(degatsParCoup);
				}
				break;
			case ObjectStates.InHand:
				break;
			case ObjectStates.Thrown:
				break;
		}
	}

	void OnCollisionEnter (Collision collision) {
		switch (currentObjectState) {
			case ObjectStates.NotPickable:
				return; //L'objet ne peut pas etre sélectionné
				break;
			case ObjectStates.Pickable:

					break;
//			case ObjectStates.InInventory:
//				break;
				case ObjectStates.Attack:

					break;

				case ObjectStates.InHand:

					break;

				case ObjectStates.Thrown:
					if (doitExploser) {
						Exploser(collision.collider);
						return;
					}
					//Ensuite, on inflige des dégats à ce qu'on a touché.
					InfligerDegats (collision.gameObject);
					if (doitDisparaitreAprèsAvoirToucheQuelqueChose) {
						Disparaitre ();
						return;
					}
				break;
		}

		//Debug.Log("Touché " + collision.gameObject.name + " à " + Vector3.Distance(collision.transform.position,new Vector3(-4.9f,0f,-33f)));
		//Debug.Log (collision.gameObject.name);
		//Si dégats de zone/spéciaux, on les traite.

	}

	//Méthodes liées à OnCollisionEnter, case Thrown
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
	//Fin des méthodes liées à OnCollisionEnter, case Thrown

	public bool GetEstEnTrainDeLancer () {
		return estEnTrainDeLancer;
	}
	
	public void SetMunitions (int munitions_) {
		munitions = Mathf.Max (0,munitions_);
		inventaire.ChangerMunitions (gameObject, munitions);
	}
	
	public void SetInventaire (Inventaire inventaire_) {
		inventaire = inventaire_;
	}

}
