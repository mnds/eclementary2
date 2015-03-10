/**
 * \file      Inventaire.cs
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

using UnityEngine.UI;

public class Inventaire : MonoBehaviour {
	public List<GameObject> listeObjetsRecoltables; //TOUS les objets possibles
	List<GameObject> listeObjetsUtilisables = new List<GameObject>(); //Tous les objets de quantité supérieure à 1
	public List<int> quantiteObjets; //Match listeObjetsRecoltables. On doit en garder une trace pour d'éventuels changements de scène ou autres traitements
	public List<Sprite> listeImages;//Liste des images de TOUS les objets récoltables
	public Sprite imageTransparente; // Image transparente, utilisée dans les états non jouables
	public Sprite imageRadar;
	public Sprite imageBalayageRadar;

	private GameObject inventaire;//Stock la fenetre d'inventaire
	private GameObject caracteristique;//Stock la fenetre des caracteristiques
	public Sprite imageVide;

	//Objet actuel
	GameObject objetActuel;
	int positionObjetPrecedent; //Utilisé pour stocker l'objet actuel en cas de désactivation de l'inventaire
	//On ne peut pas attaquer et lancer en meme temps ; inventaire se charge de ça
	Attaquer attaquerObjetActuel;
	Lancer lancerObjetActuel;

	int positionScroll = 0; //Endroit où se trouve l'objet sélectionné. Si égal à lOU.Count, c'est qu'on n'est pas équipé
	public Camera camera;

	//Handles glow
	GlowSimple gsAncien;

	// variable booléenne qui servira à l'implémentation des méthodes de IScriptEtatJouable
	private bool enabled;

	/*
	void OnLevelWasLoaded () {
		if (!enabled)
			return;
		//On déclare l'inventaire, qui doit etre activé au départ.
		inventaire=GameObject.Find ("FenetreInventaire");
		inventaire.SetActive(false);
		if(!inventaire)
			Debug.Log("Activez l'objet de nom FenetreInventaire");
	}
	*/

	// Use this for initialization
	void Start () {
		// Initialisée avec l'état du flag 130 
		enabled = FlagManager.ChercherFlagParId(130).actif;
		//On déclare l'inventaire, qui doit etre activé au départ.
		inventaire=GameObject.Find ("FenetreInventaire");
		inventaire.SetActive(false);

		if(!inventaire)
			Debug.Log("Activez l'objet de nom FenetreInventaire");

		//On déclare la fenetre carac, qui doit etre activée au départ
		caracteristique = GameObject.Find ("InterfaceCaracterstique");
		caracteristique.SetActive (false);

		if(!caracteristique)
			Debug.Log("Activez l'objet de nom InterfaceCaracterstique");

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
			//Ensuite, tous les objets qui ont Soigner doivent etre liés à l'inventaire
			Soigner soigner = listeObjetsRecoltables[k].GetComponent<Soigner>();
			if(soigner!=null) {
				soigner.SetInventaire(this);
				soigner.SetMunitions(quantiteObjets[k]);
			}
			//Ensuite, tous les objets qui ont Tirer doivent etre liés à l'inventaire
			Tirer tirer = listeObjetsRecoltables[k].GetComponent<Tirer>();
			if(tirer!=null) {
				tirer.SetInventaire(this);
				tirer.SetMunitions(quantiteObjets[k]);
			}
		}
		objetActuel = listeObjetsUtilisables[0]; //pour ne pas qu'il soit null
		//Choix de l'objet actuel parmi ceux qui effectivement sont dans l'inventaire
		if (positionScroll < listeObjetsUtilisables.Count) //Si on est dans une position acceptable
			ChangerObjetActuel(listeObjetsUtilisables [positionScroll]);
	}
	
	// Update is called once per frame
	void Update () {
		if (!enabled)
			return;
		VerifierTouches ();
	}
	
	void OnGUI() {
		if (!enabled)
			return;
		if (ControlCenter.GetJoueurPrincipal () != gameObject) return; //Si pas le joueur principal
		// The current weapon is always displayed
		if(attaquerObjetActuel && attaquerObjetActuel.vignette)
			GUI.Label( new Rect( 0, 0, 50, 50), attaquerObjetActuel.vignette);
	}

	/**
	 * @brief Change l'objet actuellement équipé.
	 * @param objet_ L'objet à rendre actuel.
	 *
	 * @details On cherche si l'objet (ou un de ses enfants) a un des points de vie. Si oui, on en retire, selon le champ damage de l'objet auquel ce script est attaché.
	 */
	void ChangerObjetActuel(GameObject objet_) {
		//On désactive l'objet actuel s'il existe
		if(objetActuel!=null)
			objetActuel.active = false;
		//On change l'objet
		positionObjetPrecedent = positionScroll;
		objetActuel = objet_;
		//On active ce nouvel objet
		if(objetActuel!=null)
			objetActuel.active = true;
		//On change les attaquer/lancer en mémoire
		if (objetActuel != null) {
			attaquerObjetActuel = objetActuel.GetComponent<Attaquer> ();
			if(attaquerObjetActuel==null)
				attaquerObjetActuel = objetActuel.GetComponentInChildren<Attaquer>();
			
			lancerObjetActuel = objetActuel.GetComponent<Lancer> ();
			if (lancerObjetActuel==null)
				lancerObjetActuel=objetActuel.GetComponentInChildren<Lancer>();
		}

		//On change l'image et le texte du médaillon de rappel de l'arme équipée
		GameObject armeCourante;//Le médaillon qui s'affiche en jeu
		armeCourante=GameObject.Find("Arme Active");

		int positionObjet = 0;//Analogue à positionScroll mais dans la listeObjetsUtilisables

		//On cherche quel objet est l'objet actuel
		for (int i=0;i<listeObjetsRecoltables.Count;i++)
		{
			if(listeObjetsRecoltables[i]==listeObjetsUtilisables[positionScroll])
			{
				positionObjet=i;
			}
		}
		//On remplace l'image par l'image de l'objet en question
		armeCourante.transform.FindChild("Image").gameObject.GetComponent<Image>().sprite=listeImages[positionObjet];
		//Si c'est l'arme nulle, on remplace le texte par rien, sinon on met le nom de l'arme
		if(listeObjetsRecoltables[positionObjet].name=="ArmeNull")
		{
			armeCourante.transform.FindChild("Text").gameObject.GetComponent<Text>().text="";
		}
		else
		{
			armeCourante.transform.FindChild("Text").gameObject.GetComponent<Text>().text=listeObjetsRecoltables[positionObjet].name;
		}
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
		if (ControlCenter.GetJoueurPrincipal () != gameObject) return; //Si pas le joueur principal
		if(ControlCenter.GetCinematiqueEnCours()) return; //Pas d'inventaire si cinématique en cours


		bool inventaireOuvert=false;//Vérifie si l'inventaire est ouvert
		if(inventaire)
			inventaireOuvert = inventaire.activeSelf;//Dit si l'inventaire est déjà ouvert
		ControlCenter.inventaireOuvert = inventaireOuvert;
		
		if(inventaireOuvert && Input.GetButtonDown("Inventaire"))
		{
			inventaire.SetActive(false);//Si on appuie sur la touche et qu'il est ouvert, on ferme
		}
		else if(!inventaireOuvert && Input.GetButtonDown("Inventaire"))
		{
			inventaire.SetActive(true);//Si on appuie et qu'il est fermé on ouvre
			MiseAJourInventaire();
		}

		if(ControlCenter.inventaireOuvert) return;

		bool caracOuverte = false;//Vérifie si la fenetre de caracs est ouverte
		if(caracteristique)
			caracOuverte = caracteristique.activeSelf;//Dit si la fenetre d'inventaire est déjà ouvert
		ControlCenter.caracteristiqueOuvert = caracOuverte;
		
		if(caracOuverte && Input.GetButtonDown("Caracteristique"))
		{
			caracteristique.SetActive(false);//Si on appuie sur la touche et qu'il est ouvert, on ferme
		}
		else if(!caracOuverte && Input.GetButtonDown("Caracteristique"))
		{
			caracteristique.SetActive(true);//Si on appuie et qu'il est fermé on ouvre
		}

		if(ControlCenter.caracteristiqueOuvert) return;


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

		// Current weapon is also changed when tab or Shift+Tab pressed
		int direction = 0;
		// Si on tourne la molette de la souris vers le haut
		if (Input.GetAxis("Mouse ScrollWheel") < 0.0f && !objetActuelEnTrainDAttaquer && !objetActuelEstEnTrainDeLancer)
			direction = -1;
		// Si on tourne la molette de la souris vers le bas
		else if (Input.GetAxis("Mouse ScrollWheel") > 0.0f && !objetActuelEnTrainDAttaquer && !objetActuelEstEnTrainDeLancer)
			direction = 1;
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
			
			while(pickableGameObject == null && copieObjet.transform.parent)
			{
				copieObjet=copieObjet.transform.parent.gameObject;
				pickableGameObject = copieObjet.GetComponent<Pickable>();
			}
			
			//Sinon, on est à la racine, donc on cherche dans les enfants.
			
			if(pickableGameObject==null)
				pickableGameObject=copieObjet.GetComponentInChildren<Pickable>();

			GlowSimple gs;
			gs = objet.GetComponentInChildren<GlowSimple>(); //GlowSimple est sur Graphique, qui n'a pas le collider
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
						   ||nomObjet=="Balle de "+listeObjetsRecoltables[i].name //Pour les balles
						   ||nomObjet=="Balle de "+listeObjetsRecoltables[i].name+"(Clone)" //Pour l'instantiation des balles
						   ||nomObjet==listeObjetsRecoltables[i].name) {
							ChangerMunitions(listeObjetsRecoltables[i],quantiteObjets[i]+pickableGameObject.nombreMunitions);
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
					if (objetActuel==objet) { //On met par défaut l'objet 0. En notant h le rang dans lOU de l'objet actuel, on peut faire quelque chose de plus beau en prenant le rang Mathf.Max(0,h-1) mais ça fait une boucle en plus, d'où plus de complexité
						positionScroll=0;
						ChangerObjetActuel(listeObjetsUtilisables[0]);
					}
				}
				quantiteObjets[k]=munitions; //On change les munitions

				//On s'occupe ensuite de Lancer Soigner et Tirer s'ils existent
				Lancer lancer = listeObjetsRecoltables[k].GetComponent<Lancer>();
				if(lancer!=null) {
					lancer.SetMunitions(quantiteObjets[k]);
				}
				Soigner soigner = listeObjetsRecoltables[k].GetComponent<Soigner>();
				if(soigner!=null) {
					soigner.SetMunitions(quantiteObjets[k]);
				}
				Tirer tirer = listeObjetsRecoltables[k].GetComponent<Tirer>();
				if(tirer!=null) {
					tirer.SetMunitions(quantiteObjets[k]);
				}
				return;
			}
		}
	}

	public void MiseAJourInventaire()//Permet de mettre la visualisation de l'inventaire à jour quand on l'ouvre
	{
		GameObject slotCourant;//Le slot de l'inventaire que l'on va modifier
		GameObject image;//L'image du slot que l'on va modifier
		GameObject texte;//Le nom de l'objet
		int positionSlot = 1;
		
		
		for(int i=0;i<listeObjetsRecoltables.Count;i++)//On parcourt la liste des objets
		{
			for(int k=1;k<listeObjetsUtilisables.Count+1;k++)//On la compare à la liste de tous les objets du joueur
			{
				if(listeObjetsRecoltables[i]==listeObjetsUtilisables[k-1] && listeObjetsRecoltables[i].name!="ArmeNull")//Quand on trouve une correspondance, autre que l'arme nulle
				{
					//On prend le slot qui sera occupé par l'objet
					slotCourant=GameObject.Find("Slot "+ positionSlot);
					image=slotCourant.transform.FindChild("Image").gameObject;
					texte=slotCourant.transform.FindChild("Text").gameObject;
					//On remplace l'image par celle de l'objet
					image.GetComponent<Image>().sprite = listeImages[i];
					//On remplace le texte par le nom de l'objet
					texte.GetComponent<Text>().text = listeObjetsRecoltables[i].name;
					//On passe au slot suivant
					positionSlot++;
				}
			}
		}
		//On remplace maintenant tous les slots inutilisés par "vide"
		for (int i=positionSlot; i<26; i++) 
		{
			//On prend le slot qui sera occupé par l'image vide
			slotCourant=GameObject.Find("Slot "+ i);
			//On remplace l'image par l'image vide
			slotCourant.transform.FindChild("Image").gameObject.GetComponent<Image>().sprite=imageVide;
			//On remplace le texte par rien
			slotCourant.transform.FindChild("Text").gameObject.GetComponent<Text>().text="";
			
		}
		
	}

	public Camera GetCamera () {
		return camera;
	}

	// Implémentation de IScriptEtatJouable
	public bool isEnabled() {
		return enabled;
	}
	
	public void setEnabled( bool ok ) {
		GameObject armeCourante = GameObject.Find ("Arme Active");
		if (!ok) { // Activation de l'inventaire
			// La vignette de l'arme courante est remplacée par une image transparente dans un état non jouable
			//Désactivation de l'arme actuellement utilisée
			ChangerObjetActuel(listeObjetsUtilisables[0]); // L'objet actuel est remplacé par "Arme Null"
			// Désactivation de l'aperçu de l'arme utilisée
			armeCourante.transform.FindChild ("Image").gameObject.GetComponent<Image> ().sprite = imageTransparente;
			armeCourante.transform.FindChild ("Text").gameObject.GetComponent<Text> ().text = "";
			// Désactivation du radar
			GameObject.Find ("Radar").transform.GetComponent<Image>().sprite = imageTransparente;
			GameObject.Find ("Radar_balayage").transform.GetComponent<Image>().sprite = imageTransparente;
		} 
		else { // Activation de l'inventaire
			ChangerObjetActuel( listeObjetsUtilisables[positionScroll] ); // On utilise de nouveau l'objet en main avant désactivation
			int positionObjet = 0;
			for (int i=0;i<listeObjetsRecoltables.Count;i++)
			{
				if(listeObjetsRecoltables[i]==listeObjetsUtilisables[positionScroll])
				{
					positionObjet=i;
				}
			}
			// Activation de l'aperçu de l'arme utilisée
			armeCourante.transform.FindChild("Image").gameObject.GetComponent<Image>().sprite=listeImages[positionObjet];
			// Activation du radar
			GameObject.Find ("Radar").transform.GetComponent<Image>().sprite = imageRadar;
			GameObject.Find ("Radar_balayage").transform.GetComponent<Image>().sprite = imageBalayageRadar;
		}

		enabled = ok; // Mise à jour de l'état
	}
}
