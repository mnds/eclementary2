using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// NOTE IMPORTANTE
// Tous les graphiques, là où on met le script Attaquer, doivent avoir pour nom "Graphique"


//** listeUtilisables ne sera jamais vide, il y aura tjs le coup de poing. Ou un truc qui sert à rien
public class Inventaire : MonoBehaviour {
	public List<GameObject> listeObjetsRecoltables; //TOUS les objets possibles
	List<GameObject> listeObjetsUtilisables = new List<GameObject>(); //Tous les objets de quantité supérieure à 1
	public List<int> quantiteObjets; //Match listeObjetsRecoltables. On doit en garder une trace pour d'éventuels changements de scène ou autres traitements

	//Objet actuel
	GameObject objetActuel;
	Attaquer attaquerObjetActuel;
	Lancer lancerObjetActuel;

	int positionScroll = 0; //Endroit où se trouve l'objet sélectionné. Si égal à lOU.Count, c'est qu'on n'est pas équipé
	public Camera camera;

	// Use this for initialization
	void Start () {
		for(int k=0;k<listeObjetsRecoltables.Count;k++) //On récupère les objets de quantité non nulle
		{
			if(quantiteObjets[k]>0) //Si l'objet est en quantité non nulle
				listeObjetsUtilisables.Add (listeObjetsRecoltables[k]);
			//Ensuite, tous les objets qui ont Lancer doivent etre liés à l'inventaire
			Lancer lancer = listeObjetsRecoltables[k].GetComponent<Lancer>();
			if(lancer!=null) {
				lancer.SetInventaire(this);
				lancer.SetMunitions(quantiteObjets[k]);
			}
		}
		objetActuel = listeObjetsUtilisables[0]; //pour ne pas qu'il soit null
		//Choix de l'objet actuel parmi ceux qui effectivement sont dans l'inventaire
		if (positionScroll < listeObjetsUtilisables.Count) //Si on est dans une position acceptable
			ChangerObjetActuel(listeObjetsUtilisables [positionScroll]);
	}
	
	// Update is called once per frame
	void Update () {
		VerifierTouches ();
	}

	void ChangerObjetActuel(GameObject objet_) {
		//On désactive l'objet actuel s'il existe
		if(objetActuel!=null)
			objetActuel.active = false;
		//On change l'objet
		objetActuel = objet_;
		//On active ce nouvel objet
		if(objetActuel!=null)
			objetActuel.active = true;
		//On change les attaquer/lancer en mémoire
		attaquerObjetActuel = objetActuel.GetComponent<Attaquer> ();
		if(attaquerObjetActuel==null)
			attaquerObjetActuel = objetActuel.GetComponentInChildren<Attaquer>();

		lancerObjetActuel = objetActuel.GetComponent<Lancer> ();
		if (lancerObjetActuel==null)
			lancerObjetActuel=objetActuel.GetComponentInChildren<Lancer>();
	}

	void VerifierTouches()
	{
		//On vérifie si l'objet est utilisé pour attaquer ou s'il est lancé
		bool objetActuelEnTrainDAttaquer = false;
		if(attaquerObjetActuel!=null) //Si l'objet a un Attaquer, on regarde si l'objet attaque
			objetActuelEnTrainDAttaquer = attaquerObjetActuel.GetEnTrainDAttaquer ();
		
		bool objetActuelEstEnTrainDeLancer = false;
		if(lancerObjetActuel!=null) //Si l'objet a un Attaquer, on regarde si l'objet attaque
			objetActuelEstEnTrainDeLancer = lancerObjetActuel.GetEstEnTrainDeLancer ();
		
		//On ne peut pas changer d'arme si l'objet est en train d'attaquer ou d'etre lance
		if (Input.GetButtonDown ("ScrollItemsDown") && !objetActuelEnTrainDAttaquer && !objetActuelEstEnTrainDeLancer) {
			positionScroll--;

			if(positionScroll<0) //Si position non valide, on va dans la case "null" : non équipé 
				positionScroll=listeObjetsUtilisables.Count-1; //On remonte la boucle
			ChangerObjetActuel(listeObjetsUtilisables[positionScroll]);
			return;
		}
		if (Input.GetButtonDown ("ScrollItemsUp") && !objetActuelEnTrainDAttaquer && !objetActuelEstEnTrainDeLancer) {
			positionScroll++;
			if(positionScroll>listeObjetsUtilisables.Count-1) //Si position non valide, on va dans la case "null" : non équipé 
				positionScroll=0; //On redescend la boucle
			ChangerObjetActuel(listeObjetsUtilisables[positionScroll]);
			return;
		}
		KeyCode nombre = KeyCode.Alpha1;
		for(int i=0;i<9;i++) {
			if(Input.GetKey(nombre )) {
				positionScroll=i;
				if (positionScroll >= 0 && positionScroll < listeObjetsUtilisables.Count) {
					ChangerObjetActuel (listeObjetsUtilisables [positionScroll]);
					return;
				}
			}
			else
			{
				nombre+=1;
			}
		}

		//Tester si un objet est dans le champ de vision
		RaycastHit hitInfo;
		Pickable pickableGameObject;
		GameObject objet;
		
		if (Input.GetButtonDown ("InteractionButton")) {
			if(Physics.Raycast(camera.transform.position, camera.transform.forward,out hitInfo, 300f))				
			{
				objet = hitInfo.collider.gameObject;
				GameObject copieObjet = objet;
				pickableGameObject = copieObjet.GetComponent<Pickable>();
				
				while(pickableGameObject == null && objet.transform.parent)
				{
					copieObjet=copieObjet.transform.parent.gameObject;
					pickableGameObject = copieObjet.GetComponent<Pickable>();
				}
				
				//Sinon, on est à la racine, donc on cherche dans les enfants.
				
				if(pickableGameObject==null)
					pickableGameObject=copieObjet.GetComponentInChildren<Pickable>();
				
				//On vérifie qu'on a trouvé un pickable, que l'objet est prenable, et qu'on est assez près
				if(pickableGameObject!=null && pickableGameObject.GetPickable() 
				   && pickableGameObject.GetPickableDistance()>Vector3.Distance(hitInfo.point,camera.transform.position))
				{
					//detruire le parent
					while(objet.transform.parent)
					{
						objet=objet.transform.parent.gameObject;
					}
					Destroy (objet);
					//ajouter a l'inventaire
					string nomObjet=objet.name;//nom de l'objet
					for(int i=0;i<listeObjetsRecoltables.Count;i++)
					{
						if(nomObjet==listeObjetsRecoltables[i].name+"(Clone)") {
							ChangerMunitions(listeObjetsRecoltables[i],quantiteObjets[i]+1);
							//recuperation de lancer
							GameObject lancerGameObject = listeObjetsRecoltables[i]; //On va parcourir les parents de gameObject pour trouver les scripts
							Lancer lancerDeObjet = lancerGameObject.GetComponent<Lancer>();
							//Tant que lancer est null, on vérifie si ce n'est pas le parent qui a le script
							while(lancerDeObjet == null && lancerGameObject.transform.parent){
								lancerGameObject=lancerGameObject.transform.parent.gameObject;
								lancerDeObjet = lancerGameObject.GetComponent<Lancer>();
							}
							//Sinon, on est à la racine, donc on cherche dans les enfants.
							if(lancerDeObjet==null)
								lancerDeObjet=objet.GetComponentInChildren<Lancer>();
							//On rajoute ce qu'il faut à Lancer
							lancerDeObjet.SetMunitionsSimple(quantiteObjets[i]);
							break;
						}
						
					}
				}
				
			}
		}
	}

	public void ChangerMunitions(GameObject objet, int munitions) {
		//On cherche objet dans lOR, et on lui met les munitions associées. Sinon, l'objet n'existe pas, on renvoie un message dans la console
		for (int k=0; k<listeObjetsRecoltables.Count; k++) {
			if(listeObjetsRecoltables[k]==objet) 
			{//Dès qu'on a le bon objet
				if(quantiteObjets[k]<=0 && munitions>0) //S'il n'y avait plus cet objet et qu'on en ajoute, on le met dans les objets utilisables
					listeObjetsUtilisables.Add (objet);
				if(quantiteObjets[k]>0 && munitions==0) {//S'il y en avait et que maintenant il n'y en a plus, on enlève de lOU, en changeant au cas où le rang de l'objetActuel
					listeObjetsUtilisables.Remove(objet);
					if (objetActuel==objet) //On met par défaut l'objet 0. En notant h le rang dans lOU de l'objet actuel, on peut faire quelque chose de plus beau en prenant le rang Mathf.Max(0,h-1) mais ça fait une boucle en plus, d'où plus de complexité
						ChangerObjetActuel(listeObjetsUtilisables[0]);
				}
				quantiteObjets[k]=munitions; //On change les munitions
				return;
			}
		}
	}
}
