/**
 * \file      MazeTravel.cs
 * \author    
 * \version   1.0
 * \date      18 septembre 2014
 * \brief     Permet aux PNJ de se déplacer dans un labyrinthe.
 *
 * \details   Ce script est lié à tous les PNJ qui vont devoir suivre le joueur dans le labyrinthe.
 * 		      Le script ne s'intéresse qu'à faire naviguer les PNJ, et pas les interactions avec le joueur.
 */

/*
 * Utilisé dans MazeManager
 */
/*
 * Lié à MazeManager qui l'initialise
 */


using UnityEngine;
using System.Collections.Generic;

public class MazeTravel : MonoBehaviour {
	//position du personnage non jouable et du fpc
	private int i;
	private int j;
	private int iFpc;
	private int jFpc; //Nécessaires pour relancer l'AES en cas de pb
	public float vitesse = 6.0F; //vitesse d'un déplacement élémentaire, fixé au départ à 6, vitesse par défaut du FPC
	//Variables récupérées par MazeManager.Start, nécessaires pour l'algorithme de déplacement
	private int longueurMurs; //la longueur est égale à la hauteur ici.
	private int nombreMursParCote;
	private Vector3 positionMaze;
	private bool isMoving = false;
	//Variables de déplacement
	private Vector3 positionProchainDeplacement;
	private Vector3 directionDeplacementCourant;
	private Vector3 positionAvantDeplacement;
	[HideInInspector] public List<int> mouvementsAFaire; //Contient les coordonnées i puis j de chaque mouvement à effectuer
	//Variables pour l'intelligence artificielle
	[HideInInspector] public List< List< List<int> > > matriceDeplacementsPossibles; //Pour chaque position i,j, indique quels sont les déplacements possibles sous la meme forme que les autres vecteurs de déplacement
	private bool algoEnCours = false;
	[HideInInspector] public List< List<bool > > casesParcourues; //Pour chaque position i,j, indique quels sont les déplacements possibles sous la meme forme que les autres vecteurs de déplacement

	void Start () {
		mouvementsAFaire = new List<int> () ;
		ReinitialiserCasesParcourues ();
	}

	/**
	 * @brief Régule le déplacement du PNJ à l'aide de Time.deltaTime
	 * 
	 * @details Le comportement du PNJ dépend du fait qu'il soit en mouvement ou non, mais également du fait qu'il ait une cible à atteindre ou pas. Ces comportements sont étudiés dans cette méthode.
	 */
	void FixedUpdate () {
		//Controle manuel
		//if(Input.GetKeyUp(KeyCode.DownArrow)){mouvementsAFaire.Add(i);mouvementsAFaire.Add(j-1);}
		//if(Input.GetKeyUp(KeyCode.LeftArrow)){mouvementsAFaire.Add(i-1);mouvementsAFaire.Add(j);}
		//if(Input.GetKeyUp(KeyCode.UpArrow)){mouvementsAFaire.Add(i);mouvementsAFaire.Add(j+1);}
		//if(Input.GetKeyUp(KeyCode.RightArrow)){mouvementsAFaire.Add(i+1);mouvementsAFaire.Add(j);}

		if(!isMoving) //Si on ne bouge pas, on lance le déplacement
		{
			if(!algoEnCours) //Si l'algo est en cours, on ne bouge pas
			{
				if(mouvementsAFaire.Count>1) //On vérifie que la liste des déplacement à faire n'est pas vide
				{
					//si le mouvement ne fait pas sortir du labyrinthe, on le fait
					if(mouvementsAFaire[0]>-1 && mouvementsAFaire[0]<nombreMursParCote-1
					   && mouvementsAFaire[1]>-1 && mouvementsAFaire[1]<nombreMursParCote-1
					   && !(mouvementsAFaire[0]==i && mouvementsAFaire[1]==j)) //ne pas aller au meme endroit
					{
						//on actualise la position du PNJ avant le déplacement pour anticiper sur l'algorithme
						if(DeplacementElementaire(mouvementsAFaire[0],mouvementsAFaire[1]))
						{
							//d'après la méthode de remplissage de la liste, les deux premiers nombres sont les coordonnées de la case cible
							i=mouvementsAFaire[0];
							j=mouvementsAFaire[1];
							isMoving=true; //On dit qu'on est en train de bouger pour ne pas lancer le prochain déplacement
						}
					}
					else //sinon on le dégage
					{
						mouvementsAFaire.RemoveRange(0,2);
					}
				}
//				else //On laisse le PNJ se balader librement
//				{
//					List<int> destinationsPossibles=matriceDeplacementsPossibles[i][j];
//					int position=Random.Range(0,(destinationsPossibles.Count)/2); //Il y a destinationsPossibles/2 choix possibles
//					if(destinationsPossibles.Count!=0) //Si des destinations sont accessibles
//					{
//						mouvementsAFaire.Add(destinationsPossibles[2*position]);
//						mouvementsAFaire.Add(destinationsPossibles[2*position+1]);
//					}
//				}
			}
		}
		else //Si on est en train de bouger, on continue le déplacement en faisant avancer le PNJ encore d'un pas
		{
			//Voici un cas particulier : si l'objet rate la position d'arrivée et commence à partir tout droit. C'est un problème. On va alors récupérer le PNJ et le mettre à la position qu'il devrait occuper.
			if((transform.position-positionAvantDeplacement).magnitude>longueurMurs+vitesse*Time.deltaTime)
			{
				transform.position=positionProchainDeplacement;
			}
			//Si on est très proche de la position d'arrivée, on arrete le mouvement. On prend un pas pour savoir si on est proche
			if((transform.position-positionProchainDeplacement).magnitude<vitesse*Time.deltaTime)
			{
				if(!algoEnCours){mouvementsAFaire.RemoveRange(0,2);} //On retire la position à laquelle on vient d'arriver
				//MoveTo(iFpc,jFpc);
				isMoving=false; //On ne bouge plus
			}
			else //Si on est encore relativement loin on peut faire un pas de plus
			{
				transform.Translate(directionDeplacementCourant*vitesse*Time.deltaTime, Space.World);
			}
		}
	}

	//
	/**
	 * @brief Remet à zéro l'algorithme et les vecteurs associés.
	 *
	 * @details Appelée lorsqu'on replace les PNJ après une mort
	 */
	public void Restart () {
		isMoving = false;
		algoEnCours = false;
		mouvementsAFaire = new List<int> () ;
		ReinitialiserCasesParcourues ();
	}

	/**
	 * @brief Remet à zéro casesParcourues.
	 *
	 * @details Remet en place une matrice remplie de "false" pour l'AES.
	 * 			Appelée dans Restart.
	 */
	public void ReinitialiserCasesParcourues () {
		List<List<bool>> casesParcourues_ = new List<List<bool>> ();
		for(int k=0;k<nombreMursParCote-1;k++)
		{
			List<bool> listeK = new List<bool>();
			for(int l=0;l<nombreMursParCote-1;l++)
			{
				listeK.Add(false);
			}
			casesParcourues_.Add (listeK);
		}
		casesParcourues = casesParcourues_;
	}

	/**
	 * @brief Fait bouger le PNJ d'une "case".
	 * @param nexti Abscisse de la prochaine position du PNJ
	 * @param nextj Ordonnée de la prochaine position du PNJ
	 *
	 * @details La méthode change les valeurs des positions avant et après déplacement pour qu'elles soient utilisées dans Update.
	 * 		    Elle change de meme la direction à suivre pour faire ce mouvement.
	 */
	public bool DeplacementElementaire (int nexti, int nextj) {
		if((Mathf.Abs(nexti-i)+Mathf.Abs(nextj-j))==1) //teste si le mouvement est légal
		{
			positionAvantDeplacement = transform.position;
			//La direction du déplacement est la position souhaitée moins la position initiale, le tout normalisé
			directionDeplacementCourant = new Vector3 (nexti-i, 0, nextj-j).normalized;
			positionProchainDeplacement = positionMaze //d'après la formule des positions des cubes
				+ new Vector3 (longueurMurs*nexti, .8f, longueurMurs*nextj+(float)longueurMurs/2+1/2);
			return true ; //C'est bon, on peut bouger
		}
		else //Le vecteur était corrompu, il faut relancer l'algorithme
		{
			MoveTo(iFpc,jFpc);
			return false; //Le mouvement était illégal
		}
			
	}

	/**
	 * @brief Permet le mouvement du PNJ jusqu'à la case donnée en argument.
	 * @param iFpc_ Abscisse de la position souhaitée du PNJ, souvent celle du FPC
	 * @param jFpc_ Ordonnée de la position souhaitée du PNJ
	 *
	 * @return bool True si l'algorithme a abouti, false sinon. Si true, la liste est bien remplie, sinon elle est vide.
	 * @details La méthode remplit la liste mouvementsAFaire à l'aide d'un algorithme à essais successifs.
	 * 			Appelé dans MazeManager::SetFPC
	 */
	public bool MoveTo (int iFpc_, int jFpc_) {
		if (vitesse == 0) //Si la vitesse est nulle, le PNJ ne peut pas bouger. On ne fait donc rien. On renvoie true car son déplacement, vide, est possible
						return true;

		iFpc = iFpc_;
		jFpc = jFpc_;
		ReinitialiserCasesParcourues (); //Réinitialiser le tout
		algoEnCours=true; //Mettre en place le flag pour empêcher le mouvement pendant le calcul des positions
		mouvementsAFaire=new List<int>(); //Réinitialiser la liste des mouvements à faire
		if(AES(iFpc,jFpc))
		{
			algoEnCours=false; //On peut rebouger
			return true;
		}
		else
		{
			algoEnCours=false; //On peut rebouger
			return false;
		}
	}

	/**
	 * @brief Vérifie que le prochain mouvement permet bien d'arriver à l'emplacement de la cible
	 *
	 * @return bool True si l'étape de l'algorithme a abouti, false sinon. Si true, la liste est bien remplie, sinon il faut en enlever les derniers éléments.
	 * @details La méthode implémente un algorithme à essais successifs pour trouver la position de la cible.
	 */
	private bool AES (int iFpc_, int jFpc_)
	{
		int tailleMouvementsAFaire = mouvementsAFaire.Count;
//		if (tailleMouvementsAFaire > 100)
//						return false; //On ne veut pas que l'algorithme dure trop longtemps
//		//Contiennent la position de laquelle on lance l'algorithme
		int iActuel;
		int jActuel;
		if(tailleMouvementsAFaire>1)
		{
			iActuel=mouvementsAFaire[tailleMouvementsAFaire-2];
			jActuel=mouvementsAFaire[tailleMouvementsAFaire-1];
		}
		else
		{
			iActuel=i;
			jActuel=j;
		}
		//Test de la fin de l'algorithme
		if(iActuel==iFpc_&&jActuel==jFpc_)
		{
			return true;
		}
		else
		{
			List<int> positionsAccessibles_=matriceDeplacementsPossibles[iActuel][jActuel];
			//On va trier positionAccessibles_ pour prendre en compte les mouvements les plus proches du FPC
			List<int> positionsAccessibles=new List<int>();
			List<int> sousListe1=new List<int>(); //éléments dans une direction du Fpc_
			List<int> sousListe2=new List<int>(); //éléments dans aucune bonne direction
			//On récupère les directions selon i et j de la cible
			int directionSelonI;
			int diffI=iFpc_-iActuel;
			if(diffI==0) directionSelonI=0;
			else if(diffI<0) directionSelonI=-1;
			else directionSelonI=1;
			int directionSelonJ;
			int diffJ=jFpc_-jActuel;
			if(diffJ==0) directionSelonJ=0;
			else if(diffJ<0) directionSelonJ=-1;
			else directionSelonJ=1;

			for(int k=0;k<(positionsAccessibles_.Count)/2;k++) //on regarde toutes les positions qui vont dans le sens du 
			{
				int iK=positionsAccessibles_[2*k];
				int jK=positionsAccessibles_[2*k+1];
				//Si l'élément est dans la bonne direction - possible seulement si l'une des directions est nulle
				if(iK-iActuel==directionSelonI && jK-jActuel==directionSelonJ){positionsAccessibles.Add(iK);positionsAccessibles.Add(jK);}
				//S'il est dans une bonne direction
				else if(iK-iActuel==directionSelonI || jK-jActuel==directionSelonJ){sousListe1.Add(iK);sousListe1.Add(jK);}
				else {sousListe2.Add(iK);sousListe2.Add(jK);}
			}
			//On remet les listes en place selon l'ordre de préférence
			positionsAccessibles.AddRange(sousListe1);
			positionsAccessibles.AddRange(sousListe2);

			//On regarde pour toutes les positions accessibles, contenues donc dans matriceDeplacementsPossibles
			for(int k=0;k<(positionsAccessibles.Count)/2;k++) //pour tous les elements
			{
				int iK=positionsAccessibles[2*k];
				int jK=positionsAccessibles[2*k+1];
//				Debug.Log (iK + " "+jK);
				//Si la case n'est pas visitée, on passe à l'étape suivante, si on ne fait rien
				if(!casesParcourues[iK][jK])
				{
					mouvementsAFaire.Add(iK);
					mouvementsAFaire.Add(jK);
					//Maintenant qu'on sait où on va, on dit que la case suivante est parcourue pour permettre de revenir sur la case qu'on vient de traverser si elle permet d'accéder rapidement au fpc
					casesParcourues [iK] [jK] = true;
					if(AES(iFpc_,jFpc_))
					{
						return true;
					}
					else
						mouvementsAFaire.RemoveRange(mouvementsAFaire.Count-2,2); //si on arrive ici, c'est qu'on n'est pas passe par le return, donc le chemin n'a pas abouti
						//casesParcourues[iK][jK]=false;
				}
			}
			return false; //Aucun des i n'a marche, le chemin n'aboutit pas
		}
	}
	
	public void SetCoordinates (int i_,int j_) {
		i = i_;
		j = j_;
	}

	//Récupère toutes les données de MazeManager nécessaires en un setter
	public void SetLongueurNombrePosition (int longueurMurs_,int nombreMursParCote_,Vector3 positionMaze_) {
		longueurMurs = longueurMurs_;
		nombreMursParCote = nombreMursParCote_;
		positionMaze = positionMaze_;
	}

	public void SetMatriceDeplacementPossibles (List<List<List<int>>> matriceDeplacementsPossibles_) {
		matriceDeplacementsPossibles=matriceDeplacementsPossibles_;
	}
}
