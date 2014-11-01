using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Doit etre placé sur le meme gameObject que le controller de mouvements du joueur
public class Interaction : MonoBehaviour {
	public Camera camera; //Main camera
	List<Replique> repliquesAffichees; //Les répliques à afficher à l'écran pour que le joueur en choisisse une
	public Dialogue dialogueJoueur;

	// Use this for initialization
	void Start () {
		repliquesAffichees = new List<Replique> (){};
	}
	
	// Update is called once per frame
	void Update () {
		ProcessInteraction ();
	}

	void ProcessInteraction () {
		//Tester si un objet est dans le champ de vision
		RaycastHit hitInfo; //On enverra un raycast
		Dialogue dialogueGameObject; //On cherche un dialogue
		GameObject objet; //Objet touché
		
		if (Input.GetButtonDown ("InteractionButton")) { //Si on cherche à interagir
			//On vérifie en priorité si le joueur est en train de parler. Si oui on l'arrete.
			if(dialogueJoueur) { //S'il y a un dialogue pour le joueur
				if(dialogueJoueur.GetRepliqueActuelle()) {
					dialogueJoueur.ArreterRepliqueActuelle();
					return; //On s'arrete
				}
			}

			//Ensuite, on regarde ce qu'il y a devant o
			if(Physics.Raycast(camera.transform.position, camera.transform.forward,out hitInfo, 300f)) //On regarde très loin pour trouver l'objet en face du joueur
			{
				objet = hitInfo.collider.gameObject; //On récupère l'objet touché
				GameObject copieObjet = objet; //On le copie pour le manipuler
				dialogueGameObject = copieObjet.GetComponent<Dialogue>(); //On cherche si l'objet touché en lui-meme
				//On cherche dans les parents de l'objet
				while(dialogueGameObject == null && objet.transform.parent)
				{
					copieObjet=copieObjet.transform.parent.gameObject;
					dialogueGameObject = copieObjet.GetComponent<Dialogue>();
				}
				//Sinon, on est à la racine, donc on cherche dans les enfants.
				if(dialogueGameObject==null)
					dialogueGameObject=copieObjet.GetComponentInChildren<Dialogue>();
				
				//On vérifie qu'on a trouvé un dialogue, que le dialogue est accessible, et qu'on est assez près
				if(dialogueGameObject!=null && dialogueGameObject.GetInteractionPossible()
				   && dialogueGameObject.GetDistanceMinimaleInteraction()>Vector3.Distance(hitInfo.point,camera.transform.position))
				{
					//S'il y a une replique actuelle, on l'arrete, c'est tout.
					if(dialogueGameObject.GetRepliqueActuelle())
						dialogueGameObject.ArreterRepliqueActuelle();
					else {
						Replique repliqueLancee; //Contient la replique à lancer
						List<Replique> repliquesAccessibles = dialogueGameObject.GetRepliquesAccessibles(); //Liste des repliques accessibles
						if(repliquesAccessibles!=null) { //S'il y a des repliques accessibles
							dialogueGameObject.Trigger (this);
						}
						 
					}
				}
			}
		}
	}
}