/**
 * \file      DetectionPersonnes.cs
 * \author    
 * \version   1.0
 * \date      20 janvier 2015
 * \brief     Permet de détecter les PNJs et les Ennemis et d'afficher les points correspondants sur le radar
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DetectionPersonnes : MonoBehaviour {


	private GameObject[] listePnj; //La liste des PNJs
	private GameObject[] listeEnnemis; //La liste des ennemis
	private GameObject joueur;//Le joueur, bah oui ...
	public float distanceRadar;//La portée du radar
	public GameObject pointRouge;
	public GameObject pointJaune;
	private Vector2 vecteurJoueur;
	private Vector2 vecteurCamera;
	private Vector2 vecteurCible;

	void Start () 
	{
		joueur = ControlCenter.GetJoueurPrincipal ();//On récupère le joueur
		distanceRadar = 10f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		listePnj = GameObject.FindGameObjectsWithTag ("PNJ"); //On récupère tous les PNJs du jeu
		listeEnnemis = GameObject.FindGameObjectsWithTag ("Ennemi"); //On récupère tous les ennemis sur le terrain

		vecteurJoueur = new Vector2 (joueur.transform.position.x,joueur.transform.position.z);//La projection de la position du joueur
		vecteurCamera = new Vector2 (joueur.transform.forward.x,joueur.transform.forward.z);//La projection du vecteur de la caméra

		Vector2 vecteurReference = new Vector2(vecteurCamera.y,-vecteurCamera.x);//Un vecteur orthogonal au vecteur Camera (pour savoir si on est à gauche ou à droite)

		if(!(listePnj.Length == 0))//Si on trouve des PNJs
		{
			/*On parcourt la liste, et si le PNJ est 
			 * assez proche du joueur, on affiche un 
			 * point jaune sur le radar à une distance
			 * et une position correcte */

			for (int i=0;i<listePnj.Length;i++)
			{
				//On stocke la projection du vecteur de la cible
				vecteurCible = new Vector2(listePnj[i].transform.position.x,listePnj[i].transform.position.z);

				//On calcule la distance entre le joueur et le PNJ
				vecteurCible = vecteurJoueur-vecteurCible;
				float distance = vecteurCible.magnitude;

				//S'il est à portée
				if (distance < distanceRadar)
				{
					float angle=0;

					if(Vector2.Dot(vecteurReference, vecteurCible)>0f)//Si on est à gauche
					{
						angle = -Vector2.Angle(vecteurCamera, vecteurCible);
					}
					else //Si on est à droite
					{
						angle = Vector2.Angle(vecteurCamera, vecteurCible);
					}

					GameObject go = (GameObject)Instantiate(pointJaune);//On instancie le point jaune
					go.transform.parent=this.transform;//On lui met le bon parent

					go.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,vecteurCible.magnitude)/distanceRadar*100f;// On le positionne à la bonne distance, en face du joueur
					go.transform.RotateAround(this.transform.position,Vector3.forward,angle-180f);//On le fait tourner autour du centre du radar de "angle" degrés
					Destroy(go,Time.deltaTime);//On le détruit après un petit moment
				}
			}
		}

		if(!(listeEnnemis.Length == 0))//Si on trouve des Ennemis
		{
			/*On parcourt la liste, et si l'ennemi est 
			 * assez proche du joueur, on affiche un 
			 * point jaune sur le radar à une distance
			 * et une position correcte */
			
			for (int i=0;i<listeEnnemis.Length;i++)
			{
				//On stocke la projection du vecteur de la cible
				vecteurCible = new Vector2(listeEnnemis[i].transform.position.x,listeEnnemis[i].transform.position.z);
				
				//On calcule la distance entre le joueur et le PNJ
				vecteurCible = vecteurJoueur-vecteurCible;
				float distance = vecteurCible.magnitude;
				
				//S'il est à portée
				if (distance < distanceRadar)
				{
					float angle=0;
					
					if(Vector2.Dot(vecteurReference, vecteurCible)>0f)//Si on est à gauche
					{
						angle = -Vector2.Angle(vecteurCamera, vecteurCible);
					}
					else //Si on est à droite
					{
						angle = Vector2.Angle(vecteurCamera, vecteurCible);
					}
					
					GameObject go = (GameObject)Instantiate(pointRouge);//On instancie le point rouge
					go.transform.parent=this.transform;//On lui met le bon parent
					
					go.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,vecteurCible.magnitude)/distanceRadar*100f;// On le positionne à la bonne distance, en face du joueur
					go.transform.RotateAround(this.transform.position,Vector3.forward,angle-180f);//On le fait tourner autour du centre du radar de "angle" degrés
					Destroy(go,Time.deltaTime);//On le détruit après un petit moment
				}
			}
		}



	}
}
