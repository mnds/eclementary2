/**
 * \file      Attaquer.cs
 * \author    
 * \version   1.0
 * \date      9 novembre 2014
 * \brief     Gère tout ce qui a attrait à la gestion des objets et leur récupération.
 *
 * \details   Contient une liste de tous les objets récoltables dans le jeu, ainsi qu'une liste correspondant notant la quantité de chacun de ces objets dans l'inventaire.
 * 	          A en mémoire l'objet actuellement équipé par le joueur, ainsi que ses scripts Attaquer (pour frapper) et Lancer (pour le jeter) s'ils existent
 * 			  Utilise constamment un Raycast pour analyser l'objet directement au centre de l'écran. S'il possède un script de surbrillance et qu'on est assez proche,
 * 			  on l'active. Si l'objet au centre a un script Pickable, on récupère cet objet par la touche d'interaction.
 */

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

	//Handles glow
	GlowSimple gsAncien;

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
	
	void OnGUI() {
		// The current weapon is always displayed
		if(attaquerObjetActuel)
			if(attaquerObjetActuel.vignette)
				GUI.Label( new Rect( 0, 0, 50, 50), attaquerObjetActuel.vignette);
	}

	/**
	 * @brief Change l'objet actuellement équipé.
	 * @param objet L'objet à rendre actuel.
	 *
	 * @details On cherche si l'objet (ou un de ses enfants) a un des points de vie. Si oui, on en retire, selon le champ damage de l'objet auquel ce script est attaché.
	 */
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


	/**
	 * @brief Finds the new value of PositionScroll, according to listeObjetsUtilisables.
	 * @param direction Relative to positionScroll: -1 to move in negatively, +1 to move positively (0 => no change)
	 * @return positionScroll-1 or positionScroll+1 if possible, or extremum position (0 or the size of listeObjetsUtilisables minus 1) after looping
	 *
	 */
	int NewPositionScroll( int direction ) {
		int newPosition = positionScroll + direction ; //temporary position

		// loop if no reachable position
		if (newPosition < 0 )
			newPosition = listeObjetsUtilisables.Count - 1;
		else if (newPosition > listeObjetsUtilisables.Count - 1)
			newPosition = 0;

		return newPosition;
	}

	/**
	 * @brief Vérifie toutes les touches appuyées et agit en conséquence.
	 *
	 * @details Vérifie si l'objet est en train d'attaquer, ou en train d'etre lancé, et relève deux booléens en conséquence. Pour cela, on utilise les scripts Attaquer/Lancer s'ils existent
	 * 			Si l'une de ces conditions et vérifiée, il est impossible de changer d'objet.
	 * 			On peut changer d'objet avec les combinaisons tab/shift+tab, les touches alphanumériques, et les deux touches ScrollItems.
	 * 			On teste ensuite si un objet est dans le champ de vision. Si la touche d'interaction est utilisée et que l'objet est récupérable, on l'ajoute à l'inventaire.
	 */
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
			positionScroll = NewPositionScroll(-1); // searching new position in the negative sense
			ChangerObjetActuel(listeObjetsUtilisables[positionScroll]);
			return;
		}
		if (Input.GetButtonDown ("ScrollItemsUp") && !objetActuelEnTrainDAttaquer && !objetActuelEstEnTrainDeLancer) {
			positionScroll = NewPositionScroll(+1); // // searching new position in the positive sense
			ChangerObjetActuel(listeObjetsUtilisables[positionScroll]);
			return;
		}

		// Cette méthode ne marche pas sur tous les claviers, les azerty sur mac notamment
		KeyCode nombre = KeyCode.Alpha1;
		for(int i=0;i<9;i++) {
			if(Input.GetKey(nombre+i)) {
				positionScroll=i;
				if (positionScroll >= 0 && positionScroll < listeObjetsUtilisables.Count 
				    	&& !objetActuelEnTrainDAttaquer && !objetActuelEstEnTrainDeLancer) {
					ChangerObjetActuel (listeObjetsUtilisables [positionScroll]);
					return;
				}
			}
		}

		// Current weapon is also changed when tab or Shift+Tab pressed
		int direction = 0;
		// Combo Shift+Tab si on n'attaque ni ne lance
		if ((Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) && Input.GetKeyUp (KeyCode.Tab)
		    	&& !objetActuelEnTrainDAttaquer && !objetActuelEstEnTrainDeLancer)
			direction = -1;
		// When only Tab is pressed
		else if( Input.GetKeyUp (KeyCode.Tab) && !objetActuelEnTrainDAttaquer && !objetActuelEstEnTrainDeLancer )
			direction = +1;
		if (direction == -1 || direction == 1) {
			positionScroll = NewPositionScroll (direction);
			ChangerObjetActuel (listeObjetsUtilisables [positionScroll]);
		}

		//Tester si un objet est dans le champ de vision
		RaycastHit hitInfo;
		Pickable pickableGameObject;
		GameObject objet;

		if(Physics.Raycast(camera.transform.position, camera.transform.forward,out hitInfo, 10f))				
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

			GlowSimple gs;
			gs = objet.GetComponent<GlowSimple>();
			if(gs) { //Si le composent a un GlowSimple
				if(gsAncien && gs!=gsAncien) //Si on a changé de GlowSimple, il faut désactiver l'ancien
					gsAncien.DesactivateGlow();
				gsAncien=gs; //On stocke le nouveau gs
			}
			else
			if(gsAncien)
					gsAncien.DesactivateGlow();

			//On vérifie qu'on a trouvé un pickable, que l'objet est prenable, et qu'on est assez près
			if(pickableGameObject!=null && pickableGameObject.GetPickable() 
			   && pickableGameObject.GetPickableDistance()>Vector3.Distance(hitInfo.point,camera.transform.position))
			{
				if(gs) gs.ActivateGlow(); //La surbrillance ne s'active que si l'objet est pickable
				if (Input.GetButtonDown ("InteractionButton")) {
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
						if(nomObjet==listeObjetsRecoltables[i].name+"(Clone)" //Pour l'instantiation
						   ||nomObjet==listeObjetsRecoltables[i].name) {
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
			else {
				if(gs) gs.DesactivateGlow();
				if(gsAncien) gsAncien.DesactivateGlow ();
			}
				
		}
		else
			if(gsAncien) { //Plus rien en vue. On regarde s'il faut désactiver une surbrillance. Si oui, on le fait, et on détruit l'objet pour ne pas avoir à refaire ça à chaque fois.
			gsAncien.DesactivateGlow();
			gsAncien=null;
			}
	}


	/**
	 * @brief Setter de munitions sur un objet.
	 *
	 * @details Utilisé quand on ramasse un objet, en utilisant en parallèle la liste quantiteObjets.
	 * 			Si munitions est strictement positif et que l'objet n'est pas encore dans la liste des objets utilisables, on l'y ajoute.
	 * 			Si munitions est nul et que l'objet est dans cette meme liste, on l'en enlève. On change également l'objet actuel si tel est le cas pour ne pas avoir de problèmes d'indexation.
	 */
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

	public Camera GetCamera () {
		return camera;
	}
}
