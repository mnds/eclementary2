/**
 * \file      SetTexte.cs
 * \author    
 * \version   1.0
 * \date      16 décembre 2014
 * \brief     Indique à ControlCenter que le gameObject sur lequel est placé ce script est le texte qui s'affichera pour les interactions.
 * 			  Envoie aussi contamment un Raycast à une certaine distance pour repérer les scripts AffichageTexteRaycast et les activer.
 * 
 * \details   Le gameObject doit avoir une instance de AffichageTexteRaycast avec pour texte "". De cette façon, s'il n'y a rien devant lui,
 * 			  le script demande à mettre le texte "" en non prioritaire
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AffichageTexteRaycast))] //Pour si l'objet devant n'en a pas, qu'on en active un quand meme
public class SetTexte : MonoBehaviour {
	public Camera cameraPrincipale=null;
	public float distanceAffichageTexte=8f;
	private AffichageTexteRaycast atrpropre;

	// Use this for initialization
	void Awake () {
//		Debug.Log ("Awake SetTexte");
		ControlCenter.SetTexte (gameObject.GetComponent<GUIText>());
	}

	void Start () {
//		Debug.Log ("Start SetTexte");
		ControlCenter.SetTexte (gameObject.GetComponent<GUIText>());
		if(cameraPrincipale==null)
			cameraPrincipale=Camera.main;
		if(distanceAffichageTexte<=0) //Remise à une valeur normale
			distanceAffichageTexte=8f;
		atrpropre=GetComponent<AffichageTexteRaycast>();
	}

	void Update () {
		//Tester si un objet est dans le champ de vision
		RaycastHit hitInfo;
		Pickable pickableGameObject;
		GameObject objet;
		
		if(Physics.Raycast(cameraPrincipale.transform.position, cameraPrincipale.transform.forward,out hitInfo, distanceAffichageTexte))				
		{
			objet = hitInfo.collider.gameObject;
//			Debug.Log ("Objet"+objet.name);
			//On récupère les textes de l'objet
			AffichageTexteRaycast[] atrs = objet.GetComponents<AffichageTexteRaycast>();
			//On les affiche tous, sachant que ceux-ci seront affichés ou pas selon les flags activés
			foreach(AffichageTexteRaycast atr in atrs)
				atr.ChangerTexte();
			if(atrs.Length==0) //Pas eu de truc à afficher
				atrpropre.ChangerTexte(); //Affichage texte défaut
		}
		else
			atrpropre.ChangerTexte ();
	}

	void OnLevelWasLoaded() {
		ControlCenter.SetTexte (gameObject.GetComponent<GUIText>());
	}
}
