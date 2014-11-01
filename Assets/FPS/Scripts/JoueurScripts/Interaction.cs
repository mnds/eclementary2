using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Doit etre placé sur le meme gameObject que le controller de mouvements du joueur
public class Interaction : MonoBehaviour {
	public Camera camera; //Main camera

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		ProcessInteraction ();
		string machin="c,c,c,c";
		string[] machins=machin.Split(',');
	}

	void ProcessInteraction () {
		//Tester si un objet est dans le champ de vision
		RaycastHit hitInfo; //On enverra un raycast
		Dialogue dialogueGameObject; //On cherche un dialogue
		GameObject objet; //Objet touché
		
		if (Input.GetButtonDown ("InteractionButton")) { //Si on cherche à interagir
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
					Debug.Log (dialogueGameObject.GetRepliqueActuelle());
					//S'il y a une replique actuelle, on l'arrete, c'est tout.
					if(dialogueGameObject.GetRepliqueActuelle())
						dialogueGameObject.ArreterRepliqueActuelle();
					else { //Sinon, on doit aller chercher quelle est la réplique à enclencher.
						Replique repliqueLancee; //Contient la replique à lancer
						List<Replique> repliquesAccessibles = dialogueGameObject.GetRepliquesAccessibles(); //Liste des repliques accessibles
						repliqueLancee = repliquesAccessibles[Random.Range(0,repliquesAccessibles.Count)]; //On en prend ici une au hasard et on la lance
						dialogueGameObject.LancerReplique(repliqueLancee); 
					}
				}
			}
		}
	}
}