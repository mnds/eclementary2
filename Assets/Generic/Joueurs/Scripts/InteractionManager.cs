/**
 * \file      InteractionManager.cs
 * \author    
 * \version   1.0
 * \date      9 novembre 2014
 * \brief     Script général d'interaction avec les objets contenant Interactif.cs.
 *
 * \details   Vérifie si un objet est en face du joueur quand il appuie sur la touche d'interaction.
 * 			  Si cet objet a un script Interactif, on appelle sa méthode DemarrerInteraction.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Doit etre placé sur le meme gameObject que le controller de mouvements du joueur
public class InteractionManager : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		ProcessInteraction ();
	}
	
	void ProcessInteraction () {
		//Tester si un objet est dans le champ de vision
		RaycastHit hitInfo; //On enverra un raycast
		List<Interactif> interactifs = new List<Interactif>(){};
		GameObject objet; //Objet touché

		if (Input.GetButtonDown ("InteractionButton")) { //Si on cherche à interagir
			// On regarde ce qu'il y a devant
			if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,out hitInfo, 300f)) //On regarde très loin pour trouver l'objet en face du joueur
			{
				objet = hitInfo.collider.gameObject; //On récupère l'objet touché
				GameObject copieObjet = objet; //On le copie pour le manipuler

				Component[] interaction;
				interaction=copieObjet.GetComponents(typeof(Interactif));
				foreach(Component c in interaction) {
					interactifs.Add ((Interactif)c);
				}

				//On cherche dans les parents de l'objet
				while(interactifs == null && objet.transform.parent)
				{
					copieObjet=copieObjet.transform.parent.gameObject;
					interaction=copieObjet.GetComponents(typeof(Interactif));
					foreach(Component c in interaction) {
						interactifs.Add ((Interactif)c);
					}
				}
				//Sinon, on est à la racine, donc on cherche dans les enfants.
				if(interactifs==null) {
					interaction=copieObjet.GetComponentsInChildren(typeof(Interactif));
					foreach(Component c in interaction) {
						interactifs.Add ((Interactif)c);
					}
				}
				
				//On vérifie qu'on a trouvé un script d'interaction et qu'on est assez près
				if(interactifs.Count>0)
					Debug.Log ("Démarrage d'une interaction avec"+copieObjet.name);

				foreach(Interactif i in interactifs) {
					if(i!=null
					   && i.GetDistanceMinimaleInteraction()>Vector3.Distance(hitInfo.point,Camera.main.transform.position))
					{
						i.DemarrerInteraction();
					}
				}
			}
		}
	}
}