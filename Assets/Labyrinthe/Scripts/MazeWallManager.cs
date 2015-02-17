/**
 * \file      MazeWallManager.cs
 * \author    
 * \version   1.0
 * \date      9 novembre 2014
 * \brief     Gère les murs du labyrinthe.
 *
 * \details   Fait tourner les murs sur eux-memes et garde l'angle avec lequel ils sont orientés.
 * 			  Configure les murs d'entrée et de sortie en leur donnant une scène d'arrivée.
 */

/*
 * Utilisé dans MazeManager
 */

using UnityEngine;
using System.Collections.Generic;

public class MazeWallManager : MonoBehaviour {
	private List<int> anglesPossibles;
	public int angleDeRotation = 0; //le mur est droit au départ
	[HideInInspector] public Vector3 axeRotation; //point par lequel passe l'axe de rotation
	public int i; //Position i selon MazeManager
	public int j; //Position j selon MazeManager
	//Pour les entree/sortie du labyrinthe
	private bool estMurEntree = false;
	private bool estMurSortie = false;
	private GameObject fpc;

	public void SetFpc(GameObject fpc_) {
		fpc = fpc_;
	}

	/**
	 * @brief Donne une rotation aléatoire au mur, choisie dans une liste.
	 */
	public void SetAxeRotation () {
		axeRotation = transform.position - new Vector3 (2.5f, 0.0f, 0.0f);
		anglesPossibles = new List<int> () {0,90,90,180,180,270,270};
		int nombreRandom = Random.Range (0,anglesPossibles.Count-1); //nombre aléatoire
		int angle = anglesPossibles[nombreRandom]; //angle aléatoire dans la liste proposée
		Tourner (angle);
	}

	/**
	 * @brief Tourne le mur d'un certain angle selon un axe orienté verticalement.
	 * @param angle L'angle avec lequel tourner. Cet angle est absolu ; on retranche l'angle de rotation actuel.
	 */
	public void Tourner (int angle) {
		//Fait tourner le mur selon un axe vertical situé au centre d'une de ses largeurs
		transform.RotateAround(axeRotation,Vector3.up,
		                       angle-angleDeRotation); //retirer l'angle de rotation initial et rajouter le nouveau
		angleDeRotation = angle; //actualiser la valeur de l'angle de rotation
	}

	/**
	 * @brief Configure le mur d'entrée.
	 */
	public void SetMurEntree () {
		estMurEntree = true;
		estMurSortie = false;
		collider.isTrigger = true; //pouvoir passer à travers
		gameObject.renderer.enabled = false; //voir à travers
	}

	/**
	 * @brief Configure le mur de sortie.
	 */
	public void SetMurSortie () {
		estMurEntree = false;
		estMurSortie = true;
		collider.isTrigger = true; //pouvoir passer à travers
		gameObject.renderer.enabled = false; //voir à travers
	}

	/**
	 * @brief Change de scène si le joueur passe par l'un des murs d'entrée ou sortie.
	 */
	void OnTriggerEnter (Collider collider) {
		if(collider.gameObject==fpc) //Si le fpc rentre
		{
			if(estMurEntree) { //On revient à la scène du campus
				//Application.LoadLevel("Lancement");
				gameObject.AddComponent<ChangementSceneFlagOnTrigger>();
				gameObject.GetComponent<ChangementSceneFlagOnTrigger>().listeDesFlagsPouvantEtreActives=new List<int>(){};
				gameObject.GetComponent<ChangementSceneFlagOnTrigger>().activationPossibleDesFlags=new List<bool>(){true};
				ChangementSceneFlagOnTrigger.FlagsRequisInterditsChangementSceneFlag fricsf = new ChangementSceneFlagOnTrigger.FlagsRequisInterditsChangementSceneFlag(ControlCenter.Scenes.Scolarite,"EntreeLaby");
				gameObject.GetComponent<ChangementSceneFlagOnTrigger>().nomsDesScenesAccessibles=new List<ChangementSceneFlagOnTrigger.FlagsRequisInterditsChangementSceneFlag>(){fricsf};
			}
			if(estMurSortie) {
				//Application.LoadLevel("CampusExterieur");
				gameObject.AddComponent<ChangementSceneFlagOnTrigger>();
				gameObject.GetComponent<ChangementSceneFlagOnTrigger>().listeDesFlagsPouvantEtreActives=new List<int>(){420}; //On peut activer le 420
				gameObject.GetComponent<ChangementSceneFlagOnTrigger>().activationPossibleDesFlags=new List<bool>(){true};
				ChangementSceneFlagOnTrigger.FlagsRequisInterditsChangementSceneFlag fricsf = new ChangementSceneFlagOnTrigger.FlagsRequisInterditsChangementSceneFlag(ControlCenter.Scenes.BureauDebouck,"EntreeBureauDebouck");
				gameObject.GetComponent<ChangementSceneFlagOnTrigger>().nomsDesScenesAccessibles=new List<ChangementSceneFlagOnTrigger.FlagsRequisInterditsChangementSceneFlag>(){fricsf};
			}
		}
	}

	public void SetCoordinates (int i_,int j_) {
		i = i_;
		j = j_;
	}
}