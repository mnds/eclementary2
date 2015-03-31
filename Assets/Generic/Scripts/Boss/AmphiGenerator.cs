/**
 * \file      AmphiGenerator.cs
 * \author    
 * \version   1.0
 * \date      7 mars 2015
 * \brief     Permet de créer les dalles au sol pendant le combat dans l'amphi.
 */

using UnityEngine;
using System.Collections;

public class AmphiGenerator : MonoBehaviour {
	public Vector3 coinSuperieurGauche; //D'où ça commence
	public GameObject tile; //Prefab de la tile
	public GameObject parentTiles; //Pour ne pas tout mettre n'importe où dans la vue Projet
	//taille de la grille
	public int nombreTilesX;
	public int nombreTilesZ;
	//taille d'une case
	public float tailleX;
	public float tailleZ;
	public float hauteurY;

	/**
	 * @brief Détruis tous les enfants du GameObject parentTiles.
	 * @details Est appelée par CreerTiles
	 */
	public void ClearParentTiles () {
		int nombreEnfants = parentTiles.transform.childCount;
		for (int i = nombreEnfants - 1; i >= 0; i--) {
			GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
		}
	}

	/**
	 * @brief Crée le damier de l'amphi. S'utilise dans l'inspecteur.
	 */
	public void CreerTiles () {
		//On commence par détruire tous les enfants du contenant
		ClearParentTiles ();

		for(int i=0;i<nombreTilesX;i++) {
			for(int j=0;j<nombreTilesZ;j++) {
				//Créer tile du haut
				GameObject tileHaut = Instantiate(tile,coinSuperieurGauche
				              +new Vector3(i*tailleX,0f,j*tailleZ),
				              Quaternion.identity) as GameObject;
				tileHaut.GetComponent<HealthTileAmphi>().SetTypeTile(HealthTileAmphi.TypeTile.PremiereCouche);
				tileHaut.transform.parent=parentTiles.transform;
				//Créer tile du milieu
				/*GameObject tileMilieu = Instantiate(tile,coinSuperieurGauche
				             +new Vector3(i*tailleX,-hauteurY,j*tailleZ),
				             Quaternion.identity) as GameObject;
				tileMilieu.GetComponent<HealthTileAmphi>().SetTypeTile(HealthTileAmphi.TypeTile.DeuxiemeCouche);
				tileMilieu.transform.parent=parentTiles.transform;*/
			}
		}

		/*//Créer les dernières tiles
		int offset = 20; //Décalage pour l'inertie
		for(int i=-offset/2;i<nombreTilesX+offset/2;i++) { //décalage pour l'inertie
			for(int j=-offset/2;j<nombreTilesZ+offset/2;j++) {
				//Créer tile mortelle
				GameObject tileFinale = Instantiate(tile,coinSuperieurGauche
				                                  +new Vector3(i*tailleX,-5*hauteurY,j*tailleZ),
				                                  Quaternion.identity) as GameObject;
				tileFinale.GetComponent<HealthTileAmphi>().SetTypeTile(HealthTileAmphi.TypeTile.QuatriemeCouche);
				tileFinale.transform.parent=parentTiles.transform;
			}
		}*/
	}
}
