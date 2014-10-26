using UnityEngine;
using System.Collections.Generic;

//crée un labyrinthe au niveau d'un GameObject situé au centre du mur placé
//horizontalement au coin en bas à gauche dans un repère x, z
//avec x vers la droite et z vers le haut.
public class MazeManager : MonoBehaviour {
	[HideInInspector] public List<GameObject> lesMurs;
	[HideInInspector] public List<GameObject> lesTriggers;
	public List<GameObject> lesPNJ;
	public List<GameObject> lesObjets; //Liste d'objets sans importance pour le déroulement du jeu. Servent à égayer la partie
	private GameObject fpc;
	private int longueurMurs = 5; //la longueur est égale à la hauteur ici.
	public int nombreMursParCote = 10;
	public Transform mazeWall; //prefab du mur
	public Transform mazeTrigger; //prefab des cubes qui tapissent le labyrinthe
	private int iFpc; //Position i du Fpc
	private int jFpc; //Position j du Fpc
	private int jEntree; //Position j du mur d'entrée, i=0
	private int jSortie; //Position j du mur de sortie, i=nombreMursParCote
	//Variables pour l'intelligence artificielle
	public List< List< List<int> > > matriceDeplacementsPossibles; //Pour chaque position i,j, indique quels sont les déplacements possibles sous la meme forme que les autres vecteurs de déplacement
	//Variables des événements
	public Transform mdz;
	private int iDuGouffre = 12;
	private int jDuGouffre = 10;

	void Start () {
		fpc = GameObject.Find ("First Person Controller");
		matriceDeplacementsPossibles = new List<List<List<int>>> ();
		GenererLabyrinthe (); //Crée et positionne les murs du labyrinthe
		PlacerPersonnages (); //place les personnages aléatoirement dans le labyrinthe
		PlacerObjets (); //place les objets aléatoirement dans le labyrinthe
		//Donne les informations nécessaires aux PNJ pour initialiser la recherche du FPC
		for(int i=0;i<lesPNJ.Count;i++) {
			lesPNJ[i].GetComponent<MazeTravel>()
				.SetLongueurNombrePosition(longueurMurs,nombreMursParCote,transform.position);
			lesPNJ[i].GetComponent<MazeEnemy>()
				.SetMazeManager(this);
		}
		//MazeDeathZone
		mdz.gameObject.GetComponent<MazeDeathZone>().SetFPC (fpc);
		mdz.gameObject.GetComponent<MazeDeathZone>().SetMazeManager (this);
	}

	void Update () {
		if (Input.GetKeyUp (KeyCode.P))
						PlacerPersonnages ();

		if (Input.GetKeyUp (KeyCode.R))
						RestartLevel ();
	}

	//Relance le niveau. Pour l'instant, ne garde rien en mémoire
	private void RestartLevel () {
		matriceDeplacementsPossibles = new List<List<List<int>>> ();
		GenererLabyrinthe (); //Crée et positionne les murs du labyrinthe
		PlacerPersonnages (); //place les personnages aléatoirement dans le labyrinthe
		RemplirMatriceDeplacementsPossibles (); //Avant que les personnages ne soient placés, détecte les directions dans lesquelles le PNJ peut se déplacer, sans tenir compte des autres personnages ou objets
		//Donne les informations nécessaires aux PNJ pour initialiser la recherche du FPC
		for(int i=0;i<lesPNJ.Count;i++) {
			lesPNJ[i].GetComponent<MazeTravel>()
				.SetLongueurNombrePosition(longueurMurs,nombreMursParCote,transform.position);
			lesPNJ[i].GetComponent<MazeEnemy>()
				.SetMazeManager(this);
		}
	}

	public void GenererLabyrinthe () {
		//Détruire tout le labyrinthe
		while(lesMurs.Count!=0)
		{
			Destroy(lesMurs[lesMurs.Count-1]);
			lesMurs.RemoveAt(lesMurs.Count-1);
		}
		while(lesTriggers.Count!=0)
		{
			Destroy(lesTriggers[lesTriggers.Count-1]);
			lesTriggers.RemoveAt(lesTriggers.Count-1);
		}
		//Mise en place des murs de sortie
		jEntree=Random.Range(0,nombreMursParCote-1);
		jSortie=Random.Range(0,nombreMursParCote-1);

		//Vérifier que la sortie n'est pas trop près du gouffre en iGouffre,jGouffre
		if(Mathf.Abs(nombreMursParCote-1-iDuGouffre)<3)
			while(Mathf.Abs(jSortie-jDuGouffre)<4)
				jSortie=Random.Range(0,nombreMursParCote-1);

		//créer tous les murs et les placer. Ils tournent d'eux-memes
		for(int i=0;i<nombreMursParCote;i++)
		{
			for(int j=0;j<nombreMursParCote;j++)
			{
				//Variables qui vont contenir successivement les murs et mwm
				Transform mur;
				//Opérations sur le mur
				mur=Instantiate(mazeWall) as Transform;
				//lors de l'instantiation, le mur tourne
				float positionMurXf=longueurMurs*i;
				float positionMurYf=longueurMurs/2;
				float positionMurZf=longueurMurs*j;
				//Changer la position du mur
				mur.gameObject.transform.position = transform.position + 
					new Vector3(positionMurXf,positionMurYf,positionMurZf);
				mur.transform.parent=this.transform; //le mur est enfant du GameObject qui tient ce script
				lesMurs.Add(mur.gameObject); //ajoute le mur à la liste des murs constituant le labyrinthe
				
				//Opérations liées au script MazeWallManager
				MazeWallManager mwm;
				mwm=mur.gameObject.GetComponent<MazeWallManager>();
				mwm.SetAxeRotation(); //prend en mémoire l'axe de rotation et tourne le mur
				mwm.SetCoordinates(i,j);
				mwm.SetFpc(fpc);
				
				//Orientation des murs extérieurs
				if(i==0){mwm.Tourner(270);}
				if(j==nombreMursParCote-1){mwm.Tourner(0);}
				if(i==nombreMursParCote-1){mwm.Tourner(90);}
				if(j==0&&i!=0){mwm.Tourner(180);}
				
				//Murs d'entrée et sortie
				if(i==0 && j==jEntree) {mwm.SetMurEntree();}
				if(i==nombreMursParCote-1 && j==jSortie) {mwm.SetMurSortie();}
				
				if(i<nombreMursParCote-1 && j<nombreMursParCote-1) //Pour que les cubes restent dans le labyrinthe
				{
					//Ajouter les cubes servant à savoir où se trouve le fpc
					Transform cube;
					cube = Instantiate(mazeTrigger) as Transform;
					cube.transform.position = transform.position + 
						new Vector3(positionMurXf,positionMurYf,positionMurZf) + //position du mur
							new Vector3(0,0,(float)longueurMurs/2+1/2); //le décaler pour le mettre dans les intersections d'une longueur + une largeur de mur sur 2
					cube.transform.localScale=new Vector3(longueurMurs,longueurMurs,longueurMurs);
					lesTriggers.Add(cube.gameObject);
					cube.transform.parent=this.transform; //le cube est enfant du GameObject qui tient ce script
					
					//Actions liées au script MazeTriggerManager
					MazeTriggerManager mtm;
					mtm=cube.gameObject.GetComponent<MazeTriggerManager>();
					mtm.SetCoordinates(i,j);
					mtm.SetFPC(fpc);
					mtm.SetMazeManager(this);
				}
			}
		}
		PurgerLesMurs ();
		//Debug.Log ("Labyrinthe généré");
	}

	//Retire tous les murs qui existent déjà pour éviter les bugs de texture
	public void PurgerLesMurs () {
		List<GameObject> nouveauLesMurs = new List<GameObject>();
		while(lesMurs.Count!=0)
		{
			GameObject mur=lesMurs[lesMurs.Count-1];
			lesMurs.RemoveAt(lesMurs.Count-1); //On retire le dernier élément de lesMurs
			bool ajouter=true; //On regarde si le mur est déjà en place dans la liste
			for(int i=0;i<nouveauLesMurs.Count;i++)
			{
				if(nouveauLesMurs[i].transform.position==mur.transform.position)
				{
					ajouter=false;
				}
			}
			if(ajouter)
			{
				nouveauLesMurs.Add(mur);
			}
			else
			{
				Destroy(mur);
			}
		}
		lesMurs = nouveauLesMurs;
	}

	public void RemplirMatriceDeplacementsPossibles () {
		//Changer de layer
		int fpcLayer = fpc.layer;
		fpc.layer=2; //layer qui empeche les prochains Raycast de toucher l'objet
		List<int> pnjLayers = new List<int> ();
		for (int i=0;i<lesPNJ.Count;i++) {
			pnjLayers.Add(lesPNJ[i].layer);
			lesPNJ[i].layer=2; //layer IgnoreRaycast
		}
		List<int> objetsLayers = new List<int> ();
		for (int i=0;i<lesObjets.Count;i++) {
			objetsLayers.Add(lesObjets[i].layer);
			lesObjets[i].layer=2; //layer IgnoreRaycast
		}

		//remplissage de la matrice
		List<List<List<int>>> matriceDeplacementsPossibles_ = new List<List<List<int>>> (); //nouvelle liste
		for(int i=0;i<nombreMursParCote-1;i++)
		{
			List<List<int>> listePourI = new List<List<int>>();
			for(int j=0;j<nombreMursParCote-1;j++)
			{
				List<int> listePourIJ = new List<int>();
				//Utilisation des Raycasts pour voir où aller
				Vector3 positionInitiale = transform.position + 
					new Vector3(longueurMurs*i,longueurMurs/2,longueurMurs*j) + //position du mur
					new Vector3(0,0,(float)longueurMurs/2+1/2);

				if(!Physics.Raycast(positionInitiale,Vector3.left,longueurMurs)) //si pas de mur à gauche
				{
					listePourIJ.Add(i-1);
					listePourIJ.Add(j);
				}
				if(!Physics.Raycast(positionInitiale,Vector3.right,longueurMurs)) //si pas de mur à gauche
				{
					listePourIJ.Add(i+1);
					listePourIJ.Add(j);
				}
				if(!Physics.Raycast(positionInitiale,Vector3.forward,longueurMurs)) //si pas de mur à gauche
				{
					listePourIJ.Add(i);
					listePourIJ.Add(j+1);
				}
				if(!Physics.Raycast(positionInitiale,Vector3.back,longueurMurs)) //si pas de mur à gauche
				{
					listePourIJ.Add(i);
					listePourIJ.Add(j-1);
				}
				listePourI.Add(listePourIJ);

				//Affichage pour debug dans la console
//				Debug.Log("Résultats pour i="+i+" et j="+j);
//				for(int k=0;k<listePourIJ.Count;k++)
//				{
//					Debug.Log (listePourIJ[k]+",");
//				}
			}
			matriceDeplacementsPossibles_.Add(listePourI);
		}
		SetMatriceDeplacementPossibles (matriceDeplacementsPossibles_);

		//Remettre les layers
		fpc.layer = fpcLayer;
		for (int i=0;i<lesPNJ.Count;i++) {
			lesPNJ[i].layer=pnjLayers[i];
		}
		for (int i=0;i<lesObjets.Count;i++) {
			lesObjets[i].layer=objetsLayers[i];
		}
	}

	public void PlacerPersonnages () {
		//Placement des PNJ
		for(int i=0;i<lesPNJ.Count;i++)
		{
			int iPNJ=Random.Range(3,nombreMursParCote-1); //Le PNJ ne doit au départ par etre trop près
			int jPNJ=Random.Range(0,nombreMursParCote-1);
			lesPNJ[i].transform.position = transform.position //d'après la formule des positions des cubes
				+ new Vector3 (longueurMurs*iPNJ, longueurMurs/2, longueurMurs*jPNJ+(float)longueurMurs/2+1/2);
			MazeTravel mw=lesPNJ[i].GetComponent<MazeTravel>();
			mw.Restart();
			mw.SetCoordinates(iPNJ,jPNJ);
		}
		//Mettre en place le fpc à l'entrée du labyrinthe
		fpc.transform.position = transform.position //d'après la formule des positions des cubes
			+ new Vector3 (0, longueurMurs/2, longueurMurs*jEntree+(float)longueurMurs/2+1/2);
		RemplirMatriceDeplacementsPossibles();
	}


	//Placement d'objets aléatoires sans aucune influence sur le jeu
	public void PlacerObjets () {
		for (int k=0;k<lesObjets.Count;k++)
		{
			int i=Random.Range(0,nombreMursParCote-1);
			int j=Random.Range(0,nombreMursParCote-1);
			lesObjets[k].transform.position = transform.position //d'après la formule des positions des cubes
				+ new Vector3 (longueurMurs*i, longueurMurs/2, longueurMurs*j+(float)longueurMurs/2+1/2);
		}
	}

	//Demande aux PNJ d'aller trouver le FPC en plus
	public void SetCoordFPC (int i_, int j_) {
		iFpc = i_;
		jFpc = j_;
		bool pnjOk = true;
		for(int i=0;i<lesPNJ.Count;i++)
		{
			//On dit aux PNJ de bouger
			bool pnjIOk=lesPNJ[i].GetComponent<MazeTravel>().MoveTo(iFpc,jFpc);
			//S'ils ne le peuvent pas, on doit régénérer le labyrinthe, replacer les personnages et arreter cette boucle
			if(!pnjIOk) pnjOk=false;
		}
		if(!pnjOk)
		{
			//On commence par désactiver les coroutines des PNJ pour éviter de faire redémarrer la génération en boucle
			for(int k=0;k<lesPNJ.Count;k++)
			{
				lesPNJ[k].GetComponent<MazeEnemy>().SetLabyrintheEnCoursDeRegeneration(true);
			}
			PlacerPersonnages();
			for(int k=0;k<lesPNJ.Count;k++)
			{
				lesPNJ[k].GetComponent<MazeEnemy>().SetLabyrintheEnCoursDeRegeneration(false);
			}
		}
	}

	public void SetMatriceDeplacementPossibles (List<List<List<int>>> matriceDeplacementsPossibles_) {
		matriceDeplacementsPossibles=matriceDeplacementsPossibles_;
		//actualisation de la matrice dans tous les MazeTravel
		for(int i=0;i<lesPNJ.Count;i++)
		{
			MazeTravel mw=lesPNJ[i].GetComponent<MazeTravel>();
			mw.SetMatriceDeplacementPossibles(matriceDeplacementsPossibles_);
		}
	}

	public int GetIFPC () {
		return iFpc;
	}
	
	public int GetJFPC() {
		return jFpc;
	}

	public int GetJEntree () {
		return jEntree;
	}
	
	public int GetJSortie() {
		return jSortie;
	}

	public int GetNombreMursParCote() {
		return nombreMursParCote;
	}
}
