/**
 * \file      MoveCamera.cs
 * \author    
 * \version   1.0
 * \date      12 novembre 2014
 * \brief     Déplace une caméra en la faisant passer par différents points.
 *
 * \details   Considère une liste de plusieurs points de l'espace et fait s'y déplacer la caméra en un temps donné par une liste donnée en attribut.
 * 			  Si un ControllerJoueur est renseigné, on bloque les mouvements du joueur.
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveCamera : MonoBehaviour {
	public Camera camera;
	public ControllerJoueur scriptFPC;
	public List<Vector3> positionsSuccessivesCamera;
	public List<Quaternion> rotationsSuccessivesCamera;
	public List<float> tempsPourArriverEnPosition;
	public float pas = 0.01f; //Rafraichissement du déplacement de la caméra
	private bool trajetEnCours = false; //Pour ne pas faire plusieurs fois le trajet

	void Update () {
		if (Input.GetKeyDown (KeyCode.L))
						EffectuerCoroutineTrajet ();
	}

	/**
	 * @brief Ajoute à la liste pSC la position actuelle.
	 * @param depuisCamera Si true, on relève la position de la caméra, si false, du GameObject qui contient ce script.
	 * @details Utilisée dans MoveCameraEditor
	 */
	public void AjouterTransform (bool depuisCamera) {
		//On crée des objets vides qui vont contenir la position à ajouter
		Vector3 nouvellePosition;
		Quaternion nouvelleRotation;

		if (depuisCamera && camera) { //On prend la caméra
			nouvellePosition = camera.transform.position;
			nouvelleRotation = camera.transform.rotation;
		}
		else { //On prend le parent
			nouvellePosition = gameObject.transform.position;
			nouvelleRotation = gameObject.transform.rotation;
		}

		positionsSuccessivesCamera.Add (nouvellePosition);
		rotationsSuccessivesCamera.Add (nouvelleRotation);
		tempsPourArriverEnPosition.Add (1f); //On ajoute le défaut : 1seconde
	}

	/**
	 * @brief Se charge d'appeler le coroutine correspondante
	 */
	public void EffectuerCoroutineTrajet () {
		if (!camera || trajetEnCours)
						return;
		StartCoroutine (EffectuerTrajet ());
	}

	/**
	 * @brief Coroutine effectuant le trajet entre tous les points renseignés.
	 */
	IEnumerator EffectuerTrajet () {
		Debug.Log ("Début trajet");
		trajetEnCours = true;
		ControlCenter.SetCinematiqueEnCours (true);
		//On conserve les positions locales de la caméra par rapport au joueur pour que la caméra y reste attachée
		Vector3 positionCameraAvantTrajet = camera.transform.localPosition;
		Quaternion rotationCameraAvantTrajet = camera.transform.localRotation;
		if (scriptFPC)
			scriptFPC.SetFreeze (true); //Bloque les mouvements du joueur
		for(int i=0;i<positionsSuccessivesCamera.Count;i++)
		{
			float tempsTrajet = 1f; //1 seconde par défaut
			if(tempsPourArriverEnPosition.Count>i) //Le champ i existe
				tempsTrajet=Mathf.Max (0f,tempsPourArriverEnPosition[i]); //Pour ne pas avoir un temps négatif
			//On déplace la caméra avec ses positions globales cette fois
			Vector3 positionInitiale = camera.transform.position;
			Quaternion rotationInitiale = camera.transform.rotation;
			Vector3 positionFinale = positionsSuccessivesCamera[i];
			Quaternion rotationFinale = rotationsSuccessivesCamera[i];

			float avancementTrajetActuel = 0f; //1 lorsque trajet fini
			while(avancementTrajetActuel<1f)
			{
				camera.transform.position=Vector3.Slerp(positionInitiale,positionFinale,avancementTrajetActuel);
				camera.transform.rotation=Quaternion.Slerp(rotationInitiale,rotationFinale,avancementTrajetActuel);
				yield return new WaitForSeconds(pas);
				avancementTrajetActuel = Mathf.Min (1f,avancementTrajetActuel+pas/tempsTrajet);
			}

		}
		//fin du trajet, on remet la camera à sa position initiale locale et on remet le FPC en marche
		camera.transform.localPosition = positionCameraAvantTrajet;
		camera.transform.localRotation = rotationCameraAvantTrajet;
		if (scriptFPC)
			scriptFPC.SetFreeze (false); //Remet les mouvements du joueur
		trajetEnCours = false;
		ControlCenter.SetCinematiqueEnCours (false);
	}
}
