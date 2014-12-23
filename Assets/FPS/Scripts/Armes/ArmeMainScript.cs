using UnityEngine;
using System.Collections;

public enum EtatArme {
	Null, //Etat null
	AuSol, //Au sol, prete à etre récupérée
	EnLAir, //En l'air, en attente de contact avec un objet
	EnAttaque, //En train de circuler entre les positions initiales et finales
	EnLance, //En train d'etre lancee, donc "cooldown"
	EnMain //En attente d'action du joueur
}

public class ArmeMainScript : MonoBehaviour {
	public bool bypass;//Si bI, rien ne se passe. Toutes les fonctions sont ignorées.
	public EtatArme etatInitial;
	protected EtatArme etatActuel;

	//Type d'arme
	public bool armeDeTir; //L'arme tire des balles.
	public bool armeDeContact; //L'arme va circuler d'une position A à une position B en infligeant des degats.
	public bool armeDeJet; //L'arme est jetée sur les ennemis
	public bool armeDeSoin; //L'arme peut etre utilisée pour soigner

	//Cooldown
	public float cooldownLancer = 0.5f; //Temps entre deux lancers de l'objet
	float tempsAvantProchaineAction; //Contient la valeur de Time.time à dépasser pour pouvoir tirer

	//Inventaire et munitions
	public int munitions = 0; //Nombre de fois où l'objet peut etre lancé
	Inventaire inventaire ; //Il est nécessaire de dire à un inventaire que les munitions changent. L'inventaire est assigné dans le script Inventaire.
	
	
	bool estEnTrainDeLancer = false; //true quand on lance. Dicté par tempsAvantProchainTir

	//Jet d'un objet
	public GameObject objetReel; //L'objet qui sera lancé, celui qui agira sur les méchants
	public float vitesseDeLance = 5f; //La force appliquée sur l'objet réel

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Cas de retour direct
		if (bypass) return;
		if (ControlCenter.GetCinematiqueEnCours()) return; //Rien pendant une cinématique

		//On teste si l'objet a encore des munitions s'il est en main
		if(munitions<=0) { //S'il n'y a plus de munitions, on désactive l'objet. Pas de problème pour eETDL, car quand on réactivera l'objet, la dernière ligne de Update sera lue et le mettra comme il faut
			gameObject.SetActive(false); //On désactive l'objet pour l'instant
		}

		switch(etatActuel) {
			//Si l'arme est en main, on teste les munitions de l'arme pour savoir si on la désactive, on regarde si des touches sont entrées par le joueur
			case(EtatArme.EnMain):
				if(munitions<=0) {
					gameObject.SetActive(false);
				}
			break;
			default:
			break;

		}
		//Si l'objet est en main, on regarde les touches appuyées par le joueur

		/*
		//Si on demande à tirer, que le cooldown est fini, et qu'on n'est pas en train d'attaquer
		if (Input.GetButtonDown("BoutonJet") && Time.time>=tempsAvantProchainTir && (attaquerGameObject==null || !attaquerGameObject.GetEnTrainDAttaquer())) { //Bouton de tir
			tempsAvantProchainTir = Time.time+delaiEntreDeuxTirs;
			GameObject objet;
			
			//POSER
			if(Input.GetButton ("SecondaryButton")) {//On demande à le poser
				objet = (GameObject)Instantiate(objetReel,inventaire.GetCamera().transform.position+inventaire.GetCamera().transform.forward,Quaternion.identity); //On oriente selon la position initiale
				objet.rigidbody.isKinematic=false; //L'objet se déplacera par une force
				objet.GetComponentInChildren<Collider>().isTrigger=false; //L'objet doit taper les autres objets
			}
			else
			{ //On veut le tirer
				//TIRER
				Vector3 positionLance = inventaire.GetCamera().transform.position+Camera.main.transform.forward; //On tire devant soi
				Debug.Log (positionLance);
				Quaternion rotationLance = inventaire.GetCamera().transform.rotation;
				objet = (GameObject)Instantiate(objetReel,positionLance,rotationLance); //On oriente selon la position initiale
				objet.transform.position=positionLance;
				objet.transform.rotation=rotationLance;
				//Actions à faire pour bien faire fonctionner le nouvel objet
				objet.rigidbody.isKinematic=false; //L'objet se déplacera par une force
				objet.GetComponentInChildren<Collider>().isTrigger=false; //L'objet doit taper les autres objets
				
				//Le tirer
				Vector3 forceAppliquee = new Vector3 (vitesseDeLance*objet.transform.forward.x,
				                                      vitesseDeLance*objet.transform.forward.y,
				                                      vitesseDeLance*objet.transform.forward.z);
				objet.rigidbody.AddForce(forceAppliquee,ForceMode.Impulse); //On lui donne une force
			}
			
			//Traitements d'inventaire
			if(inventaire!=null)
				inventaire.ChangerMunitions (gameObject, munitions-1); //On enlève une munition de l'arme
			else
				Debug.Log ("Pas d'inventaire !");
			
			//On teste si l'objet a encore des munitions
			if(munitions<=0) { //S'il n'y a plus de munitions, on désactive l'objet. Pas de problème pour eETDL, car quand on réactivera l'objet, la dernière ligne de Update sera lue et le mettra comme il faut
				gameObject.SetActive(false); //On désactive l'objet pour l'instant
			}
			
			//On récupère sur cet objet les scripts Lancer, Soigner, ObjetLance et Pickable, et on désactive tout sauf ObjetLancer.
			//Attention, objet est transformé dans le processus.
			Lancer lancer = objet.GetComponent<Lancer>();
			ObjetLance ol = objet.GetComponent<ObjetLance>();
			Pickable pickable = objet.GetComponent<Pickable>();
			Attaquer attaquer = objet.GetComponent<Attaquer>();
			Soigner soigner = objet.GetComponent<Soigner>();
			//Tant que l'un de ces objets est null, on vérifie si ce n'est pas le parent qui a le script
			while((lancer == null||ol==null||pickable==null||soigner==null) && objet.transform.parent){
				objet=objet.transform.parent.gameObject;
				if(lancer==null)
					lancer = objet.GetComponent<Lancer>();
				if(ol==null)
					ol=objet.GetComponent<ObjetLance>();
				if(pickable==null)
					pickable=objet.GetComponent<Pickable>();
				if(attaquer==null)
					attaquer = objet.GetComponent<Attaquer>();
				if(soigner==null)
					soigner = objet.GetComponent<Soigner>();
			}
			//Sinon, on est à la racine, donc on cherche dans les enfants.
			if(lancer == null)
				lancer = objet.GetComponentInChildren<Lancer>();
			if(ol==null)
				ol=objet.GetComponentInChildren<ObjetLance>();
			if(pickable==null)
				pickable=objet.GetComponentInChildren<Pickable>();
			if(attaquer==null)
				attaquer=objet.GetComponentInChildren<Attaquer>();
			if(soigner==null)
				soigner=objet.GetComponentInChildren<Soigner>();
			
			if(lancer!=null)
				lancer.SetBypass(true); //Lancer désactivé
			if(ol!=null) {
				ol.SetBypass(false);//ObjetLance actif
				ol.SetLanceurObjet(gameObject); //Pour ne pas frapper le lanceur
			}
			if(pickable!=null)
				pickable.SetBypass(true);//Pickable désactivé
			if(attaquer!=null)
				attaquer.SetBypass(true);//Attaquer désactivé
			if(soigner!=null)
				soigner.SetBypass(true); //Soigner désactivé
			
		}
		estEnTrainDeLancer = Time.time < tempsAvantProchainTir; //Savoir si on est en train de lancer
*/
	}
}
