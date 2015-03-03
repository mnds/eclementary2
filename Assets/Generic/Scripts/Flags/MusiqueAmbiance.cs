/**
 * \file      MusiqueAmbiance.cs
 * \author    
 * \version   1.0
 * \date      2 mars 2015
 * \brief     Attaché au script qui déclenchera les musiques d'ambiance
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//NOTE IMPORTANTE : NE PAS METTRE DE FLAGS POUR LA MUSIQUE, COMME CA TOUTES LES MUSIQUES SERONT TESTEES TOUT LE TEMPS
public class MusiqueAmbiance : ActionSelonFlags {
	//lorsque Verifier est appelée, on vérifie toutes les musiques. On vérifie si on est dans la bonne scène,
	//si les flags sont bons, et si c'est le cas, on la lance.
	//Ce sont des playlist, elles peuvent pointer vers plusieurs musiques si elles peuvent etre jouées aléatoirement. 
	[System.Serializable]
	public class MusiqueAmbianceSeule {
		public List<AudioClip> audioClips; //Remplie dans l'inspecteur
		private bool musiqueSelectionnee = false; //Passe à true quand la musique doit etre jouée.
		public ControlCenter.Scenes sceneDeLaMusique; //Complétée dans l'inspecteur
		public List<int> flagsDevantEtreActives;
		public List<int> flagsDevantEtreDesactives;
		public string description; //Aide pour les joueurs

		public bool GetMusiqueSelectionnee () {
			return musiqueSelectionnee;
		}
		public List<AudioClip> GetAudioClips () {
			return audioClips;
		}

		/**
		 * @brief Verifie que les flags des deux listes en attribut sont bien aux bons états
	     */
		public void Verifier () {
			foreach(int idFlagA in flagsDevantEtreActives) {
				if(!FlagManager.ChercherFlagParId(idFlagA).actif) { //Pas activé, problème
					ActionSiMauvaisFlag();
					return; //ca ne sert plus à rien
				}
			}
			foreach(int idFlagA in flagsDevantEtreDesactives) {
				if(FlagManager.ChercherFlagParId(idFlagA).actif) { //Pas activé, problème
					ActionSiMauvaisFlag();
					return; //ca ne sert plus à rien
				}
			}
			//Tout est bon
			ActionSiBonFlag();
		}

		protected void ActionSiBonFlag() {
			string nomSceneActuelle = Application.loadedLevelName;

			if(nomSceneActuelle==ControlCenter.SceneToString(sceneDeLaMusique)) {
				musiqueSelectionnee=true; //La scène et les flags sont bons
			}
			else {
				musiqueSelectionnee=false;
			}
		}
		
		protected void ActionSiMauvaisFlag() {
			musiqueSelectionnee=false;
		}
	}

	private AudioSource audioS; //Source du son
	private List<AudioClip> acs = null; //Musique actuelle, chargée dans Verifier
	public List<MusiqueAmbianceSeule> lesMASs = new List<MusiqueAmbianceSeule>(){}; //Liste de toutes les musiques d'ambiance possibles et quand elles s'activent
	private int compt; //Numéro de changement de playlist. De cette façon, on sait quelle playlist a fait quels appels aux IEnumerator, et une playlist ne peut pas changer la musique
					//alors qu'une autre est en train d'etre jouée.

	void Awake() {
		//On commence par assigner audioS
		if(!audioS) audioS=gameObject.GetComponent<AudioSource>();
	}

	void OnLevelWasLoaded () {
		//On commence par assigner audioS
		if(!audioS) audioS=gameObject.GetComponent<AudioSource>();
		Verifier (); //A chaque changement de scène, on vérifie quelle musique doit etre lancée.
	}

	/**
	 * @brief Teste toutes les musiques d'ambiance pour lancer la bonne
	 **/
	override protected void ActionSiBonFlag() {
		bool musiqueTrouvee = false;
		//On cherche la musique d'ambiance à lancer
		foreach(MusiqueAmbianceSeule mas in lesMASs) {
			mas.Verifier(); //On vérifie
			if(mas.GetMusiqueSelectionnee()) { //On cherche la bonne musique à jouer
				musiqueTrouvee = true; //On a une musique à lire
				if(acs==mas.GetAudioClips()) {
					return; //La liste est déjà en cours de lecture
				}
				compt++; //Changement de playlist !
				acs=mas.GetAudioClips(); //On change la liste de lecture
				break;
			}//Si c'est bon, ok
		}

		//Si on ne trouve aucune musique, on arrete tout
		if(!musiqueTrouvee)
		{
			acs=new List<AudioClip>(){}; //Liste vide
			audioS.Stop (); //On arrete le clip
			return;
		}

		//On choisit un clip au hasard
		AudioClip ac=acs[Random.Range (0,acs.Count)];
		float duree=ac.length+1; //Cooldown
		
		//On le lit
		Debug.Log ("Ac : "+ac);
		audioS.Stop();
		audioS.PlayOneShot(ac);
		//On attend le temps que le clip se finisse pour en envoyer un autre.
		StartCoroutine(AttendreFinSon(duree,compt));
	}

	/**
	 * @brief Renvoie un message dans la console pour dire qu'il n'y a pas de son
	 **/
	override protected void ActionSiMauvaisFlag() {
		Debug.Log("Ne pas mettre de flags dans MusiqueAmbiance, attaché à "+gameObject.name);
	}

	/**
	 * @brief Attend que le son actuel soit fini pour lancer un autre clip
	 * @param duree Temps à attendre avant le changement
	 * @param compt_ Numéro d'appel à un changement de playlist au début du yield
	 * 
	 * @details Quand une musique est lancée, il faut attendre qu'elle se finisse pour la changer ; mais si la playlist a changé ?
	 * 			On abandonne alors cette idée.
	 */
	public IEnumerator AttendreFinSon(float duree, int compt_) {
		yield return new WaitForSeconds(duree);
		if(compt==compt_) //On est toujours sur la meme playlist, on peut la faire tourner
			ChangerSon ();
		//Sinon, ça a changé, ce yield ne sert plus à rien
	}
	
	/**
	 * @brief Change de son dans la playlist déjà sélectionnée.
	 * @details Prend un clip au hasard, le lance et appelle la coroutine d'attente. Rien ne se passe si un clip
	 * 			est encore en train de tourner, ce qui arrive si ChangerClipAmbiance est appelé. Cette fonction
	 * 			permet de faire un loop des musiques.
	 */
	private void ChangerSon() {
		if(audioS.isPlaying) return; //Pour empecher de couper alors que la chanson tourne
		AudioClip ac=acs[Random.Range (0,acs.Count)];
		float duree=ac.length;
		audioS.PlayOneShot(ac);
		StartCoroutine(AttendreFinSon(duree,compt));
	}
}
