/**
 * \file      TerrainSoundManager.cs
 * \author    
 * \version   1.0
 * \date      9 novembre 2014
 * \brief     Met un son d'ambiance lié à la texture du sol sur laquelle est l'objet.
 * 
 * \details   On récupère à chaque frame la texture sous le joueur, et on vérifie quel est le son associé. S'il y en a un, on verifie que le joueur se déplace.
 * 			  Si tel est le cas, on joue le son associé dans l'audioSource mise en attribut.
 */

/*
 * Code trouvé à http://answers.unity3d.com/questions/14998/how-can-i-perform-some-action-based-on-the-terrain.html
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainSoundManager : MonoBehaviour {
	public GameObject joueur; //Joueur
	Transform joueurTransform; //Pour la position
	FPCClassic joueurScript; //Pour relever la vitesse
	CharacterController joueurcc; //Pour savoir s'il est sur le sol

	Vector3 positionJoueur; //Position du joueur
	AudioClip clipActuel;
	public AudioSource sourceDuSon;

	public List<Texture> textures; //La liste des textures qui auront des sons associés
	public List<AudioClip> audioClips; //Liste des clips audio associés
	bool sonEnclenche = false; //Pour éviter d'envoyer plein de fois un son

	// Use this for initialization
	void Start () {
		joueurTransform = joueur.GetComponent<Transform> ();
		joueurScript = joueur.GetComponent<FPCClassic> ();
		joueurcc = joueur.GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		positionJoueur = joueurTransform.position; //On actualise la position
		Texture floorTexture = getTerrainTextureAt( positionJoueur ); //On récupère la texture au sol
		for (int i=0; i<textures.Count; i++) { //On la cherche
						if (textures [i] == floorTexture) //Si on la trouve
								clipActuel = audioClips [i]; //On change le clip audio
		}

		//Son
		if(clipActuel!=null && !sonEnclenche) {
			float vitesse = joueurScript.GetVitesseMouvement();
			if (joueurcc.isGrounded && joueurScript.GetVitesseNonVerticaleActuelle()>0) { //Si le joueur est au sol et qu'il se déplace
				sonEnclenche = true;
				sourceDuSon.clip=clipActuel;
				sourceDuSon.Play ();
				StartCoroutine(Attendre(4f/vitesse));
			}
		}
	}

	/**
	 * @brief Renvoie la texture sous la position donnée en argument.
	 * @param position Point de l'espace sous lequel on cherche la texture.
	 *
	 * @return La texture en question.
	 */
	public Texture getTerrainTextureAt( Vector3 position )
	{
		if (Terrain.activeTerrain == null)
						return null;
		// Set up:
		Texture retval = new Texture();
		Vector3 TS; // terrain size
		Vector2 AS; // control texture size
		TS = Terrain.activeTerrain.terrainData.size;
		AS.x = Terrain.activeTerrain.terrainData.alphamapWidth;
		AS.y = Terrain.activeTerrain.terrainData.alphamapHeight;
		// Lookup texture we are standing on:
		int AX = (int)( ( position.x/TS.x )*AS.x + 0.5f );
		int AY = (int)( ( position.z/TS.z )*AS.y + 0.5f );
		float[,,] TerrCntrl = Terrain.activeTerrain.terrainData.GetAlphamaps(AX, AY,1 ,1);
		TerrainData TD = Terrain.activeTerrain.terrainData;
		for( int i = 0; i < TD.splatPrototypes.Length; i++ )
		{
			if( TerrCntrl[0,0,i] > .5f )
			{
				retval = TD.splatPrototypes[i].texture;
			}
		}
		return retval;
	}

	public IEnumerator Attendre (float temps) {
		yield return new WaitForSeconds(temps);
		sonEnclenche = false;
	}

}
