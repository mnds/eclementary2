/**
 * \file      MoveCamera.cs
 * \author    
 * \version   1.0
 * \date      12 novembre 2014
 * \brief     Déplace une caméra en la faisant passer par différents points.
 *
 * \details   Considère une liste de plusieurs points de l'espace et fait s'y déplacer la caméra en un temps donné par une liste donnée en attribut.
 * 			  Si un FPCClassic est renseigné, on bloque les mouvements du joueur.
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveCamera : MonoBehaviour {
	public Camera camera;
	public FPCClassic scriptFPC;
	public List<Transform> positionsSuccessivesCamera;
	public List<float> tempsPourArriverEnPosition;
	public float pas = 0.01f; //Rafraichissement du déplacement de la caméra
	private bool trajetEnCours = false; //Pour ne pas faire plusieurs fois le trajet

	/**
	 * @brief Coroutine effectuant le trajet entre tous les points renseignés.
	 */
	public IEnumerator EffectuerTrajet () {
		if (!camera || trajetEnCours) //S'il n'y a pas de caméra pour que celle-ci est en train de bouger
						yield return new WaitForEndOfFrame ();
		trajetEnCours = true;
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
			Vector3 positionFinale = positionsSuccessivesCamera[i].position;
			Quaternion rotationFinale = positionsSuccessivesCamera[i].rotation;

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
	}
}
